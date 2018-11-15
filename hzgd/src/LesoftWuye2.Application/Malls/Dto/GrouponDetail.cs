using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesoftWuye2.Application.Malls.Dto
{
    public class GrouponDetail
    {
        public long Id { get; set; }
        public bool IsSale { get; set; }
        public List<string> SlideImages { get; set; }
        public decimal Price { get; set; }

        public long ProductId { get; set; }

        public decimal SalePrice { get; set; }

        public decimal MemberPrice { get; set; }

        public int SellCount { get; set; }

        public string Unit { get; set; }

        public string Name { get; set; }

        public string Summary { get; set; }

        public List<string> Tags { get; set; }

        public int RequireCount { get; set; }

        public int CommentCount { get; set; }

        public List<CommentItem> Comments { get; set; }

        public string Content { get; set; }

        public string Thumbnail { get; set; }

        public List<GrouponingItem> GrouponingItems { get; set; }

        public bool IsLike { get; set; }

        public class CommentItem
        {
            public string Name { get; set; }
            public DateTime CreationTime { get; set; }

            public string Thumbnail { get; set; }

            public string Content { get; set; }

            public string CreationTimeFormat => CreationTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public class GrouponingItem
        {
            public long Id { get; set; }

            public string Name { get; set; }

            public DateTime ExpireTime { get; set; }

            public string Thumbnail { get; set; }

            public int RequireCount { get; set; }

            public virtual string BindRooms { get; set; }

            public string LostTime {
                get
                {
                    TimeSpan span = ExpireTime - DateTime.Now;
                    return $"{span.Hours}:{span.Minutes}:{span.Seconds}";
                }
            }
        }

        public DateTime StartTime { get; set; }

        public DateTime ExpireTime { get; set; }

        public bool IsExpire =>  ExpireTime < DateTime.Now || DateTime.Now< StartTime;
    }
}
