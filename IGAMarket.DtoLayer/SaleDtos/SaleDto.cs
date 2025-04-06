using IGAMarket.DtoLayer.SaleItemDtos;
using IGAMarket.EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAMarket.DtoLayer.SaleDtos
{
    public class SaleDto
    {
        public long Id { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public StatusEnum Status { get; set; }
        public List<SaleItemDto> SaleItems { get; set; }
    }



}
