namespace Places.BLL.DTO
{
    public class MediaDTO
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public string Type { get; set; }
        public int? PlaceId { get; set; }
        public int? UserId { get; set; }
    }
}