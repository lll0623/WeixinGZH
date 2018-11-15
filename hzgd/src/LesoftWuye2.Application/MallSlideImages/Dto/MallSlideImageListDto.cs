using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.MallSlideImages;
using Abp.Application.Services.Dto;


namespace LesoftWuye2.Application.MallSlideImages.Dto
{
    [AutoMapFrom(typeof(MallSlideImage))]
    public class MallSlideImageListDto : EntityDto<long>
    {

        [Display(Name = "缩略图")]
        public string Thumbnail { get; set; }

        [Display(Name = "排序号")]
        public int Sort { get; set; }

        public DateTime CreationTime { get; set; }

        public virtual string Url { get; set; }
    }
}
