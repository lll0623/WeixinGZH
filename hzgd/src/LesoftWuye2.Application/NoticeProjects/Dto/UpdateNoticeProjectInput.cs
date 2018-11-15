 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.NoticeProjects;
 using Abp.Application.Services.Dto;

 
namespace LesoftWuye2.Application.NoticeProjects.Dto { 
 [AutoMap(typeof(NoticeProject))]
 public class UpdateNoticeProjectInput:EntityDto<long>{


}}
