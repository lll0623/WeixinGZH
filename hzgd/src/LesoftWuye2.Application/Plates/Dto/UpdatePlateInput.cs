 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.Categories;
 using Abp.Application.Services.Dto;
 using LesoftWuye2.Core.Plates;


namespace LesoftWuye2.Application.Plates.Dto { 
 [AutoMap(typeof(Plate))]
 public class UpdatePlateInput:EntityDto<long>{

[Display(Name = "名称")]
[MaxLength(LesoftWuye2.Core.Plates.Plate.MaxNameLength)]
[Required]
public string Name {get;set;} 

[Display(Name = "缩略图")]
[MaxLength(LesoftWuye2.Core.Plates.Plate.MaxThumbnailLength)]
public string Thumbnail {get;set;} 

[Display(Name = "排序号")]
public int Sort {get;set;} 


}}
