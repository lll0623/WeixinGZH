 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.SlideImages;
 using Abp.Application.Services.Dto;
 using LesoftWuye2.Core.Products;


namespace LesoftWuye2.Application.ProductSlideImages.Dto { 
 [AutoMapFrom(typeof(ProductSlideImage))]
 public class ProductSlideImageListDto : EntityDto<long> {

[Display(Name = "缩略图")]
public string Thumbnail {get;set;} 

[Display(Name = "排序号")]
public int Sort {get;set;} 

 public DateTime CreationTime { get; set; }


}}
