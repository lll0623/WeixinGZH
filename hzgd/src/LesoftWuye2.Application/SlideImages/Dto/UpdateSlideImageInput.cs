 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.SlideImages;
 using Abp.Application.Services.Dto;

 
namespace LesoftWuye2.Application.SlideImages.Dto { 
 [AutoMap(typeof(SlideImage))]
 public class UpdateSlideImageInput:EntityDto<long>{

[Display(Name = "缩略图")]
[MaxLength(LesoftWuye2.Core.SlideImages.SlideImage.MaxThumbnailLength)]
public string Thumbnail {get;set;} 

[Display(Name = "排序号")]
public int Sort {get;set;} 


}}
