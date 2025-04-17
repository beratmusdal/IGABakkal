using IGAMarket.EntityLayer.Enums;

namespace IGAMarket.EntityLayer.Concrete
{
    public class Sale : BaseEntity
    {
        public long Id { get; set; }
        public decimal TotalAmount { get; set; } 
        public StatusEnum? Status { get; set; } 

        public string CashierName { get; set; } 

        public List<SaleItem> SaleItems { get; set; }
    }

}