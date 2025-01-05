using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

class Program
{
    // Replace with your Service Bus connection string and queue name
    private const string connectionString = "Endpoint=sb://orderprocessingnamespace.servicebus.windows.net/;SharedAccessKeyName=akall;SharedAccessKey=9bAZE59E61l/ymW48I7mVeW/EYgTzRsSS+ASbKCX8/U=;EntityPath=orderqueue";
    private const string queueName = "orderqueue";

    static async Task Main(string[] args)
    {
        Console.WriteLine("Receiving a single message from Azure Service Bus Queue...");
        await ReceiveSingleMessageAsync();
    }

    static async Task ReceiveSingleMessageAsync()
    {
        // Create a client to connect to the Service Bus
        var client = new ServiceBusClient(connectionString);

        // Create a receiver for the queue
        var receiver = client.CreateReceiver(queueName);

        try
        {
            // Try to receive a single message with a 10-second timeout
            ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync(TimeSpan.FromSeconds(10));

            if (receivedMessage != null)
            {
                // Display the message body
                string messageBody = receivedMessage.Body.ToString();
                Console.WriteLine($"Message received: {messageBody}");

                // Complete the message so it is removed from the queue
                await receiver.CompleteMessageAsync(receivedMessage);
                Console.WriteLine("Message processed successfully.");
            }
            else
            {
                Console.WriteLine("No message available to receive.");
            }
        }
        finally
        {
            // Dispose of the client and receiver
            await receiver.DisposeAsync();
            await client.DisposeAsync();
        }
    }
}
