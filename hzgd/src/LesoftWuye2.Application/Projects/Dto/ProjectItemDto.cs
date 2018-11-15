 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.Projects;
 using Abp.Application.Services.Dto;

 
namespace LesoftWuye2.Application.Projects.Dto { 
 [AutoMapFrom(typeof(Project))]
 public class ProjectItemDto:EntityDto<long> {

[Display(Name = "小区名称")]
public string Name {get;set;} 

[Display(Name = "小区代号")]
public string Code {get;set;} 


}}
