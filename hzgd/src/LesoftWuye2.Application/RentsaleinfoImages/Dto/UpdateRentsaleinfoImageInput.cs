 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.Rentsaleinfos;
 using Abp.Application.Services.Dto;

 
namespace LesoftWuye2.Application.Rentsaleinfos.Dto { 
 [AutoMap(typeof(RentsaleinfoImage))]
 public class UpdateRentsaleinfoImageInput:EntityDto<long>{


}}
