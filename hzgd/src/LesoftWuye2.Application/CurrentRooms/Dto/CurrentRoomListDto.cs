 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.CurrentRooms;
 using Abp.Application.Services.Dto;

 
namespace LesoftWuye2.Application.CurrentRooms.Dto { 
 [AutoMapFrom(typeof(CurrentRoom))]
 public class CurrentRoomListDto:EntityDto<long> {

[Display(Name = "会员id")]
public string UserId {get;set;} 

[Display(Name = "房间名称")]
public string Name {get;set;} 

[Display(Name = "房间编号")]
public string Code {get;set;} 

 public DateTime CreationTime { get; set; }


}}
