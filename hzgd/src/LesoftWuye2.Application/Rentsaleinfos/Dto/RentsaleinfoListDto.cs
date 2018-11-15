using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.Rentsaleinfos;
using Abp.Application.Services.Dto;


namespace LesoftWuye2.Application.Rentsaleinfos.Dto
{
    [AutoMapFrom(typeof(Rentsaleinfo))]
    public class RentsaleinfoListDto : EntityDto<long>
    {

        [Display(Name = "发布人")]
        [Required]
        public virtual string MemberId { get; set; }


        [Display(Name = "标题")]
        [Required]
        [MaxLength(LesoftWuye2.Core.Rentsaleinfos.Rentsaleinfo.MaxTitleLength)]
        public virtual string Title { get; set; }

        [Display(Name = "联系人")]
        [Required]
        [MaxLength(Rentsaleinfo.MaxContactLength)]
        public virtual string Contact { get; set; }

        [Display(Name = "联系电话")]
        [Required]
        [MaxLength(Rentsaleinfo.MaxMobileLength)]
        public virtual string Mobile { get; set; }

        [Display(Name = "是否显示")]
        public virtual bool IsShow { get; set; }

        [Display(Name = "是否上架")]
        public virtual bool IsSale { get; set; }

        [Display(Name = "缩略图")]
        public virtual string Thumbnail { get; set; }

        [Display(Name = "简介")]
        public virtual string Summary { get; set; }

        public virtual string Type_Name { get; set; }

        public DateTime CreationTime { get; set; }

        public RentsaleinfoSource Source { get; set; }

        public string SourceDesc {
            get
            {
                switch (Source)
                {
                    case RentsaleinfoSource.Admin:
                        return "物业";
                    case RentsaleinfoSource.User:
                        return "业主";
                    default:
                        return "";
                }
            }
        }
    }
}
