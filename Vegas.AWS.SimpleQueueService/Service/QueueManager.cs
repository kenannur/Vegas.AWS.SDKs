using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SQS;
using Vegas.AWS.Common.Extensions;

namespace Vegas.AWS.SimpleQueueService.Service
{
    public interface IQueueManager
    {
        Task<string> CreateQueueAsync(string name);

        Task DeleteQueueAsync(string name);

        Task<List<string>> ListQueuesAsync(string queueNamePrefix);

        Task PurgeQueueAsync(string url);
    }

    public class QueueManager : IQueueManager
    {
        private readonly IAmazonSQS _client;
        public QueueManager(IAmazonSQS client) => _client = client;

        public async Task<string> CreateQueueAsync(string name)
        {
            var response = await _client.CreateQueueAsync(name);
            response.EnsureSuccessStatusCode();
            return response.QueueUrl;
        }

        public async Task DeleteQueueAsync(string url)
        {
            var response = await _client.DeleteQueueAsync(url);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<string>> ListQueuesAsync(string queueNamePrefix)
        {
            var response = await _client.ListQueuesAsync(queueNamePrefix);
            response.EnsureSuccessStatusCode();
            return response.QueueUrls;
        }

        public async Task PurgeQueueAsync(string url)
        {
            var response = await _client.PurgeQueueAsync(url);
            response.EnsureSuccessStatusCode();
        }
    }
}
