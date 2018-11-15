using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.MallSlideImages;
using LesoftWuye2.Core.SlideImages;



namespace LesoftWuye2.Application.MallSlideImages.Dto
{
    [AutoMap(typeof(MallSlideImage))]
    public class CreateMallSlideImageInput
    {

        [Display(Name = "缩略图")]
        [MaxLength(LesoftWuye2.Core.SlideImages.SlideImage.MaxThumbnailLength)]
        public string Thumbnail { get; set; }

        [Display(Name = "排序号")]
        public int Sort { get; set; }

        public virtual string Url { get; set; }
    }
}
