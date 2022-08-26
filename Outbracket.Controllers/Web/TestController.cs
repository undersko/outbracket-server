using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Outbracket.Api.Contracts.Responses;

namespace Outbracket.Controllers.Web
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ApiControllerBase
    {
        [HttpGet]
        public async Task<Response> GetScaledImage(string containerName, string fileName)
        {
            return Success("all is g00d");
        }
    }
}