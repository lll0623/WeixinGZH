 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.SlideImages;
 using Abp.Application.Services.Dto;
 using LesoftWuye2.Core.Products;


namespace LesoftWuye2.Application.ProductSlideImages.Dto { 
 [AutoMap(typeof(ProductSlideImage))]
 public class ProductUpdateSlideImageInput : EntityDto<long>{

[Display(Name = "缩略图")]
[MaxLength(LesoftWuye2.Core.SlideImages.SlideImage.MaxThumbnailLength)]
public string Thumbnail {get;set;} 

[Display(Name = "排序号")]
public int Sort {get;set;} 


}}
