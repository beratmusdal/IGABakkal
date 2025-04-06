using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAMarket.DtoLayer.FireDtos
{
    public class AddFireDto
    {
        public string Barcode { get; set; }
        public int Quantity { get; set; } 
        public string Reason { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
