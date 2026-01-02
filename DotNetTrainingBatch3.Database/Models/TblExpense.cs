using System;
using System.Collections.Generic;

namespace DotNetTrainingBatch3.Database.Models;

public partial class TblExpense
{
    public int ExpenseId { get; set; }

    public decimal Amount { get; set; }

    public string Category { get; set; } = null!;

    public string? Note { get; set; }
}
