using System;
using System.Collections.Generic;

namespace DotNetTrainingBatch3.Database.Models;

public partial class TblSale
{
    public int SaleId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public DateTime? SaleDate { get; set; }
}
