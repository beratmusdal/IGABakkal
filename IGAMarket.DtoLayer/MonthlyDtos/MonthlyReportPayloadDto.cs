using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAMarket.DtoLayer.MonthlyDtos;

public class MonthlyReportPayloadDto
{
    public List<MonthlyDto> Data { get; set; }
    public MonthlySummaryDto Summary { get; set; }
}

public class MonthlySummaryDto
{
    public decimal TotalSales { get; set; }
    public decimal TotalRefund { get; set; }
    public int ClosingCount { get; set; }
    public int TotalFire { get; set; }
    public int TotalGift { get; set; }
    public string Month { get; set; } // yyyy-MM
}
