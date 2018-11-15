using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;

namespace LesoftWuye2.Application.WuyeApis.Dto
{
    public class GetPUnitListResult : InvokeResultDto
    {
        public GetPUnitListResult()
        {
            pUnits = new List<RecordItem>();
        }

        public string RecordNum { get; set; }
        public List<RecordItem> pUnits { get; set; }

        public class RecordItem
        {
            public string PUnitName { get; set; }
            public string PUnitCode { get; set; }
        }
    }

   
}
