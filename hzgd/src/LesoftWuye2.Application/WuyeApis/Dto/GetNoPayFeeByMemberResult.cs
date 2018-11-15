using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;

namespace LesoftWuye2.Application.WuyeApis.Dto
{
    public class GetNoPayFeeByMemberResult : InvokeResultDto
    {
        public string BillCode { get; set; }

        public List<FeeInfoData> Info { get; set; }

        public class FeeInfoData
        {
            public string Id { get; set; }
            public string Name { get; set; }

            public string BDate { get; set; }

            public string EDate { get; set; }

            public string Count { get; set; }

            public string MemberId { get; set; }

            public string MemberName { get; set; }

            public string PRoomCode { get; set; }

            public List<FeeItemInfoData> Items { get; set; }
        }

        public class FeeItemInfoData
        {
            public string FeeId { get; set; }
            public string FeeName { get; set; }
            public string FeeBDate { get; set; }
            public string FeeEDate { get; set; }
            public string FeeCount { get; set; }
        }



    }


}
