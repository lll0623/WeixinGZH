 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.ActivityProjects;
 using Abp.Application.Services.Dto;

 
namespace LesoftWuye2.Application.ActivityProjects.Dto { 
 [AutoMapFrom(typeof(ActivityProject))]
 public class ActivityProjectListDto:EntityDto<long> {

 public DateTime CreationTime { get; set; }


}}
