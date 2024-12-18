public class ItemDto
{
    public Guid Id { get; set; }
    public string PictureUrl { get; set; }
    public string Name { get; set; }
    public string Barcode { get; set; }
    public string CategoryName { get; set; }
    public decimal SellingPrice { get; set; }
    public decimal? SellingTaxPercentage { get; set; }
    public int Quantity { get; set; } // This is pre-aggregated in SQL

    public bool IsActive { get; set; }
}
