using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Outbracket.Api.Contracts.Responses;
using Outbracket.Common.Extensions;
using Outbracket.Services.Contracts.Interfaces.Dictionary;

namespace Outbracket.Controllers.Web
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class DictionaryController : ApiControllerBase
    {
        private readonly IDictionaryService _dictionaryService;
        
        public DictionaryController(IDictionaryService dictionaryService)
        {
            _dictionaryService = dictionaryService;
        }

        [HttpGet("countries")]
        public async Task<Response> GetCountries()
        {
            var countries = await _dictionaryService.GetCountries();
            return Success(countries.Select(x => x.ToDictionaryItemApiResponse()));
        }
    }
}