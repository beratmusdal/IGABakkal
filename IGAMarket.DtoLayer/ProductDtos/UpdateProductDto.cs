﻿using IGAMarket.EntityLayer.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAMarket.DtoLayer.ProductDtos
{
    public class UpdateProductDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public CategoryEnum Category { get; set; }
        public string PurchasePrice { get; set; } // Alış fiyatı
        public string SalePrice { get; set; } // Satış fiyatı
        public int StockQuantity { get; set; } // Stok miktarı
        public string? ImageUrl { get; set; } // Ürün fotoğrafı
        public IFormFile? ImageFile { get; set; } // Resim dosyası
    }
}