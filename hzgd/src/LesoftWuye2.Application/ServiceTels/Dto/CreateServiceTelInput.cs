 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.ServiceTels;


 
namespace LesoftWuye2.Application.ServiceTels.Dto { 
 [AutoMap(typeof(ServiceTel))]
 public class CreateServiceTelInput {

[Display(Name = "名称")]
[MaxLength(LesoftWuye2.Core.ServiceTels.ServiceTel.MaxNameLength)]
[Required]
public string Name {get;set;} 

[Display(Name = "电话号码")]
[MaxLength(LesoftWuye2.Core.ServiceTels.ServiceTel.MaxTelLength)]
[Required]
public string Tel {get;set;} 

[Display(Name = "排序号")]
public int Sort {get;set;} 


}}
