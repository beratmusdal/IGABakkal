using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAMarket.DtoLayer.DailyClosurDtos
{
    public class ProductStockUpdateDto
    {
        public long Id { get; set; }
        public int NewStock { get; set; }
    }
}
