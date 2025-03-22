namespace IGAMarket.EntityLayer.Concrete
{
    public class SaleItem : BaseEntity
    {
        public long Id { get; set; }
        public long SaleId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}