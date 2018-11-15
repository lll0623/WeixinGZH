 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.PropertyTypes;
 using Abp.Application.Services.Dto;

 
namespace LesoftWuye2.Application.PropertyTypes.Dto { 
 [AutoMap(typeof(PropertyType))]
 public class UpdatePropertyTypeInput:EntityDto<long>{

[Display(Name = "名称")]
[MaxLength(LesoftWuye2.Core.PropertyTypes.PropertyType.MaxNameLength)]
[Required]
public string Name {get;set;} 

[Display(Name = "排序号")]
public int Sort {get;set;} 


}}
