 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.PayNotifys;
 using Abp.Application.Services.Dto;

 
namespace LesoftWuye2.Application.PayNotifys.Dto { 
 [AutoMapFrom(typeof(PayNotify))]
 public class PayNotifyListDto:EntityDto<long> {

 public DateTime CreationTime { get; set; }


}}
