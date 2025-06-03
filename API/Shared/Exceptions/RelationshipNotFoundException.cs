using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{ 
    public class RelationshipNotFoundException ()
    : Exception($"Friend request was not Found!")
    {
        
    }
}