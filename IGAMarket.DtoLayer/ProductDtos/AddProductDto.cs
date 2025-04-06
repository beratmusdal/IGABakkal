using IGAMarket.EntityLayer.Enums;
using Microsoft.AspNetCore.Http;

namespace IGAMarket.DtoLayer.ProductDtos;

public class AddProductDto
{
    public string Name { get; set; }
    public string Barcode { get; set; }
    public decimal PurchasePrice { get; set; } // Alış fiyatı
    public decimal SalePrice { get; set; } // Satış fiyatı
    public CategoryEnum Category { get; set; }
    public int StockQuantity { get; set; } // Stok miktarı
    public string ImageUrl { get; set; } // Ürün fotoğrafı
    public IFormFile? ImageFile { get; set; } // Resim dosyası

}
