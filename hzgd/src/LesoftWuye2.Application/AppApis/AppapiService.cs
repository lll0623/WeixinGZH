using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.UI;
using Castle.Components.DictionaryAdapter;
using LesoftWuye2.Application.AppApis.Dto;
using LesoftWuye2.Application.Frontpages.Dto;
using LesoftWuye2.Application.SlideImages.Dto;
using LesoftWuye2.Core.ActivityProjects;
using LesoftWuye2.Core.Activitys;
using LesoftWuye2.Core.Configuration;
using LesoftWuye2.Core.CurrentRooms;
using LesoftWuye2.Core.NewsProjects;
using LesoftWuye2.Core.Newss;
using LesoftWuye2.Core.NoticeProjects;
using LesoftWuye2.Core.Notices;
using LesoftWuye2.Core.SlideImages;
using LesoftWuye2.Application.WuyeApis;
using LesoftWuye2.Core.Projects;

namespace LesoftWuye2.Application.AppApis
{
    public class AppapiService : LesoftWuye2AppServiceBase, IAppapiService
    {
        private readonly IRepository<SlideImage, long> _slideimageRepository;

        private readonly IRepository<Notice, long> _noticeRepository;
        private readonly IRepository<NoticeProject, long> _noticeProjectRepository;
        private readonly IRepository<Activity, long> _activityRepository;
        private readonly IRepository<ActivityPerson, long> _activityPersonRepository;
        private readonly IRepository<ActivityProject, long> _activityProjectRepository;
        private readonly IRepository<News, long> _newsRepository;
        private readonly IRepository<NewsProject, long> _newsProjectRepository;
            
        private readonly IRepository<CurrentRoom, long> _currentRoomRepository;
        readonly IWuyeApiAppSrvice _wuyeApiAppSrvice;
        private readonly IRepository<Project, long> _projectRepository;


        public AppapiService(
            IRepository<SlideImage, long> slideimageRepository,

            IRepository<Notice, long> noticeRepository,
            IRepository<NoticeProject, long> noticeProjectRepository,
            IRepository<Activity, long> activityRepository,
            IRepository<ActivityPerson, long> activityPersonRepository,
            IRepository<ActivityProject, long> activityProjectRepository,
            IRepository<News, long> newsRepository,
            IRepository<NewsProject, long> newsProjectRepository,
            IRepository<CurrentRoom, long> currentRoomRepository,
            IWuyeApiAppSrvice wuyeApiAppSrvice,
            IRepository<Project, long> projectRepository
            )
        {
            _slideimageRepository = slideimageRepository;

            _slideimageRepository = slideimageRepository;
            _noticeRepository = noticeRepository;
            _noticeProjectRepository = noticeProjectRepository;
            _activityRepository = activityRepository;
            _activityPersonRepository = activityPersonRepository;
            _activityProjectRepository = activityProjectRepository;
            _newsRepository = newsRepository;
            _newsProjectRepository = newsProjectRepository;
            _currentRoomRepository = currentRoomRepository;

            _wuyeApiAppSrvice = wuyeApiAppSrvice;
            _projectRepository = projectRepository;
        }


        public async Task<List<slideimage>> GetSlideImage()
        {
            string host = SettingManager.GetSettingValue(AppSettings.Wuye.Host);
            var query = _slideimageRepository.GetAll();
            var list = await query.OrderByDescending(t => t.Sort).ToListAsync();
            List<slideimage> slideimages = new EditableList<slideimage>();
            slideimages.AddRange(list.Select(slideImage => new slideimage()
            {
                imgUrl = host + slideImage.Thumbnail + "?width=640&height=300&mode=crop"
            }));

            return slideimages;
        }

        public void SetCurrentRoomInfo(SetCurrentRoomInfo info)
        {

            var old = _currentRoomRepository.GetAll().FirstOrDefault(t => t.UserId == info.userid);
            if (old != null)
            {
                old.Code = info.code;
                old.Name = info.name;
                _currentRoomRepository.Update(old);
            }
            else
            {
                CurrentRoom room = new CurrentRoom();
                room.UserId = info.userid;
                room.Name = info.name;
                room.Code = info.code;
                _currentRoomRepository.Insert(room);
            }

        }

