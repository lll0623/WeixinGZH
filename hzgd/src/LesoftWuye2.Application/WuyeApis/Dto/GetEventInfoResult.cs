using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;

namespace LesoftWuye2.Application.WuyeApis.Dto
{
    public class GetEventInfoResult : InvokeResultDto
    {
       
        public RecordItem Info { get; set; }

        public class RecordItem
        {  
            public string BillCode { get; set; }
            public string State { get; set; }
            public string BusiType { get; set; }

            public string PRoomCode { get; set; }

            public string Date { get; set; }

            public string Content { get; set; }

            public List<string > File { get; set; }

            public string SendDate { get; set; }

            public string SendMan { get; set; }
            public string ReceiveDate { get; set; }
            public string ReceiveMan { get; set; }

            public string CompleteMemo { get; set; }

            public string ArriveDate { get; set; }
            public string RBDate { get; set; }
            public string REDate { get; set; }
            public string RepaireHour { get; set; }
            public string FeeCount { get; set; }
            public string MaterialFee { get; set; }
            public string LaborFee { get; set; }
            public string OtherFee { get; set; }
            public string RepaireType { get; set; }
            public List<string> RepairedFile { get; set; }
            public string ReturnVisitDate { get; set; }
            public string ReturnVisitMan { get; set; }
            public string ReturnVisitResult { get; set; }

            public string ReturnVisitCustSuggest { get; set; }

            public string IsProject { get; set; }
            public string HandelSugg { get; set; }

            public List<MsgItem> MSGList { get; set; }
        }

        public class MsgItem
        {
            public int Type { get; set; }
            public string Content { get; set; }
            public string CreateTime { get; set; }

            public string Name {
                get
                {
                    if (Type == 1)
                        return "我";
                    return "物业";
                }
            }
        }
    }

   
}
