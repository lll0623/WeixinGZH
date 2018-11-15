using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LesoftWuye2.Application.MallSlideImages.Dto;

namespace LesoftWuye2.Application.Malls.Dto
{
    public class MallHomeData
    {
        public List<MallSlideImageListDto> SlideImages { get; set; }

        public List<GrouponItem> Groupons { get; set; }
    }
}
