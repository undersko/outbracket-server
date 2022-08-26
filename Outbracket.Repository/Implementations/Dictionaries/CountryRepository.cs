using Outbracket.Entities.Account;
using Outbracket.Entities.Dictionaries;
using Outbracket.Repositories.Contracts;
using Outbracket.Repositories.Contracts.Interfaces.Account;
using Outbracket.Repositories.Contracts.Interfaces.Dictionaries;
using Outbracket.Repositories.Implementations.Common;

namespace Outbracket.Repositories.Implementations.Dictionaries
{
    public class CountryRepository : DictionaryBaseRepository<Country>, ICountryRepository
    {
        public CountryRepository(Context context) : base(context) { }
    }
}