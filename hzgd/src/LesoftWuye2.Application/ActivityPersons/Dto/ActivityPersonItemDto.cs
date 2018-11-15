 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.Activitys;
 using Abp.Application.Services.Dto;

 
namespace LesoftWuye2.Application.Activitys.Dto { 
 [AutoMapFrom(typeof(ActivityPerson))]
 public class ActivityPersonItemDto:EntityDto<long> {


}}
