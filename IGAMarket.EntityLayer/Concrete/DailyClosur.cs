namespace IGAMarket.EntityLayer.Concrete
{
    public class DailyClosur : BaseEntity
    {
        public long Id { get; set; }
        public decimal TotalSalePrice { get; set; }
        public decimal TotalDonationPrice { get; set; }
        public decimal TotalPrice => TotalSalePrice - TotalDonationPrice;
    }
}