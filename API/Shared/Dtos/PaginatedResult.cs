using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Shared.Dtos
{
public record PaginatedResult<TData>(int PageIndex, int PageCount, int Count , IEnumerable<TData> Data);   
}