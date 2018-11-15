 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.Activitys;
 using Abp.Application.Services.Dto;

 
namespace LesoftWuye2.Application.Activitys.Dto { 
 [AutoMap(typeof(ActivityPerson))]
 public class UpdateActivityPersonInput:EntityDto<long>{


}}
