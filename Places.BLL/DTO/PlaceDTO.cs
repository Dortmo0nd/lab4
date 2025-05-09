using System.ComponentModel.DataAnnotations;

namespace Places.BLL.DTO
{
    public class PlaceDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<ReviewDTO> Reviews { get; set; } = new List<ReviewDTO>();
        public List<QuestionDTO> Questions { get; set; } = new List<QuestionDTO>();
        public List<MediaDTO> MediaFiles { get; set; } = new List<MediaDTO>();
    }
}