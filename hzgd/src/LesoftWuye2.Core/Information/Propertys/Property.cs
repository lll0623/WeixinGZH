using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using LesoftWuye2.Core.LifeInfoTypes;
using LesoftWuye2.Core.PropertyCitys;
using LesoftWuye2.Core.PropertyTypes;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.Propertys
{
    [Table("Propertys")]
    [EntityDescription("项目")]
    public class Property : AuditedEntity<long>
    {
        public const int MaxTitleLength = 20;
        public const int MaxThumbnailLength = 128;

        [Display(Name = "名称")]
        [Required]
        [MaxLength(MaxTitleLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Title { get; set; }

        [Display(Name = "排序号")]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual int Sort { get; set; }

        [Display(Name = "缩略图")]
        [MaxLength(MaxThumbnailLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Thumbnail { get; set; }

        [Display(Name = "不区分小区")]
        public bool AllProjects { get; set; }

        public long PropertyCityId { get; set; }

        [ForeignKey("PropertyCityId")]
        public virtual PropertyCity City { get; set; }

        public long PropertyTypeId { get; set; }

        [ForeignKey("PropertyTypeId")]
        public virtual PropertyType Type { get; set; }
    }
}
