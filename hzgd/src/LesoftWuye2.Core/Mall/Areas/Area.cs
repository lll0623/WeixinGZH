using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.Areas
{
    [Table("Areas")]
    [EntityDescription("行政区域")]
    public class Area : Entity<long>
    {
        public long Parent_id { get; set; }
        public string Name { get; set; }
        public string Short_name { get; set; }
        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public int Level { get; set; }
        public int Sort { get; set; }

        public int Status { get; set; }
    }
}
