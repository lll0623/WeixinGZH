using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using LesoftWuye2.Core.ForumPosts;
using LesoftWuye2.Core.Plates;
using LesoftWuye2.Core.Wuyebase.Members;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.ForumImages
{
    [Table("ForumImages")]
    [EntityDescription("帖子图片")]
    public class ForumImage : AuditedEntity<long>
    {

        public const int MaxImageLength = 128;


        public long OwnerId { get; set; }

        public ForumImageType Type { get; set; }


        [Required]
        [MaxLength(MaxImageLength)]

        public string Image { get; set; }


    }
}
