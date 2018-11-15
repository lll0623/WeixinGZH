using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesoftWuye2.Application.Weixin.Dto
{
    public class RefundApplyDto
    {
        public long Id { get; set; }
        public int Type { get; set; }
        public string Remark { get; set; }
        public List<string> Images { get; set; }
    }
}
