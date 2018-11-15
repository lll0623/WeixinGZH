 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.LifeInfoTypes;
 using Abp.Application.Services.Dto;

 
namespace LesoftWuye2.Application.LifeInfoTypes.Dto { 
 [AutoMapFrom(typeof(LifeInfoType))]
 public class LifeInfoTypeListDto:EntityDto<long> {

[Display(Name = "名称")]
public string Name {get;set;} 

[Display(Name = "缩略图")]
public string Thumbnail {get;set;} 

[Display(Name = "排序号")]
public int Sort {get;set;} 

 public DateTime CreationTime { get; set; }


}}
