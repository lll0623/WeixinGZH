 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.ServiceTels;
 using Abp.Application.Services.Dto;

 
namespace LesoftWuye2.Application.ServiceTels.Dto { 
 [AutoMapFrom(typeof(ServiceTel))]
 public class ServiceTelListDto:EntityDto<long> {

[Display(Name = "名称")]
public string Name {get;set;} 

[Display(Name = "电话号码")]
public string Tel {get;set;} 

[Display(Name = "排序号")]
public int Sort {get;set;} 

 public DateTime CreationTime { get; set; }


}}
