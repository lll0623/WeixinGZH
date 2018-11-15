using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;

namespace LesoftWuye2.Application.WuyeApis.Dto
{
    public class GetMessageItemResult : InvokeResultDto
    {
        public string BillCode { get; set; }

        public List<MessagesInfoData> MessagesInfo { get; set; }

        public class MessagesInfoData
        {
            public string Id { get; set; }
            public string Title { get; set; }

            public string Date { get; set; }

            public string Abstract { get; set; }

            public string State { get; set; }
        }
    }


}
