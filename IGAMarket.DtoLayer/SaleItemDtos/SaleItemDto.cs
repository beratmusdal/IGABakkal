using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAMarket.DtoLayer.SaleItemDtos
{
    public class SaleItemDto
    {
        public string Barcode { get; set; }  // productId'yi Barcode olarak kabul ediyoruz
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
