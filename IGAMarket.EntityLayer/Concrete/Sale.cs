using IGAMarket.EntityLayer.Enums;

namespace IGAMarket.EntityLayer.Concrete
{
    public class Sale : BaseEntity
    {
        public long Id { get; set; }
        public decimal TotalAmount { get; set; } // Satışın toplam tutarı
        public StatusEnum? Status { get; set; } // Satışın durumu
        public List<SaleItem> SaleItems { get; set; }
    }
}