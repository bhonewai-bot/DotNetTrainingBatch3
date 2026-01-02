namespace DotNetTrainingBatch3.Mvc.Models;

public class SaleViewModel
{
    public int SaleId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Total { get; set; }
    public DateTime? SaleDate { get; set; }
}