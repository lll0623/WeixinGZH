using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesoftWuye2.Application.Weixin.Dto
{
    /// <summary>
    /// 首页数据
    /// </summary>
    public class HomeDataDto
    {
        /// <summary>
        /// 轮播图
        /// </summary>
        public List<string> SlideImages { get; set; }

        /// <summary>
        /// 最新一条公告
        /// </summary>
        public ArticleItem Notice { get; set; }

        /// <summary>
        /// 最新一条活动
        /// </summary>
        public ArticleItem Activity { get; set; }

        /// <summary>
        /// 最新一条新闻
        /// </summary>
        public ArticleItem News { get; set; }

        /// <summary>
        /// 首页下方文章项目
        /// </summary>
        public class ArticleItem
        {
            public long Id { get; set; }

            public string Title { get; set; }

            public string Thumbnail { get; set; }

            public DateTime CreationTime { get; set; }

        }

        public string DefaultRoom { get; set; }
    }
}
