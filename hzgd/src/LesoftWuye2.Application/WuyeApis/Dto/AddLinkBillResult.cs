using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;

namespace LesoftWuye2.Application.WuyeApis.Dto
{
    public class AddLinkBillResult : InvokeResultDto
    {
        public string BillCode { get; set; }

        public OtherInfoData OtherInfo { get; set; }

        public class OtherInfoData
        {
            public string Msg { get; set; }
            public string Tel { get; set; }
        }
    }

   
}
