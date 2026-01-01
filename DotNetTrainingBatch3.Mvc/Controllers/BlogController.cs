using DotNetTrainingBatch3.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetTrainingBatch3.Mvc.Controllers
{
    public class BlogController : Controller
    {
        private readonly DotNetTrainingBatch3DbContext _db;

        public BlogController(DotNetTrainingBatch3DbContext db)
        {
            _db = db;
        }

        // GET: BlogController
        public async Task<IActionResult> Index()
        {
            var blogs = await _db.TblBlogs
                .AsNoTracking()
                .OrderByDescending(x => x.BlogId)
                .ToListAsync();

            return View(blogs);
        }

        // Shows the create form
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        
        // Saves the new blog data
        [HttpPost]
        public async Task<IActionResult> Save(TblBlog blog)
        {
            await _db.TblBlogs.AddAsync(blog);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        
        // Shows the blog to be edited
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var blog = await _db.TblBlogs.FirstOrDefaultAsync(x => x.BlogId == id);
            if (blog is null)
            {
                return RedirectToAction("Index");
            }

            return View(blog);
        }

        // Updates the blog data
        [HttpPost]
        public async Task<IActionResult> Update(int id, TblBlog blog)
        {
            var item = await _db.TblBlogs.FirstOrDefaultAsync(x => x.BlogId == id);
            if (item is null)
            {
                return RedirectToAction("Index");
            }
            
            item.BlogTitle = blog.BlogTitle;
            item.BlogAuthor = blog.BlogAuthor;
            item.BlogContent = blog.BlogContent;
            
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.TblBlogs.FirstOrDefaultAsync(x => x.BlogId == id);
            if (item is null)
            {
                return RedirectToAction("Index");
            }
            
            _db.TblBlogs.Remove(item);
            await _db.SaveChangesAsync();
            
            return RedirectToAction("Index");
        }
    }
}
