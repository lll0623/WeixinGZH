using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesoftWuye2.Application.WuyeApis.Dto
{
    public class GetPRoomPrePayItemResult : InvokeResultDto
    {
        public GetPRoomPrePayItemResult()
        {
            ItemList=new List<PayItem>();
        }

        public List<PayItem> ItemList { get; set; }

        public class PayItem
        {
            public int ItemID { get; set; }
            public string ItemName { get; set; }
            public int Type { get; set; }

            public decimal MonthPrice { get; set; }

            public string BeginYM { get; set; }
        }
    }
}
