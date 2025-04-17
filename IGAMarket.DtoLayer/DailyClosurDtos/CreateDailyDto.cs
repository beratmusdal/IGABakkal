using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAMarket.DtoLayer.DailyClosurDtos
{
    public class CreateDailyDto
    {
        public string PersonelName { get; set; }
        public string ZNo { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalRefund { get; set; }
        public int TotalFireQuantity { get; set; }
        public string Description { get; set; }
    }
}
