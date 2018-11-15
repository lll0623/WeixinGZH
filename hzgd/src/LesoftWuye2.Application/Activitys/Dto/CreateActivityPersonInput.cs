using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.Activitys;



namespace LesoftWuye2.Application.Activitys.Dto
{
    [AutoMap(typeof(ActivityPerson))]
    public class CreateActivityPersonInput
    {

        public long ActivityId { get; set; }
        

        [Display(Name = "发布人")]
        public virtual string MemberId { get; set; }

        [Display(Name = "联系人")]
        [Required]
        [MaxLength(LesoftWuye2.Core.Activitys.ActivityPerson.MaxContactLength)]
        public virtual string Contact { get; set; }

        [Display(Name = "手机号码")]
        [Required]
        [MaxLength(LesoftWuye2.Core.Activitys.ActivityPerson.MaxMobileLength)]
        public virtual string Mobile { get; set; }

    }
}
