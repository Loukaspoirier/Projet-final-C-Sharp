using BibliothequeAPI.Models;

namespace BibliothequeAPI.Repositories
{
    public interface IRepository
    {
        IEnumerable<Media> GetAll();
        Media? GetById(int id);
        void Add(Media media);
        void Update(int id, Media updatedMedia);
        void Delete(int id);
    }
}
