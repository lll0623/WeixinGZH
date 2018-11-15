using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.Estateinfos;
using Abp.Application.Services.Dto;


namespace LesoftWuye2.Application.Estateinfos.Dto
{
    [AutoMapFrom(typeof(Estateinfo))]
    public class EstateinfoListDto : EntityDto<long>
    {

        [Display(Name = "发布人")]
        [Required]
        public virtual string MemberId { get; set; }

        public virtual EstateinfoMode Mode { get; set; }

        [Display(Name = "标题")]
        [Required]
        [MaxLength(LesoftWuye2.Core.Estateinfos.Estateinfo.MaxTitleLength)]
        public virtual string Title { get; set; }

        [Display(Name = "联系人")]
        [Required]
        [MaxLength(Estateinfo.MaxContactLength)]
        public virtual string Contact { get; set; }

        [Display(Name = "联系电话")]
        [Required]
        [MaxLength(Estateinfo.MaxMobileLength)]
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

        public string ModeName
        {
            get
            {
                switch (Mode)
                {
                    case EstateinfoMode.Rent:
                        return "出租";
                    case EstateinfoMode.ForRent:
                        return "求租";
                    case EstateinfoMode.Purchase:
                        return "买卖";
                    case EstateinfoMode.Transfer:
                        return "转让";
                    default:
                        return "未知";
                }
            }
        }
    }
}
