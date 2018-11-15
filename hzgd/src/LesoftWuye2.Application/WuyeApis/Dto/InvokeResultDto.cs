using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesoftWuye2.Application.WuyeApis.Dto
{
    /// <summary>
    /// 物业api接口通用执行结果
    /// </summary>
    public class InvokeResultDto
    {
        public string result { get; set; }
        public string msg { get; set; }
        public bool Success {
            get { return result == "success"; }
        }
    }
}
