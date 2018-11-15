using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesoftWuye2.Application.Forums.Dto
{
    public class NewCommentDto
    {
        public long MemberId { get; set; }

        public long PostId { get; set; }
        
        public string Content { get; set; }

        public List<string> Images { get; set; }
    }
}
