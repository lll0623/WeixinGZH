 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.NoticeProjects;
 using Abp.Application.Services.Dto;

 
namespace LesoftWuye2.Application.NoticeProjects.Dto { 
 [AutoMapFrom(typeof(NoticeProject))]
 public class NoticeProjectListDto:EntityDto<long> {

 public DateTime CreationTime { get; set; }


}}
