 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.Estateinfos;
 using Abp.Application.Services.Dto;

 
namespace LesoftWuye2.Application.Estateinfos.Dto { 
 [AutoMapFrom(typeof(EstateinfoImage))]
 public class EstateinfoImageItemDto:EntityDto<long> {


}}
