using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Microsoft.Extensions.DependencyInjection;
using Vegas.AWS.SimpleQueueService.Service;

namespace Vegas.AWS.SimpleQueueService.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAWSSimpleQueue(this IServiceCollection services)
        {
            services.AddSingleton<IAmazonSQS, AmazonSQSClient>();
            services.AddInternals();
            return services;
        }

        public static IServiceCollection AddAWSSimpleQueue(this IServiceCollection services,
            AWSCredentials credentials, RegionEndpoint region)
        {
            services.AddSingleton<IAmazonSQS, AmazonSQSClient>(sp => new AmazonSQSClient(credentials, region));
            services.AddInternals();
            return services;
        }

        private static void AddInternals(this IServiceCollection services)
        {
            services.AddSingleton<IProducerService, ProducerService>();
            services.AddSingleton<IConsumerService, ConsumerService>();
            services.AddSingleton<IQueueManager, QueueManager>();
        }
    }
}
