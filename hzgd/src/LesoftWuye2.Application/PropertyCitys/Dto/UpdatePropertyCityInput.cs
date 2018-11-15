 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.PropertyCitys;
 using Abp.Application.Services.Dto;

 
namespace LesoftWuye2.Application.PropertyCitys.Dto { 
 [AutoMap(typeof(PropertyCity))]
 public class UpdatePropertyCityInput:EntityDto<long>{

[Display(Name = "名称")]
[MaxLength(LesoftWuye2.Core.PropertyCitys.PropertyCity.MaxNameLength)]
[Required]
public string Name {get;set;} 

[Display(Name = "排序号")]
public int Sort {get;set;} 


}}
