using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using LesoftWuye2.Core.Estateinfos;
using LesoftWuye2.Core.LifeInfos;

namespace LesoftWuye2.Application.Estateinfos.Dto
{
    
    public class EstateinfoMyInfoDto 
    {
        public int UnShow { get; set; }
        public int IsSale { get; set; }
        public int UnSale { get; set; }
    }
}
