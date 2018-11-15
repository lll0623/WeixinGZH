using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesoftWuye2.Application.WuyeApis.Dto
{
   public class LoginResult: InvokeResultDto
    {
        public LoginResultInfo infos { get; set; }

        public class LoginResultInfo
        {
            public string Id { get; set; }
            public string NickName { get; set; }
            public string HeadImage { get; set; }
        }
    }
}
