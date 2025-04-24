using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAMarket.DtoLayer.FireDtos;

public class SelectFireDto
{
    public long Id { get; set; }
    public string Barcode { get; set; }
    public int Quantity { get; set; }
    public string Reason { get; set; } // Açıklama gibi kullanılıyor
    public decimal TotalPurchasePrice { get; set; } // Quantity * PurchasePrice
    public string Name { get; set; } // Ürün adı
    public DateTime CreateDate { get; set; }
    public bool IsDeleted { get; set; } // Hatalı isdeletec yerine bu
}
