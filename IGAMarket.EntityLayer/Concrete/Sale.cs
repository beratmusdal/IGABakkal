namespace IGAMarket.EntityLayer.Concrete
{
    public class Sale : BaseEntity
    {
        public long Id { get; set; }
        public decimal TotalAmount { get; set; } // Satışın toplam tutarı
        public List<SaleItem> SaleItems { get; set; }
    }
}