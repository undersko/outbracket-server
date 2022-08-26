using System;
using Outbracket.Api.Contracts.Requests.Common;

namespace Outbracket.Api.Contracts.Requests.Account
{
    public class UpdateUserInfoApiRequest
    {
        public Guid? Id { get; set; }
        
        public string Bio { get; set; }

        public Guid? ImageId { get; set; }

        public DictionaryItemApiRequest Country { get; set; }
    }
}