using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.EstateinfoTypes;



namespace LesoftWuye2.Application.EstateinfoTypes.Dto
{
    [AutoMap(typeof(EstateinfoType))]
    public class CreateEstateinfoTypeInput
    {

        [Display(Name = "名称")]
        [MaxLength(LesoftWuye2.Core.EstateinfoTypes.EstateinfoType.MaxNameLength)]
        [Required]
        public string Name { get; set; }

        

        [Display(Name = "排序号")]
        public int Sort { get; set; }


    }
}
