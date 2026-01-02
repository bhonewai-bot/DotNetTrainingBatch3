using DotNetTrainingBatch3.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetTrainingBatch3.Mvc.Controllers
{
    public class ProductController : Controller
    {
        private readonly DotNetTrainingBatch3DbContext _db;

        public ProductController(DotNetTrainingBatch3DbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _db.TblProducts
                .AsNoTracking()
                .OrderByDescending(x => x.ProductId)
                .ToListAsync();
            
            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(TblProduct product)
        {
            await _db.AddAsync(product);
            await _db.SaveChangesAsync();
            
            TempData["StatusMessage"] = "New product created successfully";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _db.TblProducts.FirstOrDefaultAsync(x => x.ProductId == id);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, TblProduct product)
        {
            var item = await _db.TblProducts.FirstOrDefaultAsync(x => x.ProductId == id);
            if (item is null)
            {
                return RedirectToAction("Index");
            }
            
            item.ProductName = product.ProductName;
            item.Price = product.Price;
            item.Quantity = product.Quantity;
            
            await _db.SaveChangesAsync();
            
            TempData["StatusMessage"] = "Product updated successfully";
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _db.TblProducts.FirstOrDefaultAsync(x => x.ProductId == id);
            if (product is null)
            {
                return RedirectToAction("Index");
            }
            
            _db.TblProducts.Remove(product);
            await _db.SaveChangesAsync();
            
            TempData["StatusMessage"] = "Product deleted successfully";
            
            return RedirectToAction("Index");
        }
    }
}
