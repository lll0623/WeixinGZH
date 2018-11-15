 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.Wuyebase.Members;


 
namespace LesoftWuye2.Application.Wuyebase.Members.Dto { 
 [AutoMap(typeof(Member))]
 public class CreateMemberInput {

[Display(Name = "名称")]
[MaxLength(LesoftWuye2.Core.Wuyebase.Members.Member.MaxNameLength)]
[Required]
public string Name {get;set;} 

[Display(Name = "缩略图")]
[MaxLength(LesoftWuye2.Core.Wuyebase.Members.Member.MaxThumbnailLength)]
public string Thumbnail {get;set;} 


}}
