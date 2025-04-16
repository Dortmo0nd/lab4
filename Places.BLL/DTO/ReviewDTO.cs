namespace Places.BLL.DTO
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int PlaceId { get; set; }
        public int UserId { get; set; }
    }
}