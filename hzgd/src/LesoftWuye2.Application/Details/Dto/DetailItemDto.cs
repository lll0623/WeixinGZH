 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.Details;
 using Abp.Application.Services.Dto;

 
namespace LesoftWuye2.Application.Details.Dto { 
 [AutoMapFrom(typeof(Detail))]
 public class DetailItemDto:EntityDto<long> {


}}
