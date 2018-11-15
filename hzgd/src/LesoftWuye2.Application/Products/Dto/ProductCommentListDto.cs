using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesoftWuye2.Application.Products.Dto
{
    public class ProductCommentListDto
    {
        public long Id { get; set; }

        public string MemberName { get; set; }

        public string ProductName { get; set; }

        public string Content { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
