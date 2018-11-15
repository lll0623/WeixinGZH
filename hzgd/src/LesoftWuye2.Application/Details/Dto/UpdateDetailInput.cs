 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.Details;
 using Abp.Application.Services.Dto;

 
namespace LesoftWuye2.Application.Details.Dto { 
 [AutoMap(typeof(Detail))]
 public class UpdateDetailInput:EntityDto<long>{


}}
