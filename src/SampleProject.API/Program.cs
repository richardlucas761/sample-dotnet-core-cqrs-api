using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace SampleProject.API
{
    public static class Program
    {
        public static void Main()
        {
            using var host = new HostBuilder()
                .ConfigureWebHost(webHostBuilder =>
                {
                    webHostBuilder.UseStartup<Startup>();
                })
                .Build();

            host.Run();
        }
    }
}
