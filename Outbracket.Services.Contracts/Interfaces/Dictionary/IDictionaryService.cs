using System.Collections.Generic;
using System.Threading.Tasks;
using Outbracket.Services.Contracts.Models.Common;

namespace Outbracket.Services.Contracts.Interfaces.Dictionary
{
    public interface IDictionaryService
    {
        Task<IEnumerable<DictionaryItem>> GetCountries();
    }
}