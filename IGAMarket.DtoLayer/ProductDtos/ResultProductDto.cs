using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAMarket.DtoLayer.ProductDtos
{
    public class ResultProductDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long Barcode { get; set; }
        public decimal PurchasePrice { get; set; } // Alış fiyatı
        public decimal SalePrice { get; set; } // Satış fiyatı
        public int StockQuantity { get; set; } // Stok miktarı
        public string Category { get; set; }
        public string? ImageUrl { get; set; } // Ürün fotoğrafı
        public DateTime CreateDate { get; set; }
    }   

}
