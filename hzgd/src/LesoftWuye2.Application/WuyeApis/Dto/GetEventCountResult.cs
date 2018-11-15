using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;

namespace LesoftWuye2.Application.WuyeApis.Dto
{
    public class GetEventCountResult : InvokeResultDto
    {
        public GetEventCountResult()
        {
            
        }

        
        public List<RecordItem> Info { get; set; }

        public class RecordItem
        {
            public int EType { get; set; }
            public string ETypeName { get; set; }

            public List<RecordSubItem> ETypeInfo { get; set; }
        }

        public class RecordSubItem
        {
            public int EDType { get; set; }
            public string EDTypeName { get; set; }

            public int EDCount { get; set; }
        }
    }

   
}
