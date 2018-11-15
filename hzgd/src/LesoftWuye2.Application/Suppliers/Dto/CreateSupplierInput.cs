 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.Suppliers;


 
namespace LesoftWuye2.Application.Suppliers.Dto { 
 [AutoMap(typeof(Supplier))]
 public class CreateSupplierInput {

[Display(Name = "名称")]
[MaxLength(LesoftWuye2.Core.Suppliers.Supplier.MaxNameLength)]
[Required]
public string Name {get;set;} 


}}
