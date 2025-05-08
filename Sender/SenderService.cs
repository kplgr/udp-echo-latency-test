using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;

namespace Sender
{
    internal class SenderService(ILogger<SenderService> logger) : BackgroundService
    {
        private readonly ILogger<SenderService> logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            const string address = "10.85.112.20";
            const int port = 6000;

            var client = new UdpClient(address, port);


            Thread.Sleep(5000);

            logger.LogInformation("Starting transmission to {address} @ {port}", address, port);
            while (!stoppingToken.IsCancellationRequested)
            {
                // Simulate sending data

                var now = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                await client.SendAsync(BitConverter.GetBytes(now));


                Thread.Sleep(1000); // Simulate work
            }
        }
    }
}
