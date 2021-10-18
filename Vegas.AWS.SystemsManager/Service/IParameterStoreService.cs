using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon.SimpleSystemsManagement.Model;

namespace Vegas.AWS.SystemsManager.Service
{
    public interface IParameterStoreService
    {
        Task CreateParameterAsync(string name, string value);

        Task<Parameter> GetParameterAsync(string name);

        Task<List<Parameter>> GetParametersAsync(string path);

        Task DeleteParameterAsync(string name);

        Task ImportParametersAsync(string json, string prefix);

        Task ImportParametersAsync(Stream jsonStream, string prefix);

        Task<string> ExportParametersAsync(string path);
    }
}
