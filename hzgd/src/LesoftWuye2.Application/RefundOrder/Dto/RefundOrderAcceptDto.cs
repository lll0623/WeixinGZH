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
  
    public class RefundOrderAcceptDto
    {
        public long OrderId { get; set; }
    }
}
