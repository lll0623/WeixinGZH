using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesoftWuye2.Application.Malls.Dto
{
    public class OrderSubmitInfo
    {
        public long MemberId { get; set; }

        public long ProductId { get; set; }

        public long GrouponId { get; set; }

        public int Type { get; set; }

        public long GrouponOrderId { get; set; }

        public int Num { get; set; }

        public long AddressId { get; set; }

        public string ClientRemark { get; set; }
    }
}
