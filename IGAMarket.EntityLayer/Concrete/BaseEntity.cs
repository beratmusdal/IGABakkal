namespace IGAMarket.EntityLayer.Concrete
{
    public class BaseEntity
    {
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
}