namespace API.Shared.Dtos.PostDtos
{
    public class PostMediaDto
    {
        public IFormFile Media { get; set; } = null!;
        public string Type { get; set; } = null!; 
    }
}
