using Outbracket.Api.Contracts.Requests.Common;
using Outbracket.Entities.Common;
using Outbracket.Services.Contracts.Models.Common;

namespace Outbracket.Common.Extensions
{
    public static class DictionaryExtensions
    {
        public static DictionaryItem ToDictionaryItem(this IBaseDictionary source)
        {
            return source == null ? null : new DictionaryItem { Id = source.Id, Name = source.Name };
        }
        
        public static DictionaryItem ToDictionaryItem(this DictionaryItemApiRequest source)
        {
            return source == null ? null : new DictionaryItem { Id = source.Id, Name = source.Name };
        }
        
        public static DictionaryItemApiResponse ToDictionaryItemApiResponse(this DictionaryItem source)
        {
            return source == null ? null : new DictionaryItemApiResponse { Id = source.Id, Name = source.Name };
        }
    }
}