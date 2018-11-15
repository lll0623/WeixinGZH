using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.RentsaleinfoTypes;



namespace LesoftWuye2.Application.RentsaleinfoTypes.Dto
{
    [AutoMap(typeof(RentsaleinfoType))]
    public class CreateRentsaleinfoTypeInput
    {

        [Display(Name = "名称")]
        [MaxLength(LesoftWuye2.Core.RentsaleinfoTypes.RentsaleinfoType.MaxNameLength)]
        [Required]
        public string Name { get; set; }

        

        [Display(Name = "排序号")]
        public int Sort { get; set; }


    }
}
