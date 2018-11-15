 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.PayNotifys;
 using Abp.Application.Services.Dto;

 
namespace LesoftWuye2.Application.PayNotifys.Dto { 
 [AutoMap(typeof(PayNotify))]
 public class UpdatePayNotifyInput:EntityDto<long>{


}}
