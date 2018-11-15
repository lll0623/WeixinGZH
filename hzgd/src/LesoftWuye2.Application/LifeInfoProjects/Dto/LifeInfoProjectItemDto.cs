 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.LifeInfoProjects;
 using Abp.Application.Services.Dto;

 
namespace LesoftWuye2.Application.LifeInfoProjects.Dto { 
 [AutoMapFrom(typeof(LifeInfoProject))]
 public class LifeInfoProjectItemDto:EntityDto<long> {


}}
