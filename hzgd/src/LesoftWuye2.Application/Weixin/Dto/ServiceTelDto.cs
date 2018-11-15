using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LesoftWuye2.Application.ServiceTels.Dto;

namespace LesoftWuye2.Application.Weixin.Dto
{
    public class ServiceTelDto
    {
        public List<string> SlideImages { get; set; }
        public List<ServiceTelListDto> Items { get; set; }
    }
}
