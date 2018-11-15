using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using LesoftWuye2.Core.Categories;
using LesoftWuye2.Core.Wuyebase.Members;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.MemberAddresss
{
    [Table("MemberAddresss")]
    [EntityDescription("收货地址")]
    public class MemberAddress : AuditedEntity<long>
    {
        public const int MaxAddressLength = 255;
        public const int MaxContactLength = 20;
        public const int MaxMobileLength = 11;

        public long MemberId { get; set; }

        [ForeignKey("MemberId")]

        public virtual Member Member { get; set; }


        public virtual bool IsDefault { get; set; }

        public virtual long ProvinceId { get; set; }

        public virtual long CityId { get; set; }

        public virtual long DistrictId { get; set; }


        [Display(Name = "详细地址")]
        [Required]
        [MaxLength(MaxAddressLength)]
        public virtual string Address { get; set; }

        [Required]
        [MaxLength(MaxMobileLength)]
        public virtual string Mobile { get; set; }

        [Required]
        [MaxLength(MaxContactLength)]
        public virtual string Contact { get; set; }
    }
}
