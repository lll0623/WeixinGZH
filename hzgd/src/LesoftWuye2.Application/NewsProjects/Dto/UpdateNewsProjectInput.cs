 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.NewsProjects;
 using Abp.Application.Services.Dto;

 
namespace LesoftWuye2.Application.NewsProjects.Dto { 
 [AutoMap(typeof(NewsProject))]
 public class UpdateNewsProjectInput:EntityDto<long>{


}}
