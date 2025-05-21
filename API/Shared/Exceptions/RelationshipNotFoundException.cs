using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{ 
    public class RelationshipNotFoundException (int relationshipId)
    : Exception($"Friend request with id:{relationshipId} was Found!")
    {
        
    }
}