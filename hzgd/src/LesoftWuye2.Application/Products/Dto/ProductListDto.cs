 using System;
 using System.ComponentModel.DataAnnotations;
 using Abp.AutoMapper;
 using LesoftWuye2.Core.Products;
 using Abp.Application.Services.Dto;

 
namespace LesoftWuye2.Application.Products.Dto { 
 [AutoMapFrom(typeof(Product))]
 public class ProductListDto:EntityDto<long> {

[Display(Name = "名称")]
public string Name {get;set;} 

[Display(Name = "规格")]
public string Specification {get;set;} 

[Display(Name = "单位")]
public string Unit {get;set;} 

[Display(Name = "是否销售中")]
public bool IsSale {get;set;} 

[Display(Name = "价格")]
public decimal Price {get;set;} 

[Display(Name = "销售价格")]
public decimal SalePrice {get;set;} 

[Display(Name = "业主价格")]
public decimal MemberPrice {get;set;} 

[Display(Name = "缩略图")]
public string Thumbnail {get;set;} 

[Display(Name = "排序号")]
public int Sort {get;set;} 

 public DateTime CreationTime { get; set; }
        public virtual int SellCount { get; set; }

    }
}
