using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;

namespace LesoftWuye2.Application.WuyeApis.Dto
{
    public class SearchHousingManagementResult : InvokeResultDto
    {
        public SearchHousingManagementResult()
        {
            infos = new List<RecordItem>();
        }

        public bool HasData
        {
            get
            {
                foreach (var item in infos)
                {
                    if (!string.IsNullOrEmpty(item.tel))
                        return true;
                }
                return false;
            }
        }

        public List<RecordItem> infos { get; set; }

        public class RecordItem
        {
            public string type { get; set; }
            public string name { get; set; }
            public string tel { get; set; }

            public string title
            {
                get
                {
                    switch (type)
                    {
                        case "0":
                            return "客服电话";
                        case "1":
                            return "管家";
                        default:
                            return "未知";
                    }
                }
            }
        }
    }

   
}
