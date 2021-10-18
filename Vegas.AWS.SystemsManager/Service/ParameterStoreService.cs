using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Vegas.AWS.Common.Extensions;

namespace Vegas.AWS.SystemsManager.Service
{
    public class ParameterStoreService : IParameterStoreService
    {
        private readonly IAmazonSimpleSystemsManagement _client;

        public ParameterStoreService(IAmazonSimpleSystemsManagement client)
        {
            _client = client;
        }

        public async Task CreateParameterAsync(string name, string value)
        {
            var response = await _client.PutParameterAsync(new PutParameterRequest
            {
                DataType = "text",
                Tier = ParameterTier.Standard,
                Type = ParameterType.String,
                Name = name,
                Value = value
            });
            response.EnsureSuccessStatusCode();
        }

        public async Task<Parameter> GetParameterAsync(string name)
        {
            var response = await _client.GetParameterAsync(new GetParameterRequest
            {
                Name = name
            });
            response.EnsureSuccessStatusCode();
            return response.Parameter;
        }

        public async Task<List<Parameter>> GetParametersAsync(string path)
        {
            var response = await _client.GetParametersByPathAsync(new GetParametersByPathRequest
            {
                Path = path
            });
            response.EnsureSuccessStatusCode();
            return response.Parameters;
        }

        public async Task DeleteParameterAsync(string name)
        {
            var response = await _client.DeleteParameterAsync(new DeleteParameterRequest
            {
                Name = name
            });
            response.EnsureSuccessStatusCode();
        }

        public async Task ImportParametersAsync(string json, string prefix)
        {
            var dictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            foreach (var keyValuePair in dictionary)
            {
                var key = Path.Combine(prefix, keyValuePair.Key);
                await CreateParameterAsync(key, keyValuePair.Value);
            }
        }

        public async Task ImportParametersAsync(Stream jsonStream, string prefix)
        {
            var dictionary = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(jsonStream);
            foreach (var keyValuePair in dictionary)
            {
                var key = Path.Combine(prefix, keyValuePair.Key);
                await CreateParameterAsync(key, keyValuePair.Value);
            }
        }

        public async Task<string> ExportParametersAsync(string path)
        {
            var parameters = await GetParametersAsync(path);
            var dictionary = new Dictionary<string, string>();
            foreach (var parameter in parameters)
            {
                dictionary.Add(parameter.Name, parameter.Value);
            }
            var json = JsonSerializer.Serialize(dictionary);
            return json;
        }

    }
}
