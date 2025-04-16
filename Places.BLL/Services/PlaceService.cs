using System.Collections.Generic;
using System.Linq;
using Places.BLL.DTO;
using Places.BLL.Interfaces;
using Places.BLL.Mappers;
using Places.Abstract.UnitOfWork;
using Places.Models;

namespace Places.BLL.Services
{
    public class PlaceService : IPlaceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PlaceMapper _mapper;

        public PlaceService(IUnitOfWork unitOfWork, PlaceMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public PlaceDTO GetPlaceById(int id)
        {
            var place = _unitOfWork.GetRepository<Place>().GetById(id);
            return _mapper.ToDto(place);
        }

        public IEnumerable<PlaceDTO> GetAllPlaces()
        {
            var places = _unitOfWork.GetRepository<Place>().GetAll();
            return places.Select(p => _mapper.ToDto(p));
        }

        public void AddPlace(PlaceDTO placeDto)
        {
            if (placeDto == null || string.IsNullOrEmpty(placeDto.Name))
                throw new ArgumentException("Place data is invalid.");
            
            var place = _mapper.ToEntity(placeDto);
            _unitOfWork.GetRepository<Place>().Add(place);
            _unitOfWork.SaveChanges();
        }

        public void UpdatePlace(PlaceDTO placeDto)
        {
            if (placeDto == null || string.IsNullOrEmpty(placeDto.Name))
                throw new ArgumentException("Place data is invalid.");
            
            var place = _mapper.ToEntity(placeDto);
            _unitOfWork.GetRepository<Place>().Update(place);
            _unitOfWork.SaveChanges();
        }

        public void DeletePlace(int id)
        {
            var place = _unitOfWork.GetRepository<Place>().GetById(id);
            if (place != null)
            {
                _unitOfWork.GetRepository<Place>().Delete(place);
                _unitOfWork.SaveChanges();
            }
        }
    }
}