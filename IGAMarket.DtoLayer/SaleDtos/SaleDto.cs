using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAMarket.DtoLayer.SaleDtos
{
    public class SaleDto
    {
        public decimal TotalAmount { get; set; }
        public List<SaleItemDto> SaleItems { get; set; }
    }

    public class SaleItemDto
    {
        public long Id { get; set; }  // productId'yi Barcode olarak kabul ediyoruz
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

}
