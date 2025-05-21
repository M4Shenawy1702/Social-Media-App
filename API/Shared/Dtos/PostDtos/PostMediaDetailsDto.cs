using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Shared.Dtos.PostDtos
{
    public class PostMediaDetailsDto
    {
        public string Url { get; set; } = null!;
        public string Type { get; set; } = null!; 
    }
}