using ChatServer.Services;
using Data;
using Data.IoC;

namespace ChatServer.Configuration;

public static class DependencyInjectionConfig
{
    public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddScoped<IContactService, ContactService>();
        services.AddScoped<IChatMessageService, ChatMessageService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        NativeInjectorBootStrapper.RegisterServices(services);
    }
}