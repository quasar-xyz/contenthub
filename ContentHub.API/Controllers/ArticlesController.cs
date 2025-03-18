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

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Article article)
        {
            if (id != article.ArticleId)
            {
                return BadRequest("Article ID mismatch");
            }

            var existingArticle = await _context.Articles.FindAsync(id);
            if (existingArticle == null)
            {
                return NotFound("Article not found");
            }

       
            existingArticle.Title = article.Title;
            existingArticle.Content = article.Content;
            existingArticle.CategoryId = article.CategoryId;
            existingArticle.PublishDate = article.PublishDate;

          
            if (!string.IsNullOrEmpty(article.PhotoPath) && article.PhotoPath != existingArticle.PhotoPath)
            {
                existingArticle.PhotoPath = article.PhotoPath;
            }

            _context.Articles.Update(existingArticle);
            await _context.SaveChangesAsync();

            return Ok(existingArticle); 
        }

        
        
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var articles = await _context.Articles
                .Include(a => a.Category)
                .ToListAsync();
            return Ok(articles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var article = await _context.Articles
                .Include(a => a.Category)
                .FirstOrDefaultAsync(a => a.ArticleId == id);

            return article == null ? NotFound() : Ok(article);
        }

        [HttpPost("like")]
        public async Task<IActionResult> CreateLike([FromBody] Like like)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.UserId == like.UserId && l.ArticleId == like.ArticleId);

            if (existingLike != null)
            {
                return BadRequest("User has already liked this article.");
            }

            _context.Likes.Add(like);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetLikeById), new { id = like.LikeId }, like);
        }

        [HttpGet("like/{id}")]
        public async Task<IActionResult> GetLikeById(int id)
        {
            var like = await _context.Likes.FindAsync(id);
            return like == null ? NotFound() : Ok(like);
        }

        [HttpPost("comment")]
        public async Task<IActionResult> CreateComment([FromBody] Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCommentById), new { id = comment.CommentId }, comment);
        }

        [HttpGet("comment/{id}")]
        public async Task<IActionResult> GetCommentById(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            return comment == null ? NotFound() : Ok(comment);
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
    }
}
