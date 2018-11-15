 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.PropertyCitys;


 
namespace LesoftWuye2.Application.PropertyCitys.Dto { 
 [AutoMap(typeof(PropertyCity))]
 public class CreatePropertyCityInput {

[Display(Name = "名称")]
[MaxLength(LesoftWuye2.Core.PropertyCitys.PropertyCity.MaxNameLength)]
[Required]
public string Name {get;set;} 

[Display(Name = "排序号")]
public int Sort {get;set;} 


}}
