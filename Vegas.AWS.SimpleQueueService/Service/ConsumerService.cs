using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Vegas.AWS.Common.Extensions;

namespace Vegas.AWS.SimpleQueueService.Service
{
    public interface IConsumerService
    {
        Task<Message> ReceiveMessageAsync(string url);
    }

    public class ConsumerService : IConsumerService
    {
        private readonly IAmazonSQS _client;
        public ConsumerService(IAmazonSQS client) => _client = client;

        public async Task<Message> ReceiveMessageAsync(string url)
        {
            var receiveResponse = await _client.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = url,
                AttributeNames = new List<string> { "MessageGroupId" },
                WaitTimeSeconds = 20, // 0 = ShortPolling | OtherValues = LongPolling
                MaxNumberOfMessages = 1 // 1-10
            });
            receiveResponse.EnsureSuccessStatusCode();

            var message = receiveResponse.Messages.FirstOrDefault();
            if (message == null)
            {
                return null;
            }

            var deleteResponse = await _client.DeleteMessageAsync(url, message.ReceiptHandle);
            deleteResponse.EnsureSuccessStatusCode();

            return message;
        }
    }
}
