namespace DataCat.Auth.Keycloak;

public static class DependencyInjection
{
    public static IServiceCollection AddKeycloakAuth(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();
        
        services.AddTransient<IClaimsTransformation, DataCatKeycloakClaimsTransformation>();
        services.AddScoped<IIdentity, KeycloakIdentity>();

        services.AddSingleton<KeycloakRequestBuilder>();

        services.AddScoped<IOidcRedirectService, KeycloakOidcRedirectService>();
        
        services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication"));
        services.ConfigureOptions<JwtBearerOptionsSetup>();
        
        services.Configure<KeycloakOptions>(configuration.GetSection("Keycloak"));
        services.AddSingleton<KeycloakOptions>(sp =>
        {
            var option = sp.GetRequiredService<IOptions<KeycloakOptions>>().Value;
            var adminSecretPath = option.AdminClientSecret;
            var authClientSecretPath = option.AuthClientSecret;

            var secretsProvider = sp.GetRequiredService<ISecretsProvider>();
            
            var adminSecret = secretsProvider.GetSecretAsync(adminSecretPath).GetAwaiter().GetResult();
            var authClientSecret = secretsProvider.GetSecretAsync(authClientSecretPath).GetAwaiter().GetResult();
            
            return option with { AdminClientSecret = adminSecret, AuthClientSecret = authClientSecret };
        });
        
        services.AddHttpClient<IJwtService, JwtService>((serviceProvider, httpClient) =>
        {
            var keycloakOptions = serviceProvider.GetRequiredService<KeycloakOptions>();

            httpClient.BaseAddress = new Uri(keycloakOptions.TokenUrl);
        });
        
        services.AddHttpClient<UserSynchronizationJob>((serviceProvider, httpClient) =>
        {
            var keycloakOptions = serviceProvider.GetRequiredService<KeycloakOptions>();

            httpClient.BaseAddress = new Uri(keycloakOptions.BaseUrl);
        });
        
        services.AddHttpClient<UserRoleSynchronizationJob>((serviceProvider, httpClient) =>
        {
            var keycloakOptions = serviceProvider.GetRequiredService<KeycloakOptions>();

            httpClient.BaseAddress = new Uri(keycloakOptions.BaseUrl);
        });
        
        services.AddQuartz(q =>
        {
            #region UserSynchronizationJob
            var userSynchronizationJobKey = new JobKey("UserSynchronizationJob");
            q.AddJob<UserSynchronizationJob>(opts => opts.WithIdentity(userSynchronizationJobKey));
    
            q.AddTrigger(opts => opts
                .ForJob(userSynchronizationJobKey)
                .WithIdentity("UserSynchronizationJob-trigger")
                .WithSimpleSchedule(action =>
                {
                    action.WithIntervalInSeconds(20).RepeatForever();
                })
            );
            #endregion
            
            #region UserRoleSynchronizationJob
            var userRoleSynchronizationJobKey = new JobKey("UserRoleSynchronizationJob");
            q.AddJob<UserRoleSynchronizationJob>(opts => opts.WithIdentity(userRoleSynchronizationJobKey));
    
            q.AddTrigger(opts => opts
                .ForJob(userRoleSynchronizationJobKey)
                .WithIdentity("UserRoleSynchronizationJob-trigger")
                .WithSimpleSchedule(action =>
                {
                    action.WithIntervalInSeconds(20).RepeatForever();
                })
            );
            #endregion
        });

        services.AddSingleton<KeycloakMetricsContainer>();

        services.AddOpenTelemetry()
            .ConfigureResource(configure => configure
                .AddService(KeycloakMetricsConstants.ServiceName))
            .WithMetrics(configure => configure
                .AddMeter(KeycloakMetricsConstants.MeterName));
        
        return services;
    }
}