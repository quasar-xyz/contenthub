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

        [HttpPost("bookmark")]
        public async Task<IActionResult> CreateBookmark([FromBody] Bookmark bookmark)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingBookmark = await _context.Bookmarks
                .FirstOrDefaultAsync(b => b.UserId == bookmark.UserId && b.ArticleId == bookmark.ArticleId);
            
            bookmark.BookmarkDate = DateTime.UtcNow;

            _context.Bookmarks.Add(bookmark);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookmarkById), new { id = bookmark.BookmarkId }, bookmark);
        }
        
        [HttpGet("bookmarks/{userId}")]
        public async Task<IActionResult> GetBookmarkedArticles(string userId)
        {
            var bookmarkedArticles = await _context.Articles
                .Where(a => a.Bookmarks.Any(b => b.UserId == userId))  // Only get bookmarked articles
                .Select(a => new
                {
                    a.ArticleId,
                    a.Title,
                    a.PhotoPath,
                    a.PublishDate
                })
                .ToListAsync();

            return Ok(bookmarkedArticles);
        }


        [HttpGet("bookmark/{id}")]
        public async Task<IActionResult> GetBookmarkById(int id)
        {
            var bookmark = await _context.Bookmarks.FindAsync(id);
            return bookmark == null ? NotFound() : Ok(bookmark);
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
        
        [HttpGet("articles")]
        public async Task<IActionResult> GetArticlesByIds([FromQuery] List<int> ids)
        {
            
            var articles = await _context.Articles
                .Where(a => ids.Contains(a.ArticleId))
                .ToListAsync();

            return Ok(articles);
        }

       
        [HttpGet("category/{categoryName}")]
        public async Task<IActionResult> GetArticlesByCategory(string categoryName)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryName.ToLower() == categoryName.ToLower());

            var articles = await _context.Articles
                .Where(a => a.CategoryId == category.CategoryId)
                .Include(a => a.Category)
                .ToListAsync();

            return Ok(articles);
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
