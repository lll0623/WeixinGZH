using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.ApiLogs;

namespace LesoftWuye2.Application.ApiLogs.Dto
{
    [AutoMap(typeof(ApiLog))]
    public class CreateApiLogInput
    {

        [Display(Name = "接口名称")]
        [MaxLength(ApiLog.MaxNameLength)]
        [Required]
        public string Name { get; set; }

        [Display(Name = "接口说明")]
        [MaxLength(ApiLog.MaxDescriptionLength)]
        public string Description { get; set; }

        [Display(Name = "请求参数")]
        [MaxLength(ApiLog.MaxRequestLength)]
        public string Request { get; set; }

        [Display(Name = "响应结果")]
        [MaxLength(ApiLog.MaxResponseLength)]
        public string Response { get; set; }

        [Display(Name = "是否成功")]
        public bool Success { get; set; }


    }
}
