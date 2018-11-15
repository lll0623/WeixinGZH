using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesoftWuye2.Application.WuyeApis.Dto
{
    public class AddLinkBillDto
    {
        public string PRoomCode { get; set; }
        public string CustomerID { get; set; }

        public string MemberID { get; set; }

        public string BusiType { get; set; }

        public string LinkContent { get; set; }

        public string FileAddress { get; set; }

        public string LinkType => "微信报事报修";
    }
}
