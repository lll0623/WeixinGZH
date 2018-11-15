using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesoftWuye2.Application.Malls.Dto
{
    public class OrderSubmitResult
    {
        public string OrderNo { get; set; }
        public decimal Amount { get; set; }

        public long OrderId { get; set; }
    }
}
