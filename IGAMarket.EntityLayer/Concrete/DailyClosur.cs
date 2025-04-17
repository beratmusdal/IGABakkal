namespace IGAMarket.EntityLayer.Concrete
{
    public class DailyClosur : BaseEntity
    {
        public long Id { get; set; }
        public string PersonelName { get; set; }
        public string ZNo { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalRefund { get; set; }
        public int TotalFireQuantity { get; set; }
        public string Description { get; set; }
    }
}