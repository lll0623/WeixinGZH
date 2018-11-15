using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.RentsaleinfoTypes;
using Abp.Application.Services.Dto;


namespace LesoftWuye2.Application.RentsaleinfoTypes.Dto
{
    [AutoMapFrom(typeof(RentsaleinfoType))]
    public class RentsaleinfoTypeItemDto : EntityDto<long>
    {

        [Display(Name = "名称")]
        public string Name { get; set; }



        [Display(Name = "排序号")]
        public int Sort { get; set; }


    }
}
