using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;

namespace Sender
{
    internal class SenderService(ILogger<SenderService> logger, UdpClient client) : BackgroundService
    {
        private readonly ILogger<SenderService> logger = logger;
        private readonly UdpClient client = client;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
           

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
