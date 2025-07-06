namespace CartService.API.Auth;

public static class AuthExtensions
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var authOptions = configuration.GetSection(AuthOptions.SectionName).Get<AuthOptions>();
        
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = authOptions!.Authority;
                options.RequireHttpsMetadata = authOptions!.RequireHttpsMetadata;
                options.Audience = authOptions!.Audience;
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthPolicy.CanRead, policy =>
                policy.RequireClaim("permissions", "read"));

            options.AddPolicy(AuthPolicy.CanCreate, policy =>
                policy.RequireClaim("permissions", "create"));

            options.AddPolicy(AuthPolicy.CanUpdate, policy =>
                policy.RequireClaim("permissions", "update"));

            options.AddPolicy(AuthPolicy.CanDelete, policy =>
                policy.RequireClaim("permissions", "delete"));
        });

        return services;
    }
}
