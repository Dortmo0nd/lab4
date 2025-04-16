using System.Collections.Generic;
using Places.BLL.DTO;

namespace Places.BLL.Interfaces
{
    public interface IMediaService
    {
        MediaDTO GetMediaById(int id);
        IEnumerable<MediaDTO> GetAllMedia();
        void AddMedia(MediaDTO media);
        void UpdateMedia(MediaDTO media);
        void DeleteMedia(int id);
    }
}