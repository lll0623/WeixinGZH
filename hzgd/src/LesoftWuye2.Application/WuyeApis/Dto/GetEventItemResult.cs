using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;

namespace LesoftWuye2.Application.WuyeApis.Dto
{
    public class GetEventItemResult : InvokeResultDto
    {
       
        public List<RecordItem> Info { get; set; }

        public class RecordItem
        {
            public string EType { get; set; }

            public string BillCode { get; set; }

            public string BillDate { get; set; }

            public string Content { get; set; }
            
        }
    }

   
}
