using Amazon;
using Amazon.Runtime;
using Amazon.SimpleSystemsManagement;
using Microsoft.Extensions.DependencyInjection;
using Vegas.AWS.SystemsManager.Service;

namespace Vegas.AWS.SystemsManager.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAWSParameterStore(this IServiceCollection services)
        {
            services.AddSingleton<IAmazonSimpleSystemsManagement, AmazonSimpleSystemsManagementClient>();
            services.AddScoped<IParameterStoreService, ParameterStoreService>();
            return services;
        }

        public static IServiceCollection AddAWSParameterStore(this IServiceCollection services,
            AWSCredentials credentials, RegionEndpoint region)
        {
            services.AddSingleton<IAmazonSimpleSystemsManagement, AmazonSimpleSystemsManagementClient>(sp =>
            {
                return new AmazonSimpleSystemsManagementClient(credentials, region);
            });
            services.AddScoped<IParameterStoreService, ParameterStoreService>();
            return services;
        }
    }
}
