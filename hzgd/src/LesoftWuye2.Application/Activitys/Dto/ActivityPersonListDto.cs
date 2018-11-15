using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.Activitys;
using Abp.Application.Services.Dto;


namespace LesoftWuye2.Application.Activitys.Dto
{
    [AutoMapFrom(typeof(ActivityPerson))]
    public class ActivityPersonListDto : EntityDto<long>
    {

        [Display(Name = "联系人")]
        [Required]
        [MaxLength(LesoftWuye2.Core.Activitys.ActivityPerson.MaxContactLength)]
        public virtual string Contact { get; set; }

        [Display(Name = "手机号码")]
        [Required]
        [MaxLength(LesoftWuye2.Core.Activitys.ActivityPerson.MaxMobileLength)]
        public virtual string Mobile { get; set; }

        public DateTime CreationTime { get; set; }
        

    }
}
