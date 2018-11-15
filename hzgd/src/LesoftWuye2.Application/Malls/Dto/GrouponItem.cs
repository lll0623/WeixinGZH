using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using LesoftWuye2.Core.Groupons;

namespace LesoftWuye2.Application.Malls.Dto
{
    [AutoMapFrom(typeof(Groupon))]
    public class GrouponItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Thumbnail { get; set; }
        public decimal Price { get; set; }
        public int SellCount { get; set; }
        public string Unit { get; set; }
    }
}
