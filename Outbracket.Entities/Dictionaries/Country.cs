using Outbracket.Entities.Common;

namespace Outbracket.Entities.Dictionaries
{
    public class Country : IBaseDictionary
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
    }
}