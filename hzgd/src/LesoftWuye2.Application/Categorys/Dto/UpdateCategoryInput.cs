 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.Categories;
 using Abp.Application.Services.Dto;

 
namespace LesoftWuye2.Application.Categories.Dto { 
 [AutoMap(typeof(Category))]
 public class UpdateCategoryInput:EntityDto<long>{

[Display(Name = "名称")]
[MaxLength(LesoftWuye2.Core.Categories.Category.MaxNameLength)]
[Required]
public string Name {get;set;} 

[Display(Name = "缩略图")]
[MaxLength(LesoftWuye2.Core.Categories.Category.MaxThumbnailLength)]
public string Thumbnail {get;set;} 

[Display(Name = "排序号")]
public int Sort {get;set;} 


}}
