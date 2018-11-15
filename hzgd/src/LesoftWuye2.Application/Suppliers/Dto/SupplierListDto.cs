 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.Suppliers;
 using Abp.Application.Services.Dto;

 
namespace LesoftWuye2.Application.Suppliers.Dto { 
 [AutoMapFrom(typeof(Supplier))]
 public class SupplierListDto:EntityDto<long> {

[Display(Name = "名称")]
public string Name {get;set;} 

 public DateTime CreationTime { get; set; }


}}
