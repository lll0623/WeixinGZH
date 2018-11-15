using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;

namespace LesoftWuye2.Application.WuyeApis.Dto
{
    public class GetMessageInfoResult : InvokeResultDto
    {
        public string BillCode { get; set; }

        public MessagesInfoData Info { get; set; }

        public class MessagesInfoData
        {
            public string Title { get; set; }

            public string Date { get; set; }

            public string Content { get; set; }
        }
    }

   
}
