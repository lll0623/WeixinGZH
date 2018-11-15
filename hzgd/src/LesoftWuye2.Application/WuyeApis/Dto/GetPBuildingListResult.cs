using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;

namespace LesoftWuye2.Application.WuyeApis.Dto
{
    public class GetPBuildingListResult : InvokeResultDto
    {
        public GetPBuildingListResult()
        {
            Records = new List<RecordItem>();
        }

        public string RecordNum { get; set; }
        public List<RecordItem> Records { get; set; }

        public class RecordItem
        {
            public string Name { get; set; }
            public string Code { get; set; }
        }
    }

   
}
