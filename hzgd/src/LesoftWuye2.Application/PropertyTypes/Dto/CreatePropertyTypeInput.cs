 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.PropertyTypes;


 
namespace LesoftWuye2.Application.PropertyTypes.Dto { 
 [AutoMap(typeof(PropertyType))]
 public class CreatePropertyTypeInput {

[Display(Name = "名称")]
[MaxLength(LesoftWuye2.Core.PropertyTypes.PropertyType.MaxNameLength)]
[Required]
public string Name {get;set;} 

[Display(Name = "排序号")]
public int Sort {get;set;} 


}}
