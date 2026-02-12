using _3DHISTECHhomework.Server.Domain.Interface;
using _3DHISTECHhomework.Server.Service.ImageProcess;

namespace _3DHISTECHhomework.Server.Service.Injection;

public static class Inject
{
    public static void AddService(IServiceCollection services)
    {
        services.AddScoped<IImageProcessor, ImageProcessor>();
    }
}
