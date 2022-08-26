using System;
using Outbracket.Services.Contracts.Models.Common;

namespace Outbracket.Api.Contracts.Responses.Profile
{
    public class GetUserInfoApiResponse
    {
        public Guid? Id { get; set; }
        
        public string Username { get; set; }
        
        public string Email { get; set; }

        public string Bio { get; set; }

        public Guid? ImageId { get; set; }
        
        public string? UserSettingsId { get; set; }

        public DictionaryItem Country { get; set; }
    }
}