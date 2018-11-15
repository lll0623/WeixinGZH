using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesoftWuye2.Application.WuyeApis.Dto
{
    public class GetUserInfoResult : InvokeResultDto
    {
        public List<GetUserInfoInfo> infos { get; set; }

        public class GetUserInfoInfo
        {
            public string Id { get; set; }
            public string NickName { get; set; }
            public string HeadImage { get; set; }
            public string PProjectName { get; set; }

            public string PRoomFullName { get; set; }

        }
    }
}
