using DotNetTrainingBatch3.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetTrainingBatch3.Mvc.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly DotNetTrainingBatch3DbContext _db;

        public ExpenseController(DotNetTrainingBatch3DbContext db)
        {
            _db = db;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var expenses = await _db.TblExpenses
                .AsNoTracking()
                .OrderByDescending(x => x.ExpenseId)
                .ToListAsync();
            
            return Json(expenses);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(TblExpense expense)
        {
            await _db.TblExpenses.AddAsync(expense);
            var result = await _db.SaveChangesAsync();

            var response = new
            {
                IsSuccess = result > 0,
                StatusMessage = result > 0 ? "Expenses created successfully" : "Failed to create new expense"
            };
            
            return Json(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetExpense(int id)
        {
            var expense = await _db.TblExpenses.FirstOrDefaultAsync(x => x.ExpenseId == id);
            if (expense is null)
            {
                return Json(new { IsSuccess = false, Message = "Expense not found" });
            }
            
            return Json(expense);
        }

        public IActionResult Edit(int id)
        {
            ViewBag.ExpenseId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, TblExpense expense)
        {
            var item = await _db.TblExpenses.FirstOrDefaultAsync(x => x.ExpenseId == id);
            if (item is null)
            {
                return Json(new { IsSuccess = false, Message = "Expense not found" });
            }
            
            item.Category = expense.Category;
            item.Amount = expense.Amount;
            item.Note = expense.Note;

            var result = await _db.SaveChangesAsync();
            
            var response = new
            {
                IsSuccess = result > 0,
                StatusMessage = result > 0 ? "Expenses updated successfully" : "Failed to update expense"
            };
            
            return Json(response);
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item = _db.TblExpenses.FirstOrDefault(x => x.ExpenseId == id);
            if (item is null)
            {
                return Json(new { IsSuccess = false, Message = "Expense not found" });
            }

            _db.TblExpenses.Remove(item);
            var result = await _db.SaveChangesAsync();
            
            var response = new
            {
                IsSuccess = result > 0,
                StatusMessage = result > 0 ? "Expenses updated successfully" : "Failed to update expense"
            };
            
            return Json(response);
        }
    }
}
