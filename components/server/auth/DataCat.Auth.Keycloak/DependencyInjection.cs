using DataCat.Auth.Keycloak.Jobs;

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
        
        services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication"));
        services.ConfigureOptions<JwtBearerOptionsSetup>();
        
        services.Configure<KeycloakOptions>(configuration.GetSection("Keycloak"));
        
        services.AddHttpClient<IJwtService, JwtService>((serviceProvider, httpClient) =>
        {
            var keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;

            httpClient.BaseAddress = new Uri(keycloakOptions.TokenUrl);
        });
        
        services.AddHttpClient<UserSynchronizationJob>((serviceProvider, httpClient) =>
        {
            var keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;

            httpClient.BaseAddress = new Uri(keycloakOptions.BaseUrl);
        });
        
        services.AddHttpClient<UserRoleSynchronizationJob>((serviceProvider, httpClient) =>
        {
            var keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;

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
        
        return services;
    }
}