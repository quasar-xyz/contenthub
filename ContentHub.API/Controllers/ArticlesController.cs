using System.Security.Claims;
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
        
 
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("API is working");
        }

    
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var articles = await _context.Articles.Include(a => a.Category).ToListAsync();
            return Ok(articles);
        }

       
        [HttpPost("{articleId}/like")]
        public async Task<IActionResult> LikeArticle(int articleId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.ArticleId == articleId && l.UserId == userId);

            if (existingLike != null)
            {
                return Conflict("User already liked this article.");
            }

            var like = new Like
            {
                UserId = userId,
                ArticleId = articleId
            };

            _context.Likes.Add(like);
            await _context.SaveChangesAsync();

            return Ok();
        }
       
        
        
        [HttpGet("{articleId}/likes")]
        public async Task<IActionResult> GetLikeCount(int articleId)
        {
            var likeCount = await _context.Likes.CountAsync(l => l.ArticleId == articleId);
            return Ok(likeCount);
        }
        
        
        
        
      
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
        
        
        

       
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Article updatedArticle)
        {
            
            return NoContent();
        }

       
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
