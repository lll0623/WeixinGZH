using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.EstateinfoTypes;
using Abp.Application.Services.Dto;


namespace LesoftWuye2.Application.EstateinfoTypes.Dto
{
    [AutoMap(typeof(EstateinfoType))]
    public class UpdateEstateinfoTypeInput : EntityDto<long>
    {

        [Display(Name = "名称")]
        [MaxLength(LesoftWuye2.Core.EstateinfoTypes.EstateinfoType.MaxNameLength)]
        [Required]
        public string Name { get; set; }
  
        [Display(Name = "排序号")]
        public int Sort { get; set; }


    }
}
