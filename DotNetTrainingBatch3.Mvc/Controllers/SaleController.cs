using DotNetTrainingBatch3.Database.Models;
using DotNetTrainingBatch3.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetTrainingBatch3.Mvc.Controllers
{
    public class SaleController : Controller
    {
        private readonly DotNetTrainingBatch3DbContext _db;

        public SaleController(DotNetTrainingBatch3DbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var sales = await _db.TblSales
                .Join(_db.TblProducts,
                    sale => sale.ProductId,
                    product => product.ProductId,
                    (sale, product) => new SaleViewModel()
                    {
                        SaleId = sale.SaleId,
                        ProductId = sale.ProductId,
                        ProductName = product.ProductName,
                        Price = product.Price,
                        Quantity = sale.Quantity,
                        Total = sale.Quantity * product.Price,
                        SaleDate = sale.SaleDate,
                    })
                .AsNoTracking()
                .OrderByDescending(x => x.SaleId)
                .ToListAsync();

            return Json(sales);
        }

        public async Task<IActionResult> Create()
        {
            var products = await _db.TblProducts.ToListAsync();
            
            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> Save(TblSale sale)
        {
            await _db.TblSales.AddAsync(sale);
            
            var product = await _db.TblProducts
                .FirstOrDefaultAsync(x => x.ProductId == sale.ProductId);

            if (product is null)
            {
                return Json(new { IsSuccess = false, Message = "Sale not found" });
            }
            
            product.Quantity -= sale.Quantity;
            
            var result = await _db.SaveChangesAsync();

            var response = new
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Sale created successfully!" : "Failed to create sale!"
            };
            
            return Json(response);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var sale = await  _db.TblSales
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.SaleId == id);

            if (sale is null)
            {
                return Json(new { IsSuccess = false, Message = "Sale not found" });
            }

            return Json(sale);
        }

        public async Task<IActionResult> EditPage(int id)
        {
            ViewBag.SaleId = id;
            var products = await _db.TblProducts.ToListAsync();
            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, TblSale sale)
        {
            var item = await _db.TblSales.FirstOrDefaultAsync(x => x.SaleId == id);
            if (item is null)
            {
                return Json(new { IsSuccess = false, Message = "Sale not found" });
            }
            
            item.ProductId = sale.ProductId;
            item.Quantity = sale.Quantity;
            item.SaleDate = sale.SaleDate;

            var result = await _db.SaveChangesAsync();
            
            var response = new
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Sale updated successfully!" : "Failed to update sale!"
            };
            
            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.TblSales.FirstOrDefaultAsync(x => x.SaleId == id);
            if (item is null)
            {
                return Json(new { IsSuccess = false, Message = "Sale not found" });
            }

            _db.TblSales.Remove(item);
            var result = await _db.SaveChangesAsync();

            var response = new
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Sale deleted successfully!" : "Failed to delete sale!"
            };
            
            return Json(response);
        }
    }
}