        public CurrentRoomInfo GetCurrentRoomInfo(string Upk)
        {
            //throw new UserFriendlyException("暂无绑定");

            var room = _currentRoomRepository.GetAll().FirstOrDefault(t => t.UserId == Upk);
            if (room == null)
                return new CurrentRoomInfo() { address = "无", code = "" };
            else
            {
                return new CurrentRoomInfo() { address = room.Name, code = room.Code };
            }
        }

        public async Task<List<HomeNews>> GetHomeNews(string Upk)
        {
            long ProjectId = 0;
            try
            {
                var result = _wuyeApiAppSrvice.GetMemberInfoByAPPUserID(Upk);
                if (result.Success)
                {
                    var project = _projectRepository.FirstOrDefault(t => t.Code == result.PProjectCode);
                    if (project != null)
                        ProjectId = project.Id;
                }
            }
            catch (Exception ex)
            {

            }


            HomeDataDto homeData = new HomeDataDto();
            var slideImages = await _slideimageRepository.GetAll().OrderByDescending(t => t.Sort).ToListAsync();
            homeData.SlideImages = slideImages.Select(t => t.Thumbnail).ToList();

            var topNotice = _noticeRepository
                 .GetAll().GroupJoin(_noticeProjectRepository.GetAll(), left => left.Id, right => right.NoticeId,
                     (left, right) => new
                     {
                         Notice = left,
                         NoticeProjects = right
                     }).OrderByDescending(t => t.Notice.CreationTime).FirstOrDefault(t => t.Notice.AllProjects || t.NoticeProjects.FirstOrDefault().ProjectId == ProjectId);
            if (topNotice != null)
            {
                homeData.Notice = new HomeDataDto.ArticleItem() { Id = topNotice.Notice.Id, Title = topNotice.Notice.Title, Thumbnail = topNotice.Notice.Thumbnail, CreationTime = topNotice.Notice.CreationTime };
            }

            var topActivity = _activityRepository
                .GetAll().GroupJoin(_activityProjectRepository.GetAll(), left => left.Id, right => right.ActivityId,
                    (left, right) => new
                    {
                        Activity = left,
                        ActivityProjects = right
                    }).OrderByDescending(t => t.Activity.CreationTime).FirstOrDefault(t => t.Activity.AllProjects || t.ActivityProjects.FirstOrDefault().ProjectId == ProjectId);
            if (topActivity != null)
            {
                homeData.Activity = new HomeDataDto.ArticleItem() { Id = topActivity.Activity.Id, Title = topActivity.Activity.Title, Thumbnail = topActivity.Activity.Thumbnail, CreationTime = topActivity.Activity.CreationTime };
            }

            var topNews = _newsRepository
              .GetAll().GroupJoin(_newsProjectRepository.GetAll(), left => left.Id, right => right.NewsId,
                  (left, right) => new
                  {
                      News = left,
                      NewsProjects = right
                  }).OrderByDescending(t => t.News.CreationTime).FirstOrDefault(t => t.News.AllProjects || t.NewsProjects.FirstOrDefault().ProjectId == ProjectId);
            if (topNews != null)
            {
                homeData.News = new HomeDataDto.ArticleItem() { Id = topNews.News.Id, Title = topNews.News.Title, Thumbnail = topNews.News.Thumbnail, CreationTime = topNews.News.CreationTime };
            }

            string host = SettingManager.GetSettingValue(AppSettings.Wuye.Host);
            List<HomeNews> homeNewses = new EditableList<HomeNews>();

            if (homeData.Notice != null)
            {
                homeNewses.Add(new HomeNews()
                {
                    title = "社区公告",
                    content = homeData.Notice.Title,
                    contentUrl = host + "/Maxwon/Notice_View/" + homeData.Notice.Id,
                    dateStr = homeData.Notice.CreationTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    iconUrl = host + homeData.Notice.Thumbnail + "?width=160&height=100&mode=crop",
                    moreUrl = host + "/Maxwon/Notice_List"
                });
            }


            if (homeData.Activity != null)
            {
                homeNewses.Add(new HomeNews()
                {
                    title = "社区活动",
                    content = homeData.Activity.Title,
                    contentUrl = host + "/Maxwon/Activity_View/" + homeData.Activity.Id,
                    dateStr = homeData.Activity.CreationTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    iconUrl = host + homeData.Activity.Thumbnail + "?width=160&height=100&mode=crop",
                    moreUrl = host + "/Maxwon/Activity_List"
                });
            }

            if (homeData.News != null)
            {
                homeNewses.Add(new HomeNews()
                {
                    title = "社区资讯",
                    content = homeData.News.Title,
                    contentUrl = host + "/Maxwon/News_View/" + homeData.News.Id,
                    dateStr = homeData.News.CreationTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    iconUrl = host + homeData.News.Thumbnail + "?width=160&height=100&mode=crop",
                    moreUrl = host + "/Maxwon/News_List"
                });
            }

            return homeNewses;
        }

        public List<NavigationItem> GetNavigationItems()
        {
            string host = SettingManager.GetSettingValue(AppSettings.Wuye.Host);
            List<NavigationItem> navigationItems = new EditableList<NavigationItem>();
            navigationItems.Add(new NavigationItem() { key = "BDFC", name = "绑定房产", url = host + "/Maxwon/Room_List" });
            navigationItems.Add(new NavigationItem() { key = "XXLB", name = "消息列表", url = host + "/Maxwon/Message_List" });
            navigationItems.Add(new NavigationItem() { key = "BXCL", name = "报修处理", url = host + "/Maxwon/Repair_Add" });
            navigationItems.Add(new NavigationItem() { key = "TSCL", name = "投诉处理", url = host + "/Maxwon/Complaint_Add" });
            navigationItems.Add(new NavigationItem() { key = "CFJF", name = "查费缴费", url = host + "/Maxwon/Fee_NoPay" });
            navigationItems.Add(new NavigationItem() { key = "BYDZ", name = "表扬点赞", url = host + "/Maxwon/Like_Add" });
            navigationItems.Add(new NavigationItem() { key = "ZJWM", name = "走进我们", url = host + "/Maxwon/Substation_List" });
            navigationItems.Add(new NavigationItem() { key = "WDSW", name = "我的事务", url = host + "/Maxwon/My_Count" });
            navigationItems.Add(new NavigationItem() { key = "LGDH", name = "楼管电话", url = host + "/Maxwon/ServiceTel_List" });
            navigationItems.Add(new NavigationItem() { key = "PTSC", name = "拼团商城", url = host + "/Mall/Index" });
            navigationItems.Add(new NavigationItem() { key = "YZLT", name = "业主论坛", url = host + "/Forum/Index" });

            navigationItems.Add(new NavigationItem() { key = "ORDER_MORE", name = "我的订单", url = host + "/Mall/OrderList" });
            navigationItems.Add(new NavigationItem() { key = "ORDER_PENDING_PAY", name = "我的订单_待收款", url = host + "/Mall/OrderList?type=1" });
            navigationItems.Add(new NavigationItem() { key = "ORDER_PENDING_GROUP", name = "我的订单_待成团", url = host + "/Mall/OrderList?type=2" });
            navigationItems.Add(new NavigationItem() { key = "ORDER_PENDING_SHIPPED", name = "我的订单_待发货", url = host + "/Mall/OrderList?type=3" });
            navigationItems.Add(new NavigationItem() { key = "ORDER_PENDING_RECEIPT", name = "我的订单_待收货", url = host + "/Mall/OrderList?type=4" });
            navigationItems.Add(new NavigationItem() { key = "ORDER_PENDING_COMMENT", name = "我的订单_待评价", url = host + "/Mall/OrderList?type=5" });

            navigationItems.Add(new NavigationItem() { key = "MY_FLEA", name = "我的跳蚤", url = host + "/Maxwon/Estateinfo_My?type=0" });
            navigationItems.Add(new NavigationItem() { key = "MY_GROUP", name = "我的拼团", url = host + "/Mall/GrouponOrderList" });
            navigationItems.Add(new NavigationItem() { key = "MY_HOUSE_RENTAL", name = "我的租售", url = host + "/Maxwon/Rentsaleinfo_My?type=0" });
            navigationItems.Add(new NavigationItem() { key = "MY_ADDRESS", name = "收货地址", url = host + "/Mall/AddressList" });
            navigationItems.Add(new NavigationItem() { key = "MY_ACTIVITY", name = "我的活动", url = host + "/Maxwon/Activity_My" });
            navigationItems.Add(new NavigationItem() { key = "MY_BBS", name = "我的帖子", url = host + "/Forum/MyPost" });


            return navigationItems;
        }
    }
}
