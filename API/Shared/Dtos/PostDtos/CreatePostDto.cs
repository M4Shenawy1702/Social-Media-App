using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Shared.Dtos.PostDtos
{
    public class CreatePostDto
    {
        public required  string Content { get; set; } = null!;
        public required string AuthorId { get; set; } = null!;
        public required ICollection<PostMediaDto> Media { get; set; } = [];
    }
}