using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;

namespace Echo
{
    internal class EchoService(ILogger<EchoService> logger) : BackgroundService
    {
        private readonly ILogger<EchoService> logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int port = 6000;


            var client = new UdpClient(port);

            logger.LogInformation("Starting echo server, listening on port {port}", port);

            while (!stoppingToken.IsCancellationRequested)
            {
                var result = await client.ReceiveAsync(stoppingToken);
                var receivedBytes = result.Buffer;

                var senderTransmissionTime = BitConverter.ToInt64(receivedBytes, 0);
                var now = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                byte[] messageContent = new byte[sizeof(long) * 2];


                var firstPart = BitConverter.GetBytes(senderTransmissionTime);
                var secondPart = BitConverter.GetBytes(now);

                Buffer.BlockCopy(firstPart, 0, messageContent, 0, firstPart.Length);
                Buffer.BlockCopy(secondPart, 0, messageContent, firstPart.Length, secondPart.Length);



                client.Send(messageContent, messageContent.Length, result.RemoteEndPoint);

                logger.LogInformation("Received message from {address} @ {port} and sent back the echo", result.RemoteEndPoint.Address, result.RemoteEndPoint.Port);
            }
        }
    }
}