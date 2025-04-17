using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAMarket.DtoLayer.MonthlyDtos;

public class MonthlyDto
{
    public DateTime CreateDate { get; set; } 
    public decimal TotalAmount { get; set; }
    public string ZNo { get; set; }
    public decimal TotalRefund { get; set; }  
    public int TotalFireQuantity { get; set; } 
    public decimal NetAmount => TotalAmount - TotalRefund; 
    public string PersonelName { get; set; }  
    public int TotalGiftQuantity { get; set; }

}

