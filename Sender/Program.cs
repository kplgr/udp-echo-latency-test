using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Sockets;

namespace Sender
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddSingleton(new UdpClient("10.85.112.10", 6000));

            builder.Services.AddHostedService<SenderService>();
            builder.Services.AddHostedService<ReceiverService>();

            var app = builder.Build();

            app.Run();
        }
    }
}
