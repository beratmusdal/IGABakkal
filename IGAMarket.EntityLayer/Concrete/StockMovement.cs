using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAMarket.EntityLayer.Concrete
{
    public class StockMovement : BaseEntity
    {
        public int Id { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; } 
        public DateTime MovementDate { get; set; }
        public string MovementType { get; set; } 
        public Product Product { get; set; } 
    }
}
