using System.Text.Json;
using BibliothequeAPI.Models;

namespace BibliothequeAPI.Repositories
{
    public class Repository : IRepository
    {
        private readonly string _filePath = "data.json";
        private List<Media> _medias;
        private int _nextId = 1;

        public Repository()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _medias = JsonSerializer.Deserialize<List<Media>>(json) ?? new List<Media>();
                _nextId = _medias.Count > 0 ? _medias.Max(m => m.Id) + 1 : 1;
            }
            else
            {
                _medias = new List<Media>();
            }
        }

        private void SaveToFile()
        {
            var json = JsonSerializer.Serialize(_medias);
            File.WriteAllText(_filePath, json);
        }

        public IEnumerable<Media> GetAll()
        {
            return _medias;
        }

        public Media? GetById(int id)
        {
            return _medias.FirstOrDefault(m => m.Id == id);
        }

        public void Add(Media media)
        {
            media.Id = _nextId++;
            _medias.Add(media);
            SaveToFile();
        }

        public void Update(int id, Media updatedMedia)
        {
            var index = _medias.FindIndex(m => m.Id == id);
            if (index != -1)
            {
                updatedMedia.Id = id;
                _medias[index] = updatedMedia;
                SaveToFile();
            }
        }

        public void Delete(int id)
        {
            var media = _medias.FirstOrDefault(m => m.Id == id);
            if (media != null)
            {
                _medias.Remove(media);
                SaveToFile();
            }
        }
    }
}