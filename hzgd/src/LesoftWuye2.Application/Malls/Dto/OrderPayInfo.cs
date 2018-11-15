using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesoftWuye2.Application.Malls.Dto
{
    public class OrderPayInfo
    {
        public string OrderNo { get; set; }
        public string Channel { get; set; }
        public string TradeNo { get; set; }

        public decimal Money { get; set; }
    }
}
