using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.SlideImages;
using Abp.Application.Services.Dto;


namespace LesoftWuye2.Application.SlideImages.Dto
{
    [AutoMapFrom(typeof(SlideImage))]
    public class SlideImageItemDto : EntityDto<long>
    {

        [Display(Name = "缩略图")]
        public string Thumbnail { get; set; }

        [Display(Name = "排序号")]
        public int Sort { get; set; }


    }
}
