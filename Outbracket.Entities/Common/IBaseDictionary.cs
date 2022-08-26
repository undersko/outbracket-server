using System;
using System.Collections.Generic;
using System.Text;

namespace Outbracket.Entities.Common
{
    public interface IBaseDictionary
    {
        int Id { get; set; }
        string Name { get; set; }
    }
}
