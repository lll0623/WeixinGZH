using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Events.Bus;

namespace LesoftWuye2.Application.Weixin.Events.EventDatas
{
   public class GrouponFailEventData : EventData
    {
       public GrouponFailEventData()
       {
           Orders=new List<CanceOrder>();
       }

       public List<CanceOrder> Orders { get;private set; }


        public class CanceOrder
        {
            public long OrderId { get; set; }
            public long MemberId { get; set; }
            public decimal Price { get; set; }

            public string ProductName { get; set; }
        }
    }
}
