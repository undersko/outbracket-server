using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Outbracket.Repositories.Contracts.Interfaces.Dictionaries;
using Outbracket.Services.Contracts.Interfaces.Dictionary;
using Outbracket.Services.Contracts.Models.Common;
using Outbracket.Common.Extensions;

namespace Outbracket.Services.Implementations.Dictionary
{
    public class DictionaryService : IDictionaryService
    {
        private readonly ICountryRepository _countryRepository;
        
        public DictionaryService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }
        
        public async Task<IEnumerable<DictionaryItem>> GetCountries()
        {
            var countries = await _countryRepository.GetAllAsync().ToArrayAsync();
            return countries.Select(x => x.ToDictionaryItem());
        }
    }
}