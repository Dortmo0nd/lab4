using System.Collections;
using Places.BLL.DTO;

namespace Places.BLL.Interfaces
{
    public interface IPlaceService
    {
        PlaceDTO GetPlaceById(int id);
        IEnumerable<PlaceDTO> GetAllPlaces();
        void AddPlace(PlaceDTO place);
        void UpdatePlace(PlaceDTO place);
        void DeletePlace(int id);
        PlaceDTO GetPlaceByName(string name);
    }
}