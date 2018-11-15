using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using LesoftWuye2.Application.Plates.Dto;

namespace LesoftWuye2.Application.Forums.Dto
{
    public class ForumHomeData
    {
        public IReadOnlyList<PlateListDto> Plates { get; set; }

        public List<PostListItem> TopPosts { get; set; }
    }
}
