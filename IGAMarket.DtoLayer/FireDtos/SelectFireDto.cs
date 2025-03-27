using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAMarket.DtoLayer.FireDtos
{
    public class SelectFireDto
    {
        public long Id { get; set; }
        public string Barcode { get; set; }
        public int Quantity { get; set; } // Hibe edilen miktar
        public string Reason { get; set; }
        public decimal TotalPurchasePrice { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
