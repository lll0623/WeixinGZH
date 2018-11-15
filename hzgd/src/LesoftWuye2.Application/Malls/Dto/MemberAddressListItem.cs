using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using LesoftWuye2.Core.Areas;
using LesoftWuye2.Core.Groupons;
using LesoftWuye2.Core.MemberAddresss;
using LesoftWuye2.Core.Wuyebase.Members;

namespace LesoftWuye2.Application.Malls.Dto
{
    [AutoMapFrom(typeof(MemberAddress))]
    public class MemberAddressListItem
    {
        public virtual long Id { get; set; }

        public virtual bool IsDefault { get; set; }

        public virtual long ProvinceId { get; set; }

        public virtual string ProvinceName { get; set; }

        public virtual long CityId { get; set; }

        public virtual string CityName { get; set; }

        public virtual long DistrictId { get; set; }

        public virtual string DistrictName { get; set; }

        public virtual string Address { get; set; }

        public virtual string Mobile { get; set; }

        public virtual string Contact { get; set; }
    }

    public class MemberAddressAndAera
    {
        public MemberAddress MemberAddress { get; set; }
        public Area Province { get; set; }
        public Area City { get; set; }
        public Area District { get; set; }

    }
}
