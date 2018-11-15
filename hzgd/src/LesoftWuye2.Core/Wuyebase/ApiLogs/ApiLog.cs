using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.ApiLogs
{
    [Table("ApiLogs")]
    [EntityDescription("API调用纪录")]
    public class ApiLog : CreationAuditedEntity<long>
    {
        public const int MaxNameLength = 50;
        public const int MaxDescriptionLength = 500;
        public const int MaxRequestLength = 65536;
        public const int MaxResponseLength = 65536;


        [Display(Name = "接口名称")]
        [Required]
        [MaxLength(MaxNameLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Name { get; set; }

        [Display(Name = "接口说明")]
        [MaxLength(MaxDescriptionLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Description { get; set; }

        [Display(Name = "请求参数")]
        [MaxLength(MaxRequestLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Request { get; set; }

        [Display(Name = "响应结果")]
        //[MaxLength(MaxResponseLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Response { get; set; }

        [Display(Name = "是否成功")]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual bool Success { get; set; }

    }
}
