using GloboTicket.Integration.Messages;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace GloboTicket.Integration.MessagingBus
{
    public class AzServiceBusMessageBus: IMessageBus
    {
        //TODO: read from settings
        private string connectionString = "Endpoint=sb://edgglobalticket-sample.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=7WI1L3It6g85ZDNZKKC0xgtIO0DnTXgstlHktwTCdCQ=";

        public async Task PublishMessage(IntegrationBaseMessage message, string topicName)
        {
            ISenderClient topicClient = new TopicClient(connectionString, topicName);

            var jsonMessage = JsonConvert.SerializeObject(message);
            var serviceBusMessage = new Message(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString()
            };

            await topicClient.SendAsync(serviceBusMessage);
            Console.WriteLine($"Sent message to {topicClient.Path}");
            await topicClient.CloseAsync();

        }
    }
}
