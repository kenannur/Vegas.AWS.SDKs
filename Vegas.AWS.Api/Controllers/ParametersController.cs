using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vegas.AWS.SystemsManager.Service;

namespace Vegas.AWS.Api.Controllers
{
    [Route("aws/[controller]")]
    public class ParametersController : ControllerBase
    {
        private readonly IParameterStoreService _parameterService;
        public ParametersController(IParameterStoreService parameterService) => _parameterService = parameterService;

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] string path)
        {
            var parameters = await _parameterService.GetParametersAsync(path);
            return Ok(parameters.Select(x => new
            {
                x.Name,
                x.Value
            }).ToList());
        }

        [HttpPost("Import")]
        public async Task<IActionResult> ImportAsync([FromQuery] string prefix)
        {
            var prefixTR = Path.Combine(prefix, "tr");
            var jsonTR = await System.IO.File.ReadAllTextAsync("Resources/Localizable.tr.json");
            await _parameterService.ImportParametersAsync(jsonTR, prefixTR);

            var prefixEN = Path.Combine(prefix, "en");
            var jsonEN = await System.IO.File.ReadAllTextAsync("Resources/Localizable.en.json");
            await _parameterService.ImportParametersAsync(jsonEN, prefixEN);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromQuery] string name)
        {
            await _parameterService.DeleteParameterAsync(name);
            return Ok();
        }
    }
}
