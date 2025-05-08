using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;

namespace Sender
{
    internal class ReceiverService(ILogger<ReceiverService> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var client = new UdpClient(6000);


            logger.LogInformation("Starting receiver on {port}", client.Client.LocalEndPoint);

            while (!stoppingToken.IsCancellationRequested)
            {
                
                var result = await client.ReceiveAsync(stoppingToken);
                var receivedBytes = result.Buffer;

                var senderTransmissionTime = BitConverter.ToInt64(receivedBytes, 0);
                var receiverTransmissionTime = BitConverter.ToInt64(receivedBytes, sizeof(long));

                var now = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                var roundTripTime = now - senderTransmissionTime;

                logger.LogInformation("Received data from {address} @ {port} with RTT: {rtt} ms", result.RemoteEndPoint.Address, result.RemoteEndPoint.Port, roundTripTime);

            }
        }
    }
}
