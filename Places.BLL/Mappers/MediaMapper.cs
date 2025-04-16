using Places.BLL.DTO;
using Places.Models;

namespace Places.BLL.Mappers
{
    public class MediaMapper
    {
        public MediaDTO ToDto(Media media)
        {
            if (media == null) return null;
            return new MediaDTO
            {
                Id = media.Id,
                FilePath = media.FilePath,
                Type = media.Type,
                PlaceId = media.PlaceId,
                UserId = media.UserId
            };
        }

        public Media ToEntity(MediaDTO mediaDto)
        {
            if (mediaDto == null) return null;
            return new Media
            {
                Id = mediaDto.Id,
                FilePath = mediaDto.FilePath,
                Type = mediaDto.Type,
                PlaceId = mediaDto.PlaceId,
                UserId = mediaDto.UserId
            };
        }
    }
}