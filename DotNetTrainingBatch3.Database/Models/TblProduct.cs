using System;
using System.Collections.Generic;

namespace DotNetTrainingBatch3.Database.Models;

public partial class TblProduct
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal Price { get; set; }

    public int? Quantity { get; set; }
}
