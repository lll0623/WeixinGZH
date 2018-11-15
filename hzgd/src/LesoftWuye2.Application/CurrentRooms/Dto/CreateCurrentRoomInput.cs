 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.CurrentRooms;


 
namespace LesoftWuye2.Application.CurrentRooms.Dto { 
 [AutoMap(typeof(CurrentRoom))]
 public class CreateCurrentRoomInput {

[Display(Name = "会员id")]
[MaxLength(LesoftWuye2.Core.CurrentRooms.CurrentRoom.MaxUserIdLength)]
public string UserId {get;set;} 

[Display(Name = "房间名称")]
[MaxLength(LesoftWuye2.Core.CurrentRooms.CurrentRoom.MaxNameLength)]
[Required]
public string Name {get;set;} 

[Display(Name = "房间编号")]
[MaxLength(LesoftWuye2.Core.CurrentRooms.CurrentRoom.MaxCodeLength)]
[Required]
public string Code {get;set;} 


}}
