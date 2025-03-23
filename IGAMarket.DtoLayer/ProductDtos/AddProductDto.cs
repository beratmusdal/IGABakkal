namespace IGAMarket.DtoLayer.ProductDtos;

public class AddProductDto
{
    public string Name { get; set; }
    public string Barcode { get; set; }
    public decimal PurchasePrice { get; set; } // Alış fiyatı
    public decimal SalePrice { get; set; } // Satış fiyatı
    public int StockQuantity { get; set; } // Stok miktarı
    public string ImageUrl { get; set; } // Ürün fotoğrafı
}
