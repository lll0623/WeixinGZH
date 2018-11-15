using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using LesoftWuye2.Core.ApiLogs;

namespace LesoftWuye2.Application.ApiLogs.Dto
{
    [AutoMapFrom(typeof(ApiLog))]
    public class ApiLogListDto : EntityDto<long>
    {

        [Display(Name = "接口名称")]
        public string Name { get; set; }

        [Display(Name = "接口说明")]
        public string Description { get; set; }

        [Display(Name = "请求参数")]
        public string Request { get; set; }

        [Display(Name = "响应结果")]
        public string Response { get; set; }

        [Display(Name = "是否成功")]
        public bool Success { get; set; }

        public DateTime CreationTime { get; set; }


    }
}
