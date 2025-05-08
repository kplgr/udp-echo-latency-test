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
            int listenPort = 6000;
            int replyPort = 6000;

            var client = new UdpClient();

            logger.LogInformation("Starting echo server, listening on port {port}", listenPort);

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



                var responseEndpoint = result.RemoteEndPoint;
                responseEndpoint.Port = replyPort;

                client.Send(messageContent, messageContent.Length, responseEndpoint);

                logger.LogInformation("Received message from {receiveEndpoint} and sent back the echo to {responseEndpoint}", result.RemoteEndPoint, responseEndpoint);
            }
        }
    }
}