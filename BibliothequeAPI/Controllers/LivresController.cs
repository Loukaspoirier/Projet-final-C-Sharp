using Microsoft.AspNetCore.Mvc;
using BibliothequeAPI.Models;
using BibliothequeAPI.Repositories;

namespace BibliothequeAPI.Controllers
{
    [ApiController]
    [Route("livres")]
    public class LivresController : ControllerBase
    {
        private readonly IRepository _repository;

        public LivresController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Media>> GetAll()
        {
            return Ok(_repository.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<Media> GetById(int id)
        {
            var media = _repository.GetById(id);
            if (media == null)
                return NotFound();
            return Ok(media);
        }

        [HttpPost("ebook")]
        public ActionResult AddEbook([FromBody] Ebook ebook)
        {
            ebook.Type = "PDF";
            _repository.Add(ebook);
            return CreatedAtAction(nameof(GetById), new { id = ebook.Id }, ebook);
        }

        [HttpPost("paper")]
        public ActionResult AddPaperBook([FromBody] PaperBook paperBook)
        {
            paperBook.Type = "Papier";
            _repository.Add(paperBook);
            return CreatedAtAction(nameof(GetById), new { id = paperBook.Id }, paperBook);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Media updatedMedia)
        {
            var existing = _repository.GetById(id);
            if (existing == null)
                return NotFound();

            _repository.Update(id, updatedMedia);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var existing = _repository.GetById(id);
            if (existing == null)
                return NotFound();

            _repository.Delete(id);
            return NoContent();
        }

        [HttpGet("ebook")]
        public ActionResult<IEnumerable<Ebook>> GetEbooks()
        {
            var ebooks = _repository.GetAll().OfType<Ebook>().ToList();
            return Ok(ebooks);
        }


        [HttpGet("paperbook")]
        public ActionResult<IEnumerable<PaperBook>> GetPaperBooks()
        {
            var papers = _repository.GetAll().OfType<PaperBook>().ToList();
            return Ok(papers);
        }
    }
}
