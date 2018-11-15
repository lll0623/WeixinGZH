 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.Wuyebase.Members;
 using Abp.Application.Services.Dto;

 
namespace LesoftWuye2.Application.Wuyebase.Members.Dto { 
 [AutoMapFrom(typeof(Member))]
 public class MemberItemDto:EntityDto<long> {

[Display(Name = "名称")]
public string Name {get;set;} 

[Display(Name = "缩略图")]
public string Thumbnail {get;set;} 


}}
