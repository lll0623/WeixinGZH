using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.SlideImages;



namespace LesoftWuye2.Application.SlideImages.Dto
{
    [AutoMap(typeof(SlideImage))]
    public class CreateSlideImageInput
    {

        [Display(Name = "缩略图")]
        [MaxLength(LesoftWuye2.Core.SlideImages.SlideImage.MaxThumbnailLength)]
        public string Thumbnail { get; set; }

        [Display(Name = "排序号")]
        public int Sort { get; set; }


    }
}
