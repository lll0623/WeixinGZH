using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LesoftWuye2.Application.LifeInfoTypes.Dto;
using LesoftWuye2.Application.ServiceTels.Dto;

namespace LesoftWuye2.Application.Weixin.Dto
{
    public class LifeInfoNavDto
    {
        public List<string> SlideImages { get; set; }
        public List<LifeInfoTypeListDto> Items { get; set; }
    }
}
