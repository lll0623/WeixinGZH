using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.Categories;
using Abp.Application.Services.Dto;
using LesoftWuye2.Core.Plates;


namespace LesoftWuye2.Application.Plates.Dto
{
    [AutoMapFrom(typeof(Plate))]
    public class PlateItemDto : EntityDto<long>
    {

        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "缩略图")]
        public string Thumbnail { get; set; }

        [Display(Name = "排序号")]
        public int Sort { get; set; }


    }
}
