using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesoftWuye2.Application.WuyeApis.Dto
{
    public class GetMemberInfoByAPPUserIDResult : InvokeResultDto
    {
        public int MemberID { get; set; }

        public string CustomerID { get; set; }

        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string PRoomFullName { get; set; }
        public string PRoomCode { get; set; }
        public int RecordNum { get; set; }

        public List<PRoomItem> PRooms { get; set; }

        public string PProjectCode { get; set; }

        public class PRoomItem
        {
            public string PRoomFullName { get; set; }
            public string PRoomCode { get; set; }

            public string PProjectCode { get; set; }
        }
    }
}
