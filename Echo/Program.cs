using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Echo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddHostedService<EchoService>();


            var app = builder.Build();

            app.Run();

        }
    }
}
