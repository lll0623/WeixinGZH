﻿ using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.Projects;


 
namespace LesoftWuye2.Application.Projects.Dto { 
 [AutoMap(typeof(Project))]
 public class CreateProjectInput {

[Display(Name = "小区名称")]
[MaxLength(LesoftWuye2.Core.Projects.Project.MaxNameLength)]
[Required]
public string Name {get;set;} 

[Display(Name = "小区代号")]
[MaxLength(LesoftWuye2.Core.Projects.Project.MaxCodeLength)]
[Required]
public string Code {get;set;} 


}}
