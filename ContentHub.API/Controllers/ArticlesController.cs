using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContentHub.Frontend.Data;
using ContentHub.Frontend.Tables;

namespace ContentHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ArticlesController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        // ✅ API Test Endpoint
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("API is working");
        }

        // ✅ Get all articles
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var articles = await _context.Articles.Include(a => a.Category).ToListAsync();
            return Ok(articles);
        }

        // ✅ Get article by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var article = await _context.Articles.Include(a => a.Category).FirstOrDefaultAsync(a => a.ArticleId == id);
            if (article == null)
            {
                return NotFound();
            }
            return Ok(article);
        }

        // ✅ Create new article
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Article article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            article.PublishDate = DateTime.UtcNow;
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = article.ArticleId }, article);
        }

        // ✅ Edit article
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Article updatedArticle)
        {
            if (id != updatedArticle.ArticleId)
            {
                return BadRequest("ID mismatch");
            }

            var existingArticle = await _context.Articles.FindAsync(id);
            if (existingArticle == null)
            {
                return NotFound();
            }

            existingArticle.Title = updatedArticle.Title;
            existingArticle.Content = updatedArticle.Content;
            existingArticle.CategoryId = updatedArticle.CategoryId;
            existingArticle.PublishDate = DateTime.UtcNow; // Keeping it updated when modified

            _context.Articles.Update(existingArticle);
            await _context.SaveChangesAsync();

            return NoContent(); // HTTP 204 - No Content (successful update)
        }

        // ✅ Delete article
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
