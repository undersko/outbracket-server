using System;
using System.Collections.Generic;
using System.Text;

namespace Outbracket.Entities.Common
{
    public interface IBaseEntity
    {
        Guid Id { get; set; }
    }
}
