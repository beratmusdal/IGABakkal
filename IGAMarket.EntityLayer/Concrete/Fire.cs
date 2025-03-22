namespace IGAMarket.EntityLayer.Concrete;

public class Fire : BaseEntity
{ 
    public long Id { get; set; }
    public long Barcode { get; set; }
    public int Quantity { get; set; } // Hibe edilen miktar
    public string Reason { get; set; } // Neden hibe edildi?
}
