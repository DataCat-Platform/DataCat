using Asp.Versioning;

namespace DataCat.Server.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiSetup(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        services.ConfigureOptions<ConfigureSwaggerOptions>();
        
        services
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });
        
        var serviceDescriptors = typeof(Program).Assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(ApiEndpointBase)))
            .Select(type => ServiceDescriptor.Transient(typeof(ApiEndpointBase), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        // services.AddControllers()
        //     .AddJsonOptions(options =>
        //     {
        //         options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        //         options.JsonSerializerOptions.Converters.Add(
        //             new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, false));
        //     });

        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        
        return services;
    }
    
    public static IApplicationBuilder MapApiEndpoints(
        this WebApplication app,
        RouteGroupBuilder? routeGroupBuilder = null)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<ApiEndpointBase>>();

        IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;
        builder = builder.NewVersionedApi("DataCatEndpointsBuilder")
            .HasApiVersion(ApiVersions.V1);
        
        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        return app;
    }
    
    public static IServiceCollection AddCustomMiddlewares(this IServiceCollection services)
    {
        services
            .AddSingleton<ExceptionHandlingMiddleware>()
            .AddSingleton<RequestLoggingMiddleware>()
            .AddTransient<AuthorizationMiddleware>();

        return services;
    }
}