using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Events.Bus;

namespace LesoftWuye2.Application.Weixin.Events.EventDatas
{
    public class RefundOrderAcceptEventData : EventData
    {
        public long MemberId { get; set; }

        public string Amount { get; set; }

        public long OrderId { get; set; }

        public string OrderNo { get; set; }

        public string ProductName { get; set; }
    }
}
