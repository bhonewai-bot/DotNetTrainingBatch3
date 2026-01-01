using DotNetTrainingBatch3.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetTrainingBatch3.Mvc.Controllers
{
    public class BlogAjaxController : Controller
    {
        private readonly DotNetTrainingBatch3DbContext _db;

        public BlogAjaxController(DotNetTrainingBatch3DbContext db)
        {
            _db = db;
        }

        // Returns the main page view
        public IActionResult Index()
        {
            return View();
        }

        // Returns blog data as JSON for AJAX call
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var blogs = await _db.TblBlogs
                .AsNoTracking()
                .OrderByDescending(x => x.BlogId)
                .ToListAsync();
            
            return Json(blogs);
        }

        // Returns the create page view
        public IActionResult Create()
        {
            return View();
        }

        // Saves new blog data received from AJAX
        [HttpPost]
        public async Task<IActionResult> Save(TblBlog blog)
        {
            await _db.TblBlogs.AddAsync(blog);
            var result = await _db.SaveChangesAsync();

            var response = new
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Saving Successful." : "Saving Failed."
            };
            return Json(response);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var blog = await _db.TblBlogs.AsNoTracking().FirstOrDefaultAsync(x => x.BlogId == id);
            if (blog is null)
            {
                return Json(new { IsSuccess = false, Message = "Blog not found." });
            }
            return Json(blog);
        }
        
        // Returns the edit page view
        public IActionResult EditPage(int id)
        {
            ViewBag.BlogId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, TblBlog blog)
        {
            var item = await _db.TblBlogs.FirstOrDefaultAsync(x => x.BlogId == id);
            if (item is null)
            {
                return Json(new { IsSuccess = false, Message = "Blog not found." });
            }
            
            item.BlogTitle = blog.BlogTitle;
            item.BlogAuthor = blog.BlogAuthor;
            item.BlogContent = blog.BlogContent;
            
            var result = await _db.SaveChangesAsync();
            var response = new
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Updating Successful." : "Updating Failed."
            };
            
            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.TblBlogs.FirstOrDefaultAsync(x => x.BlogId == id);
            if (item is null)
            {
                return Json(new { IsSuccess = false, Message = "Blog not found." });
            }

            _db.TblBlogs.Remove(item);
            var result = await _db.SaveChangesAsync();

            var response = new
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Deleting Successful." : "Deleting Failed."
            };
            
            return Json(response);
        }
    }
}
