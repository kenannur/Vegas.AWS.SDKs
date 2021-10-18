using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Vegas.AWS.Common.Extensions;

namespace Vegas.AWS.SimpleQueueService.Service
{
    public interface IProducerService
    {
        Task SendMessageAsync(string url, string body, string groupId = default);
    }

    public class ProducerService : IProducerService
    {
        private readonly IAmazonSQS _client;
        public ProducerService(IAmazonSQS client) => _client = client;

        public async Task SendMessageAsync(string url, string body, string groupId = null)
        {
            var response = await _client.SendMessageAsync(new SendMessageRequest
            {
                QueueUrl = url,
                MessageBody = body,
                MessageGroupId = groupId
            });
            response.EnsureSuccessStatusCode();
        }
    }
}
