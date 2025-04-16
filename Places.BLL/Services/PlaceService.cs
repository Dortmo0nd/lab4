using System;
using System.Collections.Generic;
using System.Linq;
using Places.BLL.DTO;
using Places.BLL.Interfaces;
using Places.BLL.Mappers;
using Places.Abstract;
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
            var place = _unitOfWork.PlaceRepository.GetById(id);
            return _mapper.ToDto(place);
        }

        public IEnumerable<PlaceDTO> GetAllPlaces()
        {
            var places = _unitOfWork.PlaceRepository.GetAll();
            return places.Select(p => _mapper.ToDto(p));
        }

        public void AddPlace(PlaceDTO placeDto)
        {
            if (placeDto == null || string.IsNullOrEmpty(placeDto.Name))
                throw new ArgumentException("Invalid place data");

            var place = _mapper.ToEntity(placeDto);
            _unitOfWork.PlaceRepository.Add(place);
            _unitOfWork.SaveChanges();
        }

        public void UpdatePlace(PlaceDTO placeDto)
        {
            if (placeDto == null || string.IsNullOrEmpty(placeDto.Name))
                throw new ArgumentException("Invalid place data");

            var place = _mapper.ToEntity(placeDto);
            _unitOfWork.PlaceRepository.Update(place);
            _unitOfWork.SaveChanges();
        }

        public void DeletePlace(int id)
        {
            var place = _unitOfWork.PlaceRepository.GetById(id);
            if (place != null)
            {
                _unitOfWork.PlaceRepository.Delete(place);
                _unitOfWork.SaveChanges();
            }
        }
    }
}