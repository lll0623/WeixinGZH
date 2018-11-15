using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using LesoftWuye2.Core.Estateinfos;
using LesoftWuye2.Core.Mall.Orders;

namespace LesoftWuye2.Application.Orders.Dto
{
    [AutoMap(typeof(OrderShip))]
    public class OrderShipDto
    {
        public long OrderId { get; set; }
        
        public virtual string ExpressCode { get; set; }
        
        public virtual string ExpressNo { get; set; }
    }
}
