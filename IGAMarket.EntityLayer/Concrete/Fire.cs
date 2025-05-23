﻿namespace IGAMarket.EntityLayer.Concrete;

public class Fire : BaseEntity
{ 
    public long Id { get; set; }
    public string Barcode { get; set; }
    public int Quantity { get; set; } // Hibe edilen miktar
    public string Reason { get; set; } // Neden hibe edildi?    
    public decimal PurchasePrice { get; set; } // Alış fiyatı
    public decimal TotalPrice { get; set; } 
}
