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

            var client = new UdpClient("10.85.112.10", 6000);

            Thread.Sleep(5000);

            logger.LogInformation("Starting transmission to {endpoint}", client.Client.RemoteEndPoint);
            while (!stoppingToken.IsCancellationRequested)
            {
                // Simulate sending data

                var now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                var messageBytes = BitConverter.GetBytes(now);

                await client.SendAsync(messageBytes, stoppingToken);


                logger.LogInformation("Pinging...");

                Thread.Sleep(1000); // Simulate work
            }
        }
    }
}
