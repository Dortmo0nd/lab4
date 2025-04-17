using Places.Models;
using Places.BLL.DTO;

namespace Places.BLL.Mappers
{
    public class PlaceMapper
    {
        public PlaceDTO ToDto(Place place)
        {
            if (place == null) return null;
            return new PlaceDTO
            {
                Id = place.Id,
                Name = place.Name,
                Description = place.Description,
                Latitude = place.Latitude,
                Longitude = place.Longitude
            };
        }

        public Place ToEntity(PlaceDTO placeDto)
        {
            if (placeDto == null) return null;
            return new Place
            {
                Id = placeDto.Id,
                Name = placeDto.Name,
                Description = placeDto.Description,
                Latitude = placeDto.Latitude,
                Longitude = placeDto.Longitude
            };
        }
    }
}