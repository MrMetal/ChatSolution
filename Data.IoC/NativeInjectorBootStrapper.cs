using Microsoft.Extensions.DependencyInjection;

namespace Data.IoC;

public class NativeInjectorBootStrapper
{
    public static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
    }
}