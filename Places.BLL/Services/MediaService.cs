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
    public class MediaService : IMediaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly MediaMapper _mapper;

        public MediaService(IUnitOfWork unitOfWork, MediaMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public MediaDTO GetMediaById(int id)
        {
            var media = _unitOfWork.MediaRepository.GetById(id);
            return _mapper.ToDto(media);
        }

        public IEnumerable<MediaDTO> GetAllMedia()
        {
            var media = _unitOfWork.MediaRepository.GetAll();
            return media.Select(m => _mapper.ToDto(m));
        }

        public void AddMedia(MediaDTO mediaDto)
        {
            if (mediaDto == null || string.IsNullOrEmpty(mediaDto.FilePath) || string.IsNullOrEmpty(mediaDto.Type))
                throw new ArgumentException("Invalid media data");

            var media = _mapper.ToEntity(mediaDto);
            _unitOfWork.MediaRepository.Add(media);
            _unitOfWork.SaveChanges();
        }

        public void UpdateMedia(MediaDTO mediaDto)
        {
            if (mediaDto == null || string.IsNullOrEmpty(mediaDto.FilePath) || string.IsNullOrEmpty(mediaDto.Type))
                throw new ArgumentException("Invalid media data");

            var media = _mapper.ToEntity(mediaDto);
            _unitOfWork.MediaRepository.Update(media);
            _unitOfWork.SaveChanges();
        }

        public void DeleteMedia(int id)
        {
            var media = _unitOfWork.MediaRepository.GetById(id);
            if (media != null)
            {
                _unitOfWork.MediaRepository.Delete(media);
                _unitOfWork.SaveChanges();
            }
        }
    }
}