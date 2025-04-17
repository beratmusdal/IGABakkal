using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAMarket.DtoLayer.MonthlyDtos;

public class SelectMonthlyDto
{
    public string PersonelName { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TotalRefund { get; set; }
    public int TotalFireQuantity { get; set; }
    public DateTime CreateDate { get; set; }
}
