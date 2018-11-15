using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using LesoftWuye2.Application.Activitys.Dto;
using LesoftWuye2.Application.Estateinfos.Dto;
using LesoftWuye2.Application.FeeServices.Dto;
using LesoftWuye2.Application.Groupons;
using LesoftWuye2.Application.Weixin.Dto;
using LesoftWuye2.Application.Weixin.Session;
using LesoftWuye2.Application.LifeInfos.Dto;
using LesoftWuye2.Application.LifeInfoTypes.Dto;
using LesoftWuye2.Application.Malls.Dto;
using LesoftWuye2.Application.MallSlideImages.Dto;
using LesoftWuye2.Application.Newss.Dto;
using LesoftWuye2.Application.Notices.Dto;
using LesoftWuye2.Application.Rentsaleinfos.Dto;
using LesoftWuye2.Application.ServiceTels.Dto;
using LesoftWuye2.Application.Substations.Dto;
using LesoftWuye2.Application.Utils;
using LesoftWuye2.Application.WuyeApis;
using LesoftWuye2.Application.WuyeApis.Dto;
using LesoftWuye2.Application.YsPay;
using LesoftWuye2.Core.ActivityProjects;
using LesoftWuye2.Core.Activitys;
using LesoftWuye2.Core.Areas;
using LesoftWuye2.Core.Categories;
using LesoftWuye2.Core.Details;
using LesoftWuye2.Core.Estateinfos;
using LesoftWuye2.Core.EstateinfoTypes;
using LesoftWuye2.Core.FeeServiceProjects;
using LesoftWuye2.Core.FeeServices;
using LesoftWuye2.Core.Groupons;
using LesoftWuye2.Core.LifeInfoProjects;
using LesoftWuye2.Core.LifeInfos;
using LesoftWuye2.Core.LifeInfoTypes;
using LesoftWuye2.Core.Mall.Orders;
using LesoftWuye2.Core.MallSlideImages;
using LesoftWuye2.Core.MemberAddresss;
using LesoftWuye2.Core.NewsProjects;
using LesoftWuye2.Core.Newss;
using LesoftWuye2.Core.NoticeProjects;
using LesoftWuye2.Core.Notices;
using LesoftWuye2.Core.Orders;
using LesoftWuye2.Core.Products;
using LesoftWuye2.Core.Projects;
using LesoftWuye2.Core.Rentsaleinfos;
using LesoftWuye2.Core.RentsaleinfoTypes;
using LesoftWuye2.Core.ServiceTels;
using LesoftWuye2.Core.SlideImages;
using LesoftWuye2.Core.SqlExecuters;
using LesoftWuye2.Core.Substations;
using LesoftWuye2.Core.Utils;
using LesoftWuye2.Core.Wuyebase.Members;
using Obs;
using Obs.Dto;
using Obs.Filter;
using Abp.Domain.Uow;
using Abp.Events.Bus;
using LesoftWuye2.Application.Weixin.Events.EventDatas;
using LesoftWuye2.Core.Mall.RefundOrders;
using LesoftWuye2.Core.RefundOrders;

namespace LesoftWuye2.Application.Weixin
{
    public class WeixinService : LesoftWuye2AppServiceBase, IWeixinService
    {
        private readonly IWeixinSession _weixinSession;
        private readonly IRepository<SlideImage, long> _slideimageRepository;
        private readonly IRepository<Notice, long> _noticeRepository;
        private readonly IRepository<NoticeProject, long> _noticeProjectRepository;
        private readonly IRepository<Activity, long> _activityRepository;
        private readonly IRepository<ActivityPerson, long> _activityPersonRepository;
        private readonly IRepository<ActivityProject, long> _activityProjectRepository;
        private readonly IRepository<News, long> _newsRepository;
        private readonly IRepository<NewsProject, long> _newsProjectRepository;
        private readonly IDetailManager _detailManager;

        readonly IRepository<ServiceTel, long> _serviceTelRepository;
        readonly IRepository<LifeInfoType, long> _lifeInfoTypeRepository;
        readonly IRepository<LifeInfo, long> _lifeInfoRepository;
        readonly IRepository<LifeInfoProject, long> _lifeInfoProjectRepository;
        private readonly IRepository<Substation, long> _substationRepository;

        private readonly IRepository<EstateinfoType, long> _estateinfotypeRepository;
        private readonly IRepository<Estateinfo, long> _estateinfoRepository;
        private readonly IRepository<EstateinfoImage, long> _estateinfoImageRepository;
        private readonly IRepository<EstateinfoComment, long> _estateinfoCommentRepository;


        private readonly IRepository<RentsaleinfoType, long> _rentsaleinfotypeRepository;
        private readonly IRepository<Rentsaleinfo, long> _rentsaleinfoRepository;
        private readonly IRepository<RentsaleinfoImage, long> _rentsaleinfoImageRepository;
        private readonly IWuyeApiAppSrvice _wuyeApiAppSrvice;

        private readonly IRepository<Member, long> _memberRepository;
        private readonly IAppFolders _appFolders;

        private readonly IRepository<Groupon, long> _grouponRepository;
        private readonly IRepository<Product, long> _productRepository;
        private readonly IRepository<Category, long> _categoryRepository;
        private readonly IRepository<ProductSlideImage, long> _productSlideimageRepository;
        private readonly IRepository<ProductComment, long> _productCommentRepository;

        private readonly IRepository<MallSlideImage, long> _mallslideimageRepository;
        private readonly IRepository<MemberAddress, long> _memberAddressRepository;
        private readonly IRepository<Area, long> _areaRepository;
        private readonly IRepository<Order, long> _orderRepository;
        private readonly IRepository<OrderPay, long> _orderPayRepository;
        private readonly IRepository<OrderProduct, long> _orderProductRepository;
        private readonly IRepository<GrouponOrder, long> _groupOrderRepository;
        private readonly IRepository<GrouponMember, long> _groupMemberRepository;
        private readonly IRepository<OrderShip, long> _orderShipRepository;
        private readonly IRepository<ProductLike, long> _productLikeRepository;
        private readonly IRepository<FeeService, long> _feeServiceRepository;
        private readonly IRepository<FeeServiceProject, long> _feeServiceProjectRepository;
        private readonly IRepository<Project, long> _projectRepository;

        private readonly IRepository<RefundOrder, long> _refundOrderRepository;
        private readonly IRepository<RefundOrderImage, long> _refundOrderImageRepository;

        private readonly ISqlExecuter _sqlExecuter;
        private readonly WeiXinApi _weiXinApi;
        private readonly TemplateMessageUtils _templateMessageUtils;
        public IEventBus EventBus { get; set; }

        public WeixinService(IWeixinSession weixinSession,
            IRepository<SlideImage, long> slideimageRepository,
            IRepository<Notice, long> noticeRepository,
            IRepository<NoticeProject, long> noticeProjectRepository,
            IRepository<Activity, long> activityRepository,
            IRepository<ActivityPerson, long> activityPersonRepository,
            IRepository<ActivityProject, long> activityProjectRepository,
            IRepository<News, long> newsRepository,
            IRepository<NewsProject, long> newsProjectRepository,
            IDetailManager detailManager,

            IRepository<ServiceTel, long> serviceTelRepository,
            IRepository<LifeInfoType, long> lifeInfoTypeRepository,
            IRepository<LifeInfo, long> lifeInfoRepository,
            IRepository<LifeInfoProject, long> lifeInfoProjectRepository,
            IRepository<Substation, long> substationRepository,

            IRepository<EstateinfoType, long> estateinfotypeRepository,
            IRepository<Estateinfo, long> estateinfoRepository,
            IRepository<EstateinfoImage, long> estateinfoImageRepository,
            IRepository<EstateinfoComment, long> estateinfoCommentRepository,

            IRepository<RentsaleinfoType, long> rentsaleinfotypeRepository,
            IRepository<Rentsaleinfo, long> rentsaleinfoRepository,
            IRepository<RentsaleinfoImage, long> rentsaleinfoImageRepository,
            IWuyeApiAppSrvice wuyeApiAppSrvice,

             IRepository<Member, long> memberRepository,
            IAppFolders appFolders,

             IRepository<Groupon, long> grouponRepository,
            IRepository<Product, long> productRepository,
            IRepository<Category, long> categoryRepository,
            IRepository<ProductSlideImage, long> productSlideimageRepository,
             IRepository<ProductComment, long> productCommentRepository,
            IRepository<MallSlideImage, long> mallslideimageRepository,
            IRepository<MemberAddress, long> memberAddressRepository,
            IRepository<Area, long> areaRepository,
             IRepository<Order, long> orderRepository,
             IRepository<OrderPay, long> orderPayRepository,
             IRepository<OrderProduct, long> orderProductRepository,
             IRepository<GrouponOrder, long> groupOrderRepository,
             IRepository<GrouponMember, long> groupMemberRepository,
             IRepository<OrderShip, long> orderShipRepository,
             IRepository<ProductLike, long> productLikeRepository,
             IRepository<FeeService, long> feeServiceRepository,
             IRepository<FeeServiceProject, long> feeServiceProjectRepository,
             IRepository<Project, long> projectRepository,
            IRepository<RefundOrder, long> refundOrderRepository,
              IRepository<RefundOrderImage, long> refundOrderImageRepository,
              ISqlExecuter sqlExecuter,
            WeiXinApi weiXinApi,
            TemplateMessageUtils templateMessageUtils
            )
        {
            _weixinSession = weixinSession;
            _slideimageRepository = slideimageRepository;
            _noticeRepository = noticeRepository;
            _noticeProjectRepository = noticeProjectRepository;
            _activityRepository = activityRepository;
            _activityPersonRepository = activityPersonRepository;
            _activityProjectRepository = activityProjectRepository;
            _newsRepository = newsRepository;
            _newsProjectRepository = newsProjectRepository;
            _detailManager = detailManager;

            _serviceTelRepository = serviceTelRepository;
            _lifeInfoTypeRepository = lifeInfoTypeRepository;
            _lifeInfoRepository = lifeInfoRepository;
            _lifeInfoProjectRepository = lifeInfoProjectRepository;
            _substationRepository = substationRepository;

            _estateinfotypeRepository = estateinfotypeRepository;
            _estateinfoRepository = estateinfoRepository;
            _estateinfoImageRepository = estateinfoImageRepository;
            _estateinfoCommentRepository = estateinfoCommentRepository;

            _rentsaleinfotypeRepository = rentsaleinfotypeRepository;
            _rentsaleinfoRepository = rentsaleinfoRepository;
            _rentsaleinfoImageRepository = rentsaleinfoImageRepository;

            _wuyeApiAppSrvice = wuyeApiAppSrvice;

            _memberRepository = memberRepository;
            _appFolders = appFolders;

            _grouponRepository = grouponRepository;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _productSlideimageRepository = productSlideimageRepository;
            _productCommentRepository = productCommentRepository;
            _mallslideimageRepository = mallslideimageRepository;
            _memberAddressRepository = memberAddressRepository;
            _areaRepository = areaRepository;
            _orderRepository = orderRepository;
            _orderPayRepository = orderPayRepository;
            _orderProductRepository = orderProductRepository;
            _groupOrderRepository = groupOrderRepository;
            _groupMemberRepository = groupMemberRepository;
            _orderShipRepository = orderShipRepository;

            _productLikeRepository = productLikeRepository;
            _feeServiceRepository = feeServiceRepository;
            _feeServiceProjectRepository = feeServiceProjectRepository;
            _projectRepository = projectRepository;

            _refundOrderRepository = refundOrderRepository;
            _refundOrderImageRepository = refundOrderImageRepository;

            _sqlExecuter = sqlExecuter;
            _weiXinApi = weiXinApi;
            _templateMessageUtils = templateMessageUtils;

            EventBus = NullEventBus.Instance;
        }

        public virtual async Task<HomeDataDto> GetHomeData()
        {
            HomeDataDto homeData = new HomeDataDto();
            var slideImages = await _slideimageRepository.GetAll().OrderByDescending(t => t.Sort).ToListAsync();
            homeData.SlideImages = slideImages.Select(t => t.Thumbnail).ToList();

            var topNotice = _noticeRepository
                 .GetAll().GroupJoin(_noticeProjectRepository.GetAll(), left => left.Id, right => right.NoticeId,
                     (left, right) => new
                     {
                         Notice = left,
                         NoticeProjects = right
                     }).OrderByDescending(t => t.Notice.CreationTime).FirstOrDefault(t => t.Notice.AllProjects || t.NoticeProjects.FirstOrDefault().ProjectId == _weixinSession.ProjectId);
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
                    }).OrderByDescending(t => t.Activity.CreationTime).FirstOrDefault(t => t.Activity.AllProjects || t.ActivityProjects.FirstOrDefault().ProjectId == _weixinSession.ProjectId);
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
                  }).OrderByDescending(t => t.News.CreationTime).FirstOrDefault(t => t.News.AllProjects || t.NewsProjects.FirstOrDefault().ProjectId == _weixinSession.ProjectId);
            if (topNews != null)
            {
                homeData.News = new HomeDataDto.ArticleItem() { Id = topNews.News.Id, Title = topNews.News.Title, Thumbnail = topNews.News.Thumbnail, CreationTime = topNews.News.CreationTime };
            }

            var userInfo = _wuyeApiAppSrvice.GetMemberInfoByAPPUserID(_weixinSession.MemberId.ToString());
            //更新到数据库

            if (userInfo.Success)
            {
                homeData.DefaultRoom = string.IsNullOrEmpty(userInfo.PRoomFullName) ? "暂无绑定房产" : userInfo.PRoomFullName;
                var memberInfo = _memberRepository.FirstOrDefault(t => t.Id == _weixinSession.MemberId);
                if (memberInfo != null)
                {
                    memberInfo.BindRooms = userInfo.PProjectCode;
                    memberInfo.PRoomFullName = userInfo.PRoomFullName;
                    memberInfo.ProjectCode = userInfo.PProjectCode;
                    await _memberRepository.UpdateAsync(memberInfo);
                }

            }
            else
            {
                homeData.DefaultRoom = "暂无绑定房产";
            }


            return homeData;
        }

        public bool Login(string username, string password)
        {
            string openid = _weixinSession.OpenId;
            if (string.IsNullOrEmpty(openid))
            {
                throw new UserFriendlyException("请从微信客户端登录.");
            }
            password = MD5Helper.EnCode(password).ToLower();
            var result = _wuyeApiAppSrvice.Login(username, password);
            if (!result.Success)
            {
                throw new UserFriendlyException(result.msg);
            }
            if (result.infos == null)
            {
                throw new UserFriendlyException("从物业系统获取用户信息失败.");
            }

            var userInfo = _wuyeApiAppSrvice.GetUserInfo(result.infos.Id);
            if (userInfo.infos == null || userInfo.infos.Count == 0)
            {
                throw new UserFriendlyException("从物业系统获取用户信息失败.");
            }

            var appUserInfo = _wuyeApiAppSrvice.GetMemberInfoByAPPUserID(result.infos.Id);
            if (!appUserInfo.Success)
            {
                throw new UserFriendlyException("从物业系统获取用户信息失败.");
            }
            long memberId = long.Parse(result.infos.Id);
            var memberInfo = _memberRepository.FirstOrDefault(t => t.Id == memberId);

            //一个openid只能绑定一个账号
            var openidWithMember = _memberRepository.FirstOrDefault(t => t.Openid == openid);
            if (memberInfo == null)
            {
                if (openidWithMember != null)
                    throw new UserFriendlyException("该微信已经绑定账号，不能绑定多个账号!");

                Member member = new Member();
                member.Id = long.Parse(result.infos.Id);
                member.MemberId = result.infos.Id;
                member.Openid = openid;
                member.Name = userInfo.infos[0].NickName;
                member.BindRooms = userInfo.infos[0].PProjectName;
                member.PRoomFullName = userInfo.infos[0].PRoomFullName;
                member.ProjectCode = appUserInfo.PProjectCode;
                member.ThumbnailBase64 = userInfo.infos[0].HeadImage;
                if (!string.IsNullOrEmpty(member.ThumbnailBase64))
                {
                    try
                    {
                        string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
                        var filePath = Path.Combine(_appFolders.UploadImageFolder, fileName);

                        MemoryStream stream = new MemoryStream(Convert.FromBase64String(userInfo.infos[0].HeadImage));
                        Bitmap img = new Bitmap(stream);
                        img.Save(filePath);
                        member.Thumbnail = "/Upload/Images/" + fileName;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else
                {
                    member.Thumbnail = "/Content/frontpage/i/default-headimg.jpg";
                }
                _memberRepository.Insert(member);
            }
            else
            {
                if (openidWithMember != null && openidWithMember.Id != memberInfo.Id)
                    throw new UserFriendlyException("该微信已经绑定账号，不能绑定多个账号!");

                memberInfo.Openid = openid;
                memberInfo.Name = userInfo.infos[0].NickName;
                memberInfo.BindRooms = userInfo.infos[0].PProjectName;
                memberInfo.PRoomFullName = userInfo.infos[0].PRoomFullName;
                memberInfo.ProjectCode = appUserInfo.PProjectCode;

                if (memberInfo.ThumbnailBase64 != userInfo.infos[0].HeadImage)
                {
                    //base64转图片
                    try
                    {
                        string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
                        var filePath = Path.Combine(_appFolders.UploadImageFolder, fileName);
                        MemoryStream stream = new MemoryStream(Convert.FromBase64String(userInfo.infos[0].HeadImage));
                        Bitmap img = new Bitmap(stream);
                        img.Save(filePath);
                        memberInfo.Thumbnail = "/Upload/Images/" + fileName;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                memberInfo.ThumbnailBase64 = userInfo.infos[0].HeadImage;
                _memberRepository.Update(memberInfo);
            }

            return true;
        }

        public void Logout()
        {
            if (_weixinSession.MemberId != 0)
            {
                var memberInfo = _memberRepository.FirstOrDefault(t => t.Id == _weixinSession.MemberId);
                if (memberInfo != null)
                {
                    memberInfo.Openid = null;
                    _memberRepository.Update(memberInfo);
                }
            }
        }

        public virtual async Task<PageListResultDto<NoticeListDto>> GetNotices(GetPageListRequstDto dto)
        {
            var query = _noticeRepository.GetAll();
            var count = await query.GroupJoin(_noticeProjectRepository.GetAll(), left => left.Id, right => right.NoticeId,
                    (left, right) => new
                    {
                        Notice = left,
                        NoticeProjects = right
                    }).CountAsync(t => t.Notice.AllProjects || t.NoticeProjects.FirstOrDefault().ProjectId == _weixinSession.ProjectId);

            var list =
                await query.GroupJoin(_noticeProjectRepository.GetAll(), left => left.Id, right => right.NoticeId,
                    (left, right) => new
                    {
                        Notice = left,
                        NoticeProjects = right
                    })
                    .Where(
                        t =>
                            t.Notice.AllProjects ||
                            t.NoticeProjects.FirstOrDefault().ProjectId == _weixinSession.ProjectId)
                    .OrderBy(t => t.Notice.Sort)
                    .PageBy(dto)
                    .ToListAsync();

            var list1 = list.Select(t => t.Notice).ToList();
            var pageList = list1.MapTo<List<NoticeListDto>>();
            return new PageListResultDto<NoticeListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }

        public virtual async Task<NoticeItemDto> GetNotice(long id)
        {
            var notice = await _noticeRepository.GetAsync(id);
            var dto = notice.MapTo<NoticeItemDto>();
            dto.Content = _detailManager.Get(DetailType.Notice, notice.Id);
            return dto;
        }

        public virtual async Task<PageListResultDto<ActivityListDto>> GetActivitys(GetPageListRequstDto dto)
        {
            var query = _activityRepository.GetAll();
            var count = await query.GroupJoin(_activityProjectRepository.GetAll(), left => left.Id, right => right.ActivityId,
                    (left, right) => new
                    {
                        Activity = left,
                        ActivityProjects = right
                    }).CountAsync(t => t.Activity.AllProjects || t.ActivityProjects.FirstOrDefault().ProjectId == _weixinSession.ProjectId);

            var list =
                await query.GroupJoin(_activityProjectRepository.GetAll(), left => left.Id, right => right.ActivityId,
                    (left, right) => new
                    {
                        Activity = left,
                        ActivityProjects = right
                    })
                    .Where(
                        t =>
                            t.Activity.AllProjects ||
                            t.ActivityProjects.FirstOrDefault().ProjectId == _weixinSession.ProjectId)
                  .OrderBy(t => t.Activity.Sort)
                    .PageBy(dto)
                    .ToListAsync();

            var list1 = list.Select(t => t.Activity).ToList();
            var pageList = list1.MapTo<List<ActivityListDto>>();
            return new PageListResultDto<ActivityListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }

        public virtual async Task<ActivityItemDto> GetActivity(long id)
        {
            var activity = await _activityRepository.GetAsync(id);
            var dto = activity.MapTo<ActivityItemDto>();
            dto.Content = _detailManager.Get(DetailType.Activity, activity.Id);
            return dto;
        }

        public virtual async Task JoinActivity(CreateActivityPersonInput dto)
        {
            dto.MemberId = _weixinSession.MemberId.ToString();
            if (dto.Mobile.Length != 11)
            {
                throw new UserFriendlyException("手机号码格式不正确!");
            }

            var activity = await _activityRepository.GetAsync(dto.ActivityId);
            if (activity == null)
                throw new UserFriendlyException("活动不存在!");

            if (DateTime.Now.Date > activity.Expireday)
            {
                throw new UserFriendlyException("活动已结束!");
            }

            if (activity.JoinCount >= activity.AllowCount)
                throw new UserFriendlyException("活动报名人数已满!");

            if (
                await
                    _activityPersonRepository.CountAsync(
                        t => t.ActivityId == dto.ActivityId && t.MemberId == dto.MemberId) > 0)
            {
                throw new UserFriendlyException("您已经报过名，无需重复报名!");
            }



            var activityPerson = dto.MapTo<ActivityPerson>();
            await _activityPersonRepository.InsertAsync(activityPerson);
            activity.JoinCount += 1;
            await _activityRepository.UpdateAsync(activity);
        }

        public virtual async Task<PageListResultDto<ActivityListDto>> GetMyActivitys(GetPageListRequstDto dto)
        {
            //获取用户参加的活动
            string memberId = _weixinSession.MemberId.ToString();
            var count = await _activityRepository.GetAll()
                .Join(_activityPersonRepository.GetAll(), left => left.Id, right => right.ActivityId,
                    (left, right) => new
                    {
                        Activity = left,
                        ActivityPerson = right
                    })
                    .Where(t => t.ActivityPerson.MemberId == memberId).CountAsync();

            var list = await _activityRepository.GetAll()
                .Join(_activityPersonRepository.GetAll(), left => left.Id, right => right.ActivityId,
                    (left, right) => new
                    {
                        Activity = left,
                        ActivityPerson = right
                    })
                .Where(t => t.ActivityPerson.MemberId == memberId)
                .OrderByDescending(t => t.Activity.Sort)
                .PageBy(dto)
                .ToListAsync();


            var items = list.Select(t => t.Activity).MapTo<List<ActivityListDto>>();

            PageListResultDto<ActivityListDto> result = new PageListResultDto<ActivityListDto>(count, items.ToArray(), dto.PageSize, dto.CurrentPage);
            return result;
        }

        public virtual async Task<bool> CheckIsJoinActivity(long id)
        {
            string memberId = _weixinSession.MemberId.ToString();
            return await _activityPersonRepository.CountAsync(t => t.ActivityId == id && t.MemberId == memberId) >= 1;
        }

        public virtual async Task<PageListResultDto<NewsListDto>> GetNewss(GetPageListRequstDto dto)
        {
            var query = _newsRepository.GetAll();
            var count = await query.GroupJoin(_newsProjectRepository.GetAll(), left => left.Id, right => right.NewsId,
                    (left, right) => new
                    {
                        News = left,
                        NewsProjects = right
                    }).CountAsync(t => t.News.AllProjects || t.NewsProjects.FirstOrDefault().ProjectId == _weixinSession.ProjectId);

            var list =
                await query.GroupJoin(_newsProjectRepository.GetAll(), left => left.Id, right => right.NewsId,
                    (left, right) => new
                    {
                        News = left,
                        NewsProjects = right
                    })
                    .Where(
                        t =>
                            t.News.AllProjects ||
                            t.NewsProjects.FirstOrDefault().ProjectId == _weixinSession.ProjectId)
                    .OrderBy(t => t.News.Sort)
                    .PageBy(dto)
                    .ToListAsync();

            var list1 = list.Select(t => t.News).ToList();
            var pageList = list1.MapTo<List<NewsListDto>>();
            return new PageListResultDto<NewsListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }

        public virtual async Task<NewsItemDto> GetNews(long id)
        {
            var news = await _newsRepository.GetAsync(id);
            var dto = news.MapTo<NewsItemDto>();
            dto.Content = _detailManager.Get(DetailType.News, news.Id);
            return dto;
        }

        public virtual async Task<ServiceTelDto> GetServiceTel()
        {
            ServiceTelDto dto = new ServiceTelDto();
            var slideImages = await _slideimageRepository.GetAll().OrderByDescending(t => t.Sort).ToListAsync();
            dto.SlideImages = slideImages.Select(t => t.Thumbnail).ToList();

            var list = await _serviceTelRepository.GetAll().OrderByDescending(t => t.Sort).ToListAsync();
            dto.Items = list.MapTo<List<ServiceTelListDto>>();

            return dto;
        }

        public virtual async Task<LifeInfoNavDto> GetLifeInfoNav()
        {
            LifeInfoNavDto dto = new LifeInfoNavDto();
            var slideImages = await _slideimageRepository.GetAll().OrderByDescending(t => t.Sort).ToListAsync();
            dto.SlideImages = slideImages.Select(t => t.Thumbnail).ToList();

            var list = await _lifeInfoTypeRepository.GetAll().OrderByDescending(t => t.Sort).ToListAsync();
            dto.Items = list.MapTo<List<LifeInfoTypeListDto>>();

            return dto;
        }

        public virtual async Task<PageListResultDto<LifeInfoListDto>> GetLifeInfoList(GetPageListRequstDto dto, long lifeInfoTypeId)
        {
            var query = _lifeInfoRepository.GetAll();
            var count = await query.GroupJoin(_lifeInfoProjectRepository.GetAll(), left => left.Id, right => right.LifeInfoId,
                    (left, right) => new
                    {
                        LifeInfo = left,
                        LifeInfoProjects = right
                    }).CountAsync(t => ((t.LifeInfo.AllProjects || t.LifeInfoProjects.FirstOrDefault().ProjectId == _weixinSession.ProjectId) && t.LifeInfo.LifeInfoTypeId == lifeInfoTypeId));

            var list =
                await query.GroupJoin(_lifeInfoProjectRepository.GetAll(), left => left.Id, right => right.LifeInfoId,
                    (left, right) => new
                    {
                        LifeInfo = left,
                        LifeInfoProjects = right
                    })
                    .Where(
                        t => ((t.LifeInfo.AllProjects || t.LifeInfoProjects.FirstOrDefault().ProjectId == _weixinSession.ProjectId) && t.LifeInfo.LifeInfoTypeId == lifeInfoTypeId))
                    .OrderBy(t => t.LifeInfo.Sort)
                    .PageBy(dto)
                    .ToListAsync();

            var list1 = list.Select(t => t.LifeInfo).ToList();
            var pageList = list1.MapTo<List<LifeInfoListDto>>();
            return new PageListResultDto<LifeInfoListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }

        public virtual async Task<LifeInfoItemDto> GetLifeInfo(long id)
        {
            var lifeInfo = await _lifeInfoRepository.GetAsync(id);
            var dto = lifeInfo.MapTo<LifeInfoItemDto>();
            dto.Content = _detailManager.Get(DetailType.LifeInfo, lifeInfo.Id);
            return dto;
        }

        public virtual async Task<List<string>> GetSlideImages()
        {
            var slideImages = await _slideimageRepository.GetAll().OrderByDescending(t => t.Sort).ToListAsync();
            return slideImages.Select(t => t.Thumbnail).ToList();
        }

        public virtual async Task<PageListResultDto<SubstationListDto>> GetSubstations(GetPageListRequstDto dto)
        {
            var query = _substationRepository.GetAll(); var where = FilterExpression.FindByGroup<Substation>(dto.Filter);
            var count = await query.Where(where).CountAsync();
            var list = await query.Where(where).OrderByDescending(t => t.Sort).PageBy(dto).ToListAsync();
            var pageList = list.MapTo<List<SubstationListDto>>();
            return new PageListResultDto<SubstationListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }

        public virtual async Task<SubstationItemDto> GetSubstations(long id)
        {
            var lifeInfo = await _substationRepository.GetAsync(id);
            return lifeInfo.MapTo<SubstationItemDto>();
        }

        public async Task<List<ComboboxItemDto>> GetEstateinfoTypesForCombo()
        {
            var items = await _estateinfotypeRepository.GetAll().OrderByDescending(t => t.Sort).ToListAsync();
            var list = items.Select(result =>
            {
                var item = new ComboboxItemDto();
                item.DisplayText = result.Name;
                item.Value = result.Id.ToString();
                return item;
            }).ToList();
            return list;
        }

        public virtual async Task CreateEstateinfo(CreateEstateinfoInput input)
        {
            input.MemberId = _weixinSession.MemberId.ToString();
            var estateinfo = input.MapTo<Estateinfo>();
            estateinfo.IsSale = true;
            //estateinfo.IsShow = true;
            estateinfo.Summary = input.Content.Length > 50 ? input.Content.Substring(0, 50) : input.Content;
            if (input.Images != null && input.Images.Count > 0)
                estateinfo.Thumbnail = input.Images[0];

            var dataid = await _estateinfoRepository.InsertAndGetIdAsync(estateinfo);
            _detailManager.Save(DetailType.Estateinfo, dataid, input.Content);
            if (input.Images != null)
            {
                foreach (var image in input.Images)
                {
                    EstateinfoImage estateinfoImage = new EstateinfoImage();
                    estateinfoImage.EstateinfoId = dataid;
                    estateinfoImage.Image = image;
                    await _estateinfoImageRepository.InsertAsync(estateinfoImage);
                }
            }

        }

        public virtual async Task<PageListResultDto<EstateinfoListDto>> GetEstateinfos(int mode, GetPageListRequstDto dto)
        {
            var query = _estateinfoRepository.GetAll();
            var where = FilterExpression.FindByGroup<Estateinfo>(dto.Filter);
            where = where.And(t => t.IsSale && t.IsShow);
            if (mode != 0)
            {
                where = where.And(t => t.EstateinfoTypeId == mode);

            }
            var count = await query.Where(where).CountAsync();
            var list = await query.Where(where).OrderByDescending(t => t.CreationTime).PageBy(dto).ToListAsync();
            var pageList = list.MapTo<List<EstateinfoListDto>>();

            return new PageListResultDto<EstateinfoListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }

        public virtual async Task<EstateinfoItemDto> GetEstateinfo(long id)
        {
            var estateinfo = await _estateinfoRepository.GetAsync(id);
            var dto = estateinfo.MapTo<EstateinfoItemDto>();
            dto.Content = _detailManager.Get(DetailType.Estateinfo, estateinfo.Id);

            dto.Images = _estateinfoImageRepository.GetAll().Where(t => t.EstateinfoId == dto.Id).Select(t => t.Image).ToList();
            dto.Comments =
                _estateinfoCommentRepository.GetAll()
                    .Where(t => t.EstateinfoId == dto.Id)
                    .OrderByDescending(t => t.CreationTime)
                    .Select(t => new EstateinfoCommentListDto() { MemberName = t.MemberName, Content = t.Content, CreationTime = t.CreationTime })
                    .ToList();

            return dto;
        }

        public virtual async Task CreateEstateinfoComment(CreateEstateinfoCommentInput input)
        {
            input.MemberId = _weixinSession.MemberId.ToString();
            input.MemberName = _weixinSession.MemberName;

            if (string.IsNullOrEmpty(input.MemberId) || string.IsNullOrEmpty(input.MemberName))
            {
                throw new UserFriendlyException("请登录后使用该功能.");
            }

            var estateinfoComment = input.MapTo<EstateinfoComment>();
            await _estateinfoCommentRepository.InsertAsync(estateinfoComment);
        }

        public virtual async Task<EstateinfoMyInfoDto> GetEstateinfoMyInfo()
        {
            string memberId = _weixinSession.MemberId.ToString();
            EstateinfoMyInfoDto result = new EstateinfoMyInfoDto();
            result.UnShow = await _estateinfoRepository.CountAsync(t => t.MemberId == memberId && !t.IsShow);
            result.IsSale = await _estateinfoRepository.CountAsync(t => t.MemberId == memberId && t.IsShow && t.IsSale);
            result.UnSale = await _estateinfoRepository.CountAsync(t => t.MemberId == memberId && t.IsShow && !t.IsSale);
            return result;
        }

        public virtual async Task<PageListResultDto<EstateinfoListDto>> GetMyEstateinfos(int type, GetPageListRequstDto dto)
        {
            string memberId = _weixinSession.MemberId.ToString();
            var query = _estateinfoRepository.GetAll();
            var where = FilterExpression.FindByGroup<Estateinfo>(dto.Filter);
            where = where.And(t => t.MemberId == memberId);
            switch (type)
            {
                case 0:
                    where = where.And(t => !t.IsShow);
                    break;
                case 1:
                    where = where.And(t => t.IsShow && t.IsSale);
                    break;
                case 2:
                    where = where.And(t => t.IsShow && !t.IsSale);
                    break;
            }

            var count = await query.Where(where).CountAsync();
            var list = await query.Where(where).OrderByDescending(t => t.CreationTime).PageBy(dto).ToListAsync();
            var pageList = list.MapTo<List<EstateinfoListDto>>();

            return new PageListResultDto<EstateinfoListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }

        public virtual async Task SetEstateinfoSale(long id)
        {
            string memberId = _weixinSession.MemberId.ToString();
            var info = await _estateinfoRepository.GetAsync(id);
            if (info == null || info.MemberId != memberId)
                throw new UserFriendlyException("你无权操作此功能!");
            info.IsSale = true;
            await _estateinfoRepository.UpdateAsync(info);
        }

        public virtual async Task SetEstateinfoUnSale(long id)
        {
            string memberId = _weixinSession.MemberId.ToString();
            var info = await _estateinfoRepository.GetAsync(id);
            if (info == null || info.MemberId != memberId)
                throw new UserFriendlyException("你无权操作此功能!");
            info.IsSale = false;
            await _estateinfoRepository.UpdateAsync(info);
        }


        public async Task<List<ComboboxItemDto>> GetRentsaleinfoTypesForCombo()
        {
            var items = await _rentsaleinfotypeRepository.GetAll().OrderByDescending(t => t.Sort).ToListAsync();
            var list = items.Select(result =>
            {
                var item = new ComboboxItemDto();
                item.DisplayText = result.Name;
                item.Value = result.Id.ToString();
                return item;
            }).ToList();
            return list;
        }

        public virtual async Task CreateRentsaleinfo(CreateRentsaleinfoInput input)
        {
            input.MemberId = _weixinSession.MemberId.ToString();
            var rentsaleinfo = input.MapTo<Rentsaleinfo>();
            rentsaleinfo.IsSale = true;
            //rentsaleinfo.IsShow = true;
            rentsaleinfo.Summary = input.Content.Length > 50 ? input.Content.Substring(0, 50) : input.Content;
            if (input.Images != null && input.Images.Count > 0)
                rentsaleinfo.Thumbnail = input.Images[0];

            var dataid = await _rentsaleinfoRepository.InsertAndGetIdAsync(rentsaleinfo);
            _detailManager.Save(DetailType.Rentsaleinfo, dataid, input.Content);
            if (input.Images != null)
            {
                foreach (var image in input.Images)
                {
                    RentsaleinfoImage rentsaleinfoImage = new RentsaleinfoImage();
                    rentsaleinfoImage.RentsaleinfoId = dataid;
                    rentsaleinfoImage.Image = image;
                    await _rentsaleinfoImageRepository.InsertAsync(rentsaleinfoImage);
                }
            }

        }

        public virtual async Task<PageListResultDto<RentsaleinfoListDto>> GetRentsaleinfos(int mode, GetPageListRequstDto dto)
        {
            var query = _rentsaleinfoRepository.GetAll();
            var where = FilterExpression.FindByGroup<Rentsaleinfo>(dto.Filter);
            where = where.And(t => t.IsSale && t.IsShow);
            if (mode != 0)
            {
                where = where.And(t => t.RentsaleinfoTypeId == mode);

            }
            var count = await query.Where(where).CountAsync();
            var list = await query.Where(where).OrderByDescending(t => t.CreationTime).PageBy(dto).ToListAsync();
            var pageList = list.MapTo<List<RentsaleinfoListDto>>();

            return new PageListResultDto<RentsaleinfoListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }

        public virtual async Task<RentsaleinfoItemDto> GetRentsaleinfo(long id)
        {
            var rentsaleinfo = await _rentsaleinfoRepository.GetAsync(id);
            var dto = rentsaleinfo.MapTo<RentsaleinfoItemDto>();
            dto.Content = _detailManager.Get(DetailType.Rentsaleinfo, rentsaleinfo.Id);

            dto.Images = _rentsaleinfoImageRepository.GetAll().Where(t => t.RentsaleinfoId == dto.Id).Select(t => t.Image).ToList();


            return dto;
        }


        public virtual async Task<RentsaleinfoMyInfoDto> GetRentsaleinfoMyInfo()
        {
            string memberId = _weixinSession.MemberId.ToString();
            RentsaleinfoMyInfoDto result = new RentsaleinfoMyInfoDto();
            result.UnShow = await _rentsaleinfoRepository.CountAsync(t => t.MemberId == memberId && !t.IsShow);
            result.IsSale = await _rentsaleinfoRepository.CountAsync(t => t.MemberId == memberId && t.IsShow && t.IsSale);
            result.UnSale = await _rentsaleinfoRepository.CountAsync(t => t.MemberId == memberId && t.IsShow && !t.IsSale);
            return result;
        }

        public virtual async Task<PageListResultDto<RentsaleinfoListDto>> GetMyRentsaleinfos(int type, GetPageListRequstDto dto)
        {
            string memberId = _weixinSession.MemberId.ToString();
            var query = _rentsaleinfoRepository.GetAll();
            var where = FilterExpression.FindByGroup<Rentsaleinfo>(dto.Filter);
            where = where.And(t => t.MemberId == memberId);
            switch (type)
            {
                case 0:
                    where = where.And(t => !t.IsShow);
                    break;
                case 1:
                    where = where.And(t => t.IsShow && t.IsSale);
                    break;
                case 2:
                    where = where.And(t => t.IsShow && !t.IsSale);
                    break;
            }

            var count = await query.Where(where).CountAsync();
            var list = await query.Where(where).OrderByDescending(t => t.CreationTime).PageBy(dto).ToListAsync();
            var pageList = list.MapTo<List<RentsaleinfoListDto>>();

            return new PageListResultDto<RentsaleinfoListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }

        public virtual async Task SetRentsaleinfoSale(long id)
        {
            string memberId = _weixinSession.MemberId.ToString();
            var info = await _rentsaleinfoRepository.GetAsync(id);
            if (info == null || info.MemberId != memberId)
                throw new UserFriendlyException("你无权操作此功能!");
            info.IsSale = true;
            await _rentsaleinfoRepository.UpdateAsync(info);
        }

        public virtual async Task SetRentsaleinfoUnSale(long id)
        {
            string memberId = _weixinSession.MemberId.ToString();
            var info = await _rentsaleinfoRepository.GetAsync(id);
            if (info == null || info.MemberId != memberId)
                throw new UserFriendlyException("你无权操作此功能!");
            info.IsSale = false;
            await _rentsaleinfoRepository.UpdateAsync(info);
        }

        public virtual async Task<MyCountInfo> GetMyCountInfo(string memberId)
        {
            MyCountInfo info = new MyCountInfo();

            info.EstateinfoUnShow = await _estateinfoRepository.CountAsync(t => t.MemberId == memberId && !t.IsShow);
            info.EstateinfoIsSale = await _estateinfoRepository.CountAsync(t => t.MemberId == memberId && t.IsShow && t.IsSale);
            info.EstateinfoUnSale = await _estateinfoRepository.CountAsync(t => t.MemberId == memberId && t.IsShow && !t.IsSale);

            info.RentsaleinfoUnShow = await _rentsaleinfoRepository.CountAsync(t => t.MemberId == memberId && !t.IsShow);
            info.RentsaleinfoIsSale = await _rentsaleinfoRepository.CountAsync(t => t.MemberId == memberId && t.IsShow && t.IsSale);
            info.RentsaleinfoUnSale = await _rentsaleinfoRepository.CountAsync(t => t.MemberId == memberId && t.IsShow && !t.IsSale);

            info.ActivityJoinCount = await _activityPersonRepository.CountAsync(t => t.MemberId == memberId);

            return info;
        }

        public Task<string> CreatePay()
        {
            return null;
        }

        public List<MemberRoomItem> GetMyRooms()
        {
            List<MemberRoomItem> roomItems = new List<MemberRoomItem>();
            var result = _wuyeApiAppSrvice.GetMemberInfoByAPPUserID(_weixinSession.MemberId.ToString());
            if (result.Success)
            {
                foreach (var item in result.PRooms ?? new List<GetMemberInfoByAPPUserIDResult.PRoomItem>())
                {
                    roomItems.Add(new MemberRoomItem()
                    {
                        PRoomFullName = item.PRoomFullName,
                        PRoomCode = item.PRoomCode,
                        PProjectCode = item.PProjectCode,
                        IsDefault = (result.PRoomCode == item.PRoomCode)
                    });
                }
            }

            return roomItems;
        }

        public AddLinkBillResult AddService(AddServiceDto dto)
        {
            if (_weixinSession.MemberId == 0)
            {
                throw new UserFriendlyException("请先登录!");
            }

            var appUserInfo = _wuyeApiAppSrvice.GetMemberInfoByAPPUserID(_weixinSession.MemberId.ToString());
            if (!appUserInfo.Success || string.IsNullOrEmpty(appUserInfo.CustomerID))
                throw new UserFriendlyException("请先绑定房间!");

            AddLinkBillDto linkBillDto = new AddLinkBillDto();
            linkBillDto.PRoomCode = dto.PRoomCode;
            linkBillDto.BusiType = dto.BusiType;
            linkBillDto.FileAddress = dto.FileAddress;
            linkBillDto.LinkContent = dto.LinkContent;
            linkBillDto.MemberID = _weixinSession.MemberId.ToString();
            linkBillDto.CustomerID = appUserInfo.CustomerID.ToString();
            var result = _wuyeApiAppSrvice.AddLinkBill(linkBillDto);
            return result;
        }

        public GetNoPayFeeByMemberResult GetNoPayFeeByMember(string PRoomCode)
        {
            var result = _wuyeApiAppSrvice.GetNoPayFeeByMember("0", _weixinSession.MemberId.ToString(), PRoomCode);
            return result;
        }

        public void SetCurrentRoomInfo(string PRoomCode)
        {
            var result = _wuyeApiAppSrvice.ChangeDefaultPRoom(_weixinSession.MemberId.ToString(), PRoomCode);
            if (!result.Success)
                throw new UserFriendlyException(result.msg);
        }

        public GetPRoomInfoByPhoneResult GetPRoomInfoByPhone(string Phone)
        {
            var result = _wuyeApiAppSrvice.GetPRoomInfoByPhone(Phone, _weixinSession.MemberId.ToString());
            return result;
        }

        public GetPRoomInfoByPhoneResult GetPRoomInfoByQRCode(string content)
        {
            var result = _wuyeApiAppSrvice.GetPRoomInfoByQRCode(content, _weixinSession.MemberId.ToString());
            return result;
        }

        public GetPRoomInfoByPhoneResult GetPRoomInfoByUserPass(string username, string password)
        {
            var result = _wuyeApiAppSrvice.GetPRoomInfoByUserPass(username, password, _weixinSession.MemberId.ToString());
            return result;
        }

        public InvokeResultDto MemberAddPRoom(string pRoomCode,string role)
        {
            var result = _wuyeApiAppSrvice.MemberAddPRoom(_weixinSession.MemberId.ToString(), pRoomCode, role);

            var userInfo = _wuyeApiAppSrvice.GetMemberInfoByAPPUserID(_weixinSession.MemberId.ToString());
            //更新到数据库

            if (userInfo.Success)
            {

                var memberInfo = _memberRepository.FirstOrDefault(t => t.Id == _weixinSession.MemberId);
                if (memberInfo != null)
                {
                    memberInfo.BindRooms = userInfo.PProjectCode;
                    memberInfo.PRoomFullName = userInfo.PRoomFullName;
                    memberInfo.ProjectCode = userInfo.PProjectCode;
                    _memberRepository.Update(memberInfo);
                }

            }

            return result;
        }

        public InvokeResultDto MemberRemovePRoom(string pRoomCode)
        {
            var result = _wuyeApiAppSrvice.MemberRemovePRoom(_weixinSession.MemberId.ToString(), pRoomCode);
            return result;
        }

        public SendSMSResult SendSMS(string Phone, int Type)
        {
            var result = _wuyeApiAppSrvice.SendSMS(Phone, Type);
            return result;
        }

        public void Registered(string Phone, string Password, string Name, string NickName)
        {
            string openid = _weixinSession.OpenId;
            Password = MD5Helper.EnCode(Password).ToLower();
            var result = _wuyeApiAppSrvice.Registered(Phone, Password, Name, NickName);
            if (!result.Success)
                throw new UserFriendlyException(result.msg);

            var userInfo = _wuyeApiAppSrvice.GetUserInfo(result.Id.ToString());
            if (userInfo.infos == null || userInfo.infos.Count == 0)
            {
                throw new UserFriendlyException("从物业系统获取用户信息失败.");
            }

            var appUserInfo = _wuyeApiAppSrvice.GetMemberInfoByAPPUserID(result.Id.ToString());
            if (!appUserInfo.Success)
            {
                throw new UserFriendlyException("从物业系统获取用户信息失败.");
            }

            long memberId = result.Id;
            var memberInfo = _memberRepository.FirstOrDefault(t => t.Id == memberId);

            //一个openid只能绑定一个账号
            var openidWithMember = _memberRepository.FirstOrDefault(t => t.Openid == openid);
            if (memberInfo == null)
            {
                if (openidWithMember != null)
                    throw new UserFriendlyException("该微信已经绑定账号，不能绑定多个账号!");

                Member member = new Member();
                member.Id = result.Id;
                member.MemberId = result.Id.ToString();
                member.Openid = openid;
                member.Name = userInfo.infos[0].NickName;
                member.BindRooms = userInfo.infos[0].PProjectName;
                member.PRoomFullName = userInfo.infos[0].PRoomFullName;
                member.ProjectCode = appUserInfo.PProjectCode;
                member.Thumbnail = "/Content/frontpage/i/default-headimg.jpg";
                _memberRepository.Insert(member);
            }

        }

        public Member GetMy()
        {

            var userInfo = _wuyeApiAppSrvice.GetMemberInfoByAPPUserID(_weixinSession.MemberId.ToString());
            //更新到数据库

            if (userInfo.Success)
            {
                var memberInfo = _memberRepository.FirstOrDefault(t => t.Id == _weixinSession.MemberId);
                if (memberInfo != null)
                {
                    memberInfo.BindRooms = userInfo.PProjectCode;
                    memberInfo.PRoomFullName = userInfo.PRoomFullName;
                    memberInfo.ProjectCode = userInfo.PProjectCode;
                    _memberRepository.UpdateAsync(memberInfo);
                }
            }

            return _memberRepository.FirstOrDefault(t => t.Id == _weixinSession.MemberId);
        }

        public CreatePayBillResult CreatePayBill(string FeedIds, string PRoomCode, string Amount)
        {
            return _wuyeApiAppSrvice.CreatePayBill(FeedIds, _weixinSession.MemberId.ToString(), PRoomCode, Amount);
        }

        public GetPRoomPrePayItemResult GetPRoomPrePayItem(string pRoomCode)
        {
            var result = _wuyeApiAppSrvice.GetPRoomPrePayItem(pRoomCode);

            if (result.ItemList != null)
            {
                for (int i = result.ItemList.Count - 1; i >= 0; i--)
                {
                    if (result.ItemList[i].Type == 0 && string.IsNullOrEmpty(result.ItemList[i].BeginYM))
                        result.ItemList.RemoveAt(i);
                }
            }


            return result;
        }

        public CreatePayOrderPrePayResult CreatePayOrderPrePay(string pRoomCode, string PrePayItems, string AmountSummary)
        {
            var result = _wuyeApiAppSrvice.CreatePayOrderPrePay(pRoomCode, PrePayItems, AmountSummary);
            return result;
        }

        public CreateUserByWXResult CreateUserByWX(string phone, string nickname, string name)
        {
            var result = _wuyeApiAppSrvice.CreateUserByWX(phone, nickname, name);
            if (!result.Success)
                return result;
            string openid = _weixinSession.OpenId;
            var userInfo = _wuyeApiAppSrvice.GetUserInfo(result.Id.ToString());
            if (userInfo.infos == null || userInfo.infos.Count == 0)
            {
                result.result = "fail";
                result.msg = "从物业系统获取用户信息失败.";
                return result;
            }

            var appUserInfo = _wuyeApiAppSrvice.GetMemberInfoByAPPUserID(result.Id.ToString());
            if (!appUserInfo.Success)
            {

                result.result = "fail";
                result.msg = "从物业系统获取用户信息失败.";
                return result;

            }

            long memberId = result.Id;
            var memberInfo = _memberRepository.FirstOrDefault(t => t.Id == memberId);

            //一个openid只能绑定一个账号
            var openidWithMember = _memberRepository.FirstOrDefault(t => t.Openid == openid);
            if (memberInfo == null)
            {
                if (openidWithMember != null)
                {
                    result.result = "fail";
                    result.msg = "该微信已经绑定账号，不能绑定多个账号.";
                    return result;
                }

                string thumbnail = _weiXinApi.GetHeadImage(openid, "/Content/frontpage/i/default-headimg.jpg");

                Member member = new Member();
                member.Id = result.Id;
                member.MemberId = result.Id.ToString();
                member.Openid = openid;
                member.Name = userInfo.infos[0].NickName;
                member.BindRooms = userInfo.infos[0].PProjectName;
                member.PRoomFullName = userInfo.infos[0].PRoomFullName;
                member.ProjectCode = appUserInfo.PProjectCode;
                member.Thumbnail = thumbnail;
                _memberRepository.Insert(member);
            }
            else
            {
                if (openidWithMember != null)
                {
                    result.result = "fail";
                    result.msg = "该微信已经绑定账号，不能绑定多个账号.";
                    return result;
                }
                string thumbnail = _weiXinApi.GetHeadImage(openid, "/Content/frontpage/i/default-headimg.jpg");
                memberInfo.Openid = openid;
                memberInfo.Name = userInfo.infos[0].NickName;
                memberInfo.BindRooms = userInfo.infos[0].PProjectName;
                memberInfo.PRoomFullName = userInfo.infos[0].PRoomFullName;
                memberInfo.ProjectCode = appUserInfo.PProjectCode;
                memberInfo.Thumbnail = thumbnail;
                _memberRepository.Update(memberInfo);
            }

            return result;
        }

        public virtual async Task<MallHomeData> GetMallHomeData()
        {
            MallHomeData data = new MallHomeData();

            var images = await _mallslideimageRepository.GetAll().OrderBy(t => t.Sort).ToListAsync();
            data.SlideImages = images.MapTo<List<MallSlideImageListDto>>();


            var query = _grouponRepository.GetAll();
            var where = FilterExpression.FindByGroup<Groupon>(new List<FilterCondition>());


            var queryList = query.Where(where)
               .GroupJoin(_productRepository.GetAll(), left => left.ProductId, right => right.Id,
                   (left, right) => new
                   {
                       Groupon = left,
                       Product = right
                   });

            queryList = queryList.Where(t => t.Groupon.StartTime <= DateTime.Now && t.Groupon.ExpireTime > DateTime.Now && t.Product.FirstOrDefault().IsSale);

            var tts = await queryList.Select(t => new GrouponAndProduct()
            {
                Groupon = t.Groupon,
                Product = t.Product.FirstOrDefault()
            }).OrderByDescending(t => t.Groupon.CreationTime)
            .Take(10)
            .ToListAsync();

            var grouponListDtos = tts.Select(
                result =>
                {
                    var grouponInfo = result.Groupon.MapTo<GrouponItem>();
                    grouponInfo.Name = result.Product?.Name;
                    grouponInfo.Unit = result.Product?.Unit;
                    grouponInfo.Thumbnail = result.Product?.Thumbnail;
                    if (result.Product?.SellCount != null)
                        grouponInfo.SellCount = (result.Product?.SellCount).Value;
                    return grouponInfo;
                }).ToList();

            data.Groupons = grouponListDtos.MapTo<List<GrouponItem>>();

            return data;
        }



        public async Task<List<ComboboxItemDto>> GetCategories()
        {
            var items = await _categoryRepository.GetAll().OrderBy(t => t.Sort).ToListAsync();
            var list = items.Select(result =>
            {
                var item = new ComboboxItemDto();
                item.DisplayText = result.Name;
                item.Value = result.Id.ToString();
                return item;
            }).ToList();

            list.Insert(0, new ComboboxItemDto("0", "全部"));

            return list;
        }

        public async Task<PageListResultDto<GrouponItem>> GetGrouponItems(int cid, GetPageListRequstDto dto)
        {
            var query = _grouponRepository.GetAll();
            var where = FilterExpression.FindByGroup<Groupon>(dto.Filter);



            var queryCount = query.Where(where)
                 .GroupJoin(_productRepository.GetAll(), left => left.ProductId, right => right.Id,
                   (left, right) => new
                   {
                       Groupon = left,
                       Product = right
                   });
            if (cid != 0)
            {
                queryCount = queryCount.Where(t => t.Product.FirstOrDefault().CategoryId == cid);
            }


            queryCount = queryCount.Where(t => t.Groupon.StartTime <= DateTime.Now && t.Groupon.ExpireTime > DateTime.Now && t.Product.FirstOrDefault().IsSale);

            int count = await queryCount.CountAsync();

            var queryList = query.Where(where)
               .GroupJoin(_productRepository.GetAll(), left => left.ProductId, right => right.Id,
                   (left, right) => new
                   {
                       Groupon = left,
                       Product = right
                   });

            if (cid != 0)
            {
                queryList = queryList.Where(t => t.Product.FirstOrDefault().CategoryId == cid);
            }

            queryList = queryList.Where(t => t.Groupon.StartTime <= DateTime.Now && t.Groupon.ExpireTime > DateTime.Now && t.Product.FirstOrDefault().IsSale);

            var tts = await queryList.Select(t => new GrouponAndProduct()
            {
                Groupon = t.Groupon,
                Product = t.Product.FirstOrDefault()
            }).OrderByDescending(t => t.Groupon.CreationTime).PageBy(dto).ToListAsync();

            var grouponListDtos = tts.Select(
                result =>
                {
                    var grouponInfo = result.Groupon.MapTo<GrouponItem>();
                    grouponInfo.Name = result.Product?.Name;
                    grouponInfo.Unit = result.Product?.Unit;
                    grouponInfo.Thumbnail = result.Product?.Thumbnail;
                    if (result.Product?.SellCount != null)
                        grouponInfo.SellCount = (result.Product?.SellCount).Value;
                    return grouponInfo;
                }).ToList();

            var pageList = grouponListDtos.MapTo<List<GrouponItem>>();
            return new PageListResultDto<GrouponItem>(count, pageList, dto.CurrentPage, dto.PageSize);
        }

        public async Task<GrouponDetail> GetGrouponDetail(long id)
        {
            var grouponInfo = await _grouponRepository.GetAsync(id);
            if (grouponInfo == null)
                throw new UserFriendlyException("拼团信息不存在!");
            var productInfo = await _productRepository.GetAsync(grouponInfo.ProductId);


            GrouponDetail detail = new GrouponDetail();
            detail.Id = grouponInfo.Id;
            detail.Name = productInfo.Name;
            detail.CommentCount = 0;
            detail.Comments = new List<GrouponDetail.CommentItem>();
            detail.Content = productInfo.Content;
            detail.IsSale = grouponInfo.IsSale;
            detail.Price = grouponInfo.Price;
            detail.RequireCount = grouponInfo.RequireCount;
            detail.SalePrice = productInfo.SalePrice;
            detail.SellCount = productInfo.SellCount;
            detail.SlideImages = new List<string>();
            detail.Summary = grouponInfo.Summary;
            detail.Tags = new List<string>(productInfo.ServiceTags.Split(','));
            detail.Unit = productInfo.Unit;
            detail.Thumbnail = productInfo.Thumbnail;
            detail.ProductId = productInfo.Id;
            detail.MemberPrice = productInfo.MemberPrice;
            detail.Comments = new List<GrouponDetail.CommentItem>();
            detail.StartTime = grouponInfo.StartTime;
            detail.ExpireTime = grouponInfo.ExpireTime;


            var images = await _productSlideimageRepository.GetAll()
                .OrderBy(t => t.Sort)
                .Where(t => t.ProductId == productInfo.Id)
                .ToListAsync();
            foreach (var image in images)
            {
                detail.SlideImages.Add(image.Thumbnail);
            }

            //查询已经开团的数据(只显示最近的2条)
            var grouponings = await _groupOrderRepository.GetAll()
                .GroupJoin(_memberRepository.GetAll(), left => left.HeaderId, right => right.Id, (left, right) => new
                {
                    GrouponOrder = left,
                    Member = right
                })
                .Where(t => t.GrouponOrder.GrouponId == grouponInfo.Id && t.GrouponOrder.GrouponStatus == GrouponStatus.Grouponing).OrderByDescending(t => t.GrouponOrder.CreationTime)
                .Take(5)
                .ToListAsync();
            detail.GrouponingItems = new List<GrouponDetail.GrouponingItem>();
            foreach (var grouponing in grouponings)
            {
                detail.GrouponingItems.Add(new GrouponDetail.GrouponingItem()
                {
                    Id = grouponing.GrouponOrder.Id,
                    ExpireTime = grouponing.GrouponOrder.ExpireTime,
                    Name = grouponing.Member.FirstOrDefault()?.Name,
                    Thumbnail = grouponing.Member.FirstOrDefault()?.Thumbnail,
                    RequireCount = grouponing.GrouponOrder.RequireCount - grouponing.GrouponOrder.JoinCount,
                    BindRooms = grouponing.Member.FirstOrDefault()?.BindRooms
                });
            }

            detail.CommentCount = await _productCommentRepository.CountAsync(t => t.ProductId == detail.ProductId);
            var comments = await _productCommentRepository.GetAll()
                .GroupJoin(_memberRepository.GetAll(), left => left.MemberId, right => right.Id, (left, right) => new
                {
                    ProductComment = left,
                    Member = right
                })
                .Where(t => t.ProductComment.ProductId == grouponInfo.ProductId).OrderByDescending(t => t.ProductComment.CreationTime)
                .Take(2)
                .ToListAsync();

            foreach (var comment in comments)
            {
                detail.Comments.Add(new GrouponDetail.CommentItem()
                {
                    Name = comment.Member.FirstOrDefault()?.Name,
                    Thumbnail = comment.Member.FirstOrDefault()?.Thumbnail,
                    Content = comment.ProductComment.Content,
                    CreationTime = comment.ProductComment.CreationTime
                });
            }

            if (
                await
                    _productLikeRepository.FirstOrDefaultAsync(
                        t => t.MemberId == _weixinSession.MemberId && t.ProductId == detail.ProductId) != null)
            {
                detail.IsLike = true;
            }

            return detail;
        }
        public async Task<List<ComboboxItemDto>> GetProvinces()
        {
            var items = await _areaRepository.GetAll().Where(t => t.Level == 1).OrderBy(t => t.Sort).ToListAsync();
            var list = items.Select(result =>
            {
                var item = new ComboboxItemDto();
                item.DisplayText = result.Short_name;
                item.Value = result.Id.ToString();
                return item;
            }).ToList();
            return list;
        }

        public async Task<List<ComboboxItemDto>> GetCities(long provinceId)
        {
            var items = await _areaRepository.GetAll().Where(t => t.Level == 2 && t.Parent_id == provinceId).OrderBy(t => t.Sort).ToListAsync();
            var list = items.Select(result =>
            {
                var item = new ComboboxItemDto();
                item.DisplayText = result.Short_name;
                item.Value = result.Id.ToString();
                return item;
            }).ToList();
            return list;
        }

        public async Task<List<ComboboxItemDto>> GetDistricts(long cityId)
        {
            var items = await _areaRepository.GetAll().Where(t => t.Level == 3 && t.Parent_id == cityId).OrderBy(t => t.Sort).ToListAsync();
            var list = items.Select(result =>
            {
                var item = new ComboboxItemDto();
                item.DisplayText = result.Short_name;
                item.Value = result.Id.ToString();
                return item;
            }).ToList();
            return list;
        }

        public async Task AddAddress(MemberAddress address)
        {
            //如果地址设置为默认，先将该用户的其他地址设置为非默认
            address.MemberId = _weixinSession.MemberId;
            if (address.IsDefault)
            {
                var defaults = await _memberAddressRepository.GetAll().Where(t => t.IsDefault && t.MemberId == address.MemberId).ToListAsync();
                foreach (var @default in defaults)
                {
                    @default.IsDefault = false;
                    await _memberAddressRepository.UpdateAsync(@default);
                }
            }

            await _memberAddressRepository.InsertAsync(address);
        }

        public async Task EditAddress(MemberAddress address)
        {
            var oldAddress = await _memberAddressRepository.GetAsync(address.Id);

            oldAddress.Contact = address.Contact;
            oldAddress.Mobile = address.Mobile;
            oldAddress.Address = address.Address;
            oldAddress.IsDefault = address.IsDefault;
            oldAddress.ProvinceId = address.ProvinceId;
            oldAddress.CityId = address.CityId;
            oldAddress.DistrictId = address.DistrictId;

            await _memberAddressRepository.UpdateAsync(oldAddress);
        }

        public async Task<List<MemberAddressListItem>> GetAddresss()
        {
            var query = _memberAddressRepository.GetAll();
            var where = FilterExpression.FindByGroup<MemberAddress>(new List<FilterCondition>());


            var queryList = query.Where(where)
                .GroupJoin(_areaRepository.GetAll(), left => left.ProvinceId, province => province.Id,
                    (left, province) => new
                    {
                        MemberAddress = left,
                        Province = province
                    })
                .GroupJoin(_areaRepository.GetAll(), left => left.MemberAddress.CityId, city => city.Id,
                    (left, city) => new
                    {
                        MemberAddress = left.MemberAddress,
                        Province = left.Province,
                        City = city
                    })
               .GroupJoin(_areaRepository.GetAll(), left => left.MemberAddress.DistrictId, cistrict => cistrict.Id,
                    (left, cistrict) => new
                    {
                        MemberAddress = left.MemberAddress,
                        Province = left.Province,
                        City = left.City,
                        District = cistrict
                    });

            queryList = queryList.Where(t => t.MemberAddress.MemberId == _weixinSession.MemberId);

            var tts = await queryList.Select(t => new MemberAddressAndAera()
            {
                MemberAddress = t.MemberAddress,
                Province = t.Province.FirstOrDefault(),
                City = t.City.FirstOrDefault(),
                District = t.District.FirstOrDefault()
            }).OrderByDescending(t => t.MemberAddress.CreationTime).ToListAsync();

            var addresss = tts.Select(
                result =>
                {
                    var info = result.MemberAddress.MapTo<MemberAddressListItem>();
                    info.ProvinceName = result.Province?.Short_name;
                    info.CityName = result.City?.Short_name;
                    info.DistrictName = result.District?.Short_name;
                    return info;
                }).ToList();
            return addresss;
        }

        public async Task DeleteAddress(long id)
        {
            await _memberAddressRepository.DeleteAsync(id);
        }

        public async Task SetDefaultAddress(long id)
        {
            var addresInfo = await _memberAddressRepository.GetAsync(id);

            var defaults = await _memberAddressRepository.GetAll().Where(t => t.IsDefault && t.MemberId == addresInfo.MemberId).ToListAsync();
            foreach (var @default in defaults)
            {
                @default.IsDefault = false;
                await _memberAddressRepository.UpdateAsync(@default);
            }
            addresInfo.IsDefault = true;
            await _memberAddressRepository.UpdateAsync(addresInfo);
        }

        public async Task<MemberAddress> GetAddress(long id)
        {
            var addresInfo = await _memberAddressRepository.GetAsync(id);
            return addresInfo;
        }

        public async Task<bool> CheckMemberIsOwner(string id)
        {
            var memberInfo = await _memberRepository.FirstOrDefaultAsync(Convert.ToInt64(id));
            if (string.IsNullOrEmpty(memberInfo?.BindRooms))
                return false;
            return true;
        }

        public async Task<MemberAddressListItem> GetAddressForSubmit(long selectAddressId)
        {
            var query = _memberAddressRepository.GetAll();
            var where = FilterExpression.FindByGroup<MemberAddress>(new List<FilterCondition>());


            var queryList = query.Where(where)
                .GroupJoin(_areaRepository.GetAll(), left => left.ProvinceId, province => province.Id,
                    (left, province) => new
                    {
                        MemberAddress = left,
                        Province = province
                    })
                .GroupJoin(_areaRepository.GetAll(), left => left.MemberAddress.CityId, city => city.Id,
                    (left, city) => new
                    {
                        MemberAddress = left.MemberAddress,
                        Province = left.Province,
                        City = city
                    })
               .GroupJoin(_areaRepository.GetAll(), left => left.MemberAddress.DistrictId, cistrict => cistrict.Id,
                    (left, cistrict) => new
                    {
                        MemberAddress = left.MemberAddress,
                        Province = left.Province,
                        City = left.City,
                        District = cistrict
                    });

            queryList = queryList.Where(t => t.MemberAddress.MemberId == _weixinSession.MemberId);
            if (selectAddressId != 0)
            {
                if (
                    await
                        _memberAddressRepository.FirstOrDefaultAsync(
                            t => t.MemberId == _weixinSession.MemberId && t.Id == selectAddressId) != null)
                {
                    queryList = queryList.Where(t => t.MemberAddress.Id == selectAddressId);
                }
            }

            var tts = await queryList.Select(t => new MemberAddressAndAera()
            {
                MemberAddress = t.MemberAddress,
                Province = t.Province.FirstOrDefault(),
                City = t.City.FirstOrDefault(),
                District = t.District.FirstOrDefault()
            }).OrderByDescending(t => t.MemberAddress.IsDefault).FirstOrDefaultAsync();

            if (tts != null)
            {
                MemberAddressListItem item = tts.MemberAddress.MapTo<MemberAddressListItem>();
                item.ProvinceName = tts.Province?.Short_name;
                item.CityName = tts.City?.Short_name;
                item.DistrictName = tts.District?.Short_name;
                return item;
            }

            return null;
        }

        public async Task<OrderSubmitResult> SubmitOrder(OrderSubmitInfo info)
        {
            //1.先检查参数有效性
            info.MemberId = _weixinSession.MemberId;
            if (info.Type != 0 && info.Type != 1)
            {
                throw new UserFriendlyException("无效的订单.");
            }

            var productInfo = await _productRepository.FirstOrDefaultAsync(info.ProductId);
            if (productInfo == null || !productInfo.IsSale)
                throw new UserFriendlyException("商品信息不存在或已下架!");

            var grouponInfo = await _grouponRepository.FirstOrDefaultAsync(info.GrouponId);
            if (info.Type == 0)
            {
                if (grouponInfo == null)
                    throw new UserFriendlyException("拼团信息不存在或已下架!");

                if (grouponInfo.StartTime > DateTime.Now)
                    throw new UserFriendlyException("拼团还未开始!");

                if (grouponInfo.ExpireTime < DateTime.Now)
                    throw new UserFriendlyException("拼团已结束!");
            }

            var memberInfo = await _memberRepository.FirstOrDefaultAsync(info.MemberId);
            if (memberInfo == null)
                throw new UserFriendlyException("用户信息不存在!");

            var query = _memberAddressRepository.GetAll();
            var where = FilterExpression.FindByGroup<MemberAddress>(new List<FilterCondition>());


            var queryList = query.Where(where)
                .GroupJoin(_areaRepository.GetAll(), left => left.ProvinceId, province => province.Id,
                    (left, province) => new
                    {
                        MemberAddress = left,
                        Province = province
                    })
                .GroupJoin(_areaRepository.GetAll(), left => left.MemberAddress.CityId, city => city.Id,
                    (left, city) => new
                    {
                        MemberAddress = left.MemberAddress,
                        Province = left.Province,
                        City = city
                    })
               .GroupJoin(_areaRepository.GetAll(), left => left.MemberAddress.DistrictId, cistrict => cistrict.Id,
                    (left, cistrict) => new
                    {
                        MemberAddress = left.MemberAddress,
                        Province = left.Province,
                        City = left.City,
                        District = cistrict
                    });

            queryList = queryList.Where(t => t.MemberAddress.MemberId == memberInfo.Id && t.MemberAddress.Id == info.AddressId);
            var addressInfo = await queryList.FirstOrDefaultAsync();
            if (addressInfo == null)
            {
                throw new UserFriendlyException("用户地址不存在!");
            }

            //检查如果是参团，一个团不允许参与2次
            if (info.Type == (int)OrderType.Groupon && info.GrouponOrderId != 0)
            {
                if (await _groupMemberRepository.CountAsync(t => t.MemberId == memberInfo.Id && t.GrouponOrderId == info.GrouponOrderId) > 0)
                    throw new UserFriendlyException("一个团只能参与一次.");
            }


            //2.先插入订单
            Order order = new Order();
            order.OrderNo = "D" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            order.MemberId = memberInfo.Id;
            if (info.Type == 0)
            {
                order.Amount = grouponInfo.Price * info.Num;
                order.GrouponId = grouponInfo.Id;
            }
            if (info.Type == 1)
            {
                //if (string.IsNullOrEmpty(memberInfo.BindRooms))
                order.Amount = productInfo.SalePrice * info.Num;
                //else
                //{
                //    order.Amount = productInfo.MemberPrice * info.Num;
                //}
                order.GrouponId = grouponInfo.Id;
            }
            order.Type = (OrderType)info.Type;
            order.Status = OrderStatus.WaitingPay;
            order.GrouponStatus = GrouponStatus.None;
            order.Address = addressInfo.Province.FirstOrDefault()?.Short_name +
                            addressInfo.City.FirstOrDefault()?.Short_name +
                            addressInfo.District.FirstOrDefault()?.Short_name +
                            addressInfo.MemberAddress.Address;
            order.ProvinceId = addressInfo.MemberAddress.ProvinceId;
            order.CityId = addressInfo.MemberAddress.CityId;
            order.DistrictId = addressInfo.MemberAddress.DistrictId;
            order.Contact = addressInfo.MemberAddress.Contact;
            order.Mobile = addressInfo.MemberAddress.Mobile;
            order.JoinGrouponId = info.GrouponOrderId;
            order.ClientRemark = info.ClientRemark;

            var orderId = await _orderRepository.InsertAndGetIdAsync(order);

            //3.插入订单商品

            OrderProduct orderProduct = new OrderProduct();
            orderProduct.OrderId = order.Id;
            orderProduct.ProductId = productInfo.Id;
            if (order.Type == OrderType.Groupon)
                orderProduct.GrouponPrice = grouponInfo.Price;
            orderProduct.Name = productInfo.Name;
            orderProduct.Specification = productInfo.Specification;
            orderProduct.Unit = productInfo.Unit;
            orderProduct.Price = productInfo.Price;
            orderProduct.SalePrice = productInfo.SalePrice;
            orderProduct.MemberPrice = productInfo.MemberPrice;
            orderProduct.Thumbnail = productInfo.Thumbnail;
            orderProduct.Summary = productInfo.Summary;
            orderProduct.Num = info.Num;


            await _orderProductRepository.InsertAsync(orderProduct);

            OrderSubmitResult result = new OrderSubmitResult();
            result.OrderNo = order.OrderNo;
            result.Amount = order.Amount;
            result.OrderId = orderId;
            return result;
        }


        public void PayOrder(OrderPayInfo payInfo)
        {
            //1.查出订单信息
            //2.保存支付信息，更新订单状态
            //3.如果是拼团，开团/或者参团
            //4.参团检查是否拼团成功

            var orderInfo = _orderRepository.FirstOrDefault(t => t.OrderNo == payInfo.OrderNo);
            if (orderInfo == null)
                throw new UserFriendlyException("订单不存在!");

            var groupInfo = _grouponRepository.FirstOrDefault(t => t.Id == orderInfo.GrouponId);
            if (groupInfo == null && orderInfo.Type == OrderType.Groupon)
                throw new UserFriendlyException("拼团信息不存在!");

            var oldpayInfo = _orderPayRepository.FirstOrDefault(t => t.OrderId == orderInfo.Id);
            if (oldpayInfo != null)
            {
                Logger.Info($"订单:{payInfo.OrderNo}已经支付");
                return;
            }

            var orderPay = new OrderPay();
            orderPay.OrderId = orderInfo.Id;
            orderPay.Channel = payInfo.Channel;
            orderPay.Money = payInfo.Money;
            orderPay.TradeNo = payInfo.TradeNo;
            orderPay.PayTime = DateTime.Now;

            _orderPayRepository.Insert(orderPay);

            orderInfo.IsPay = true;
            switch (orderInfo.Type)
            {
                case OrderType.Normal:
                    {
                        orderInfo.Status = OrderStatus.IsReading;
                        _orderRepository.Update(orderInfo);
                    }

                    break;
                case OrderType.Groupon:

                    //判断是开团还是参团
                    if (orderInfo.JoinGrouponId == 0)
                    {
                        GrouponOrder grouponOrder = new GrouponOrder();
                        grouponOrder.GrouponId = orderInfo.GrouponId;
                        grouponOrder.ExpireTime = DateTime.Now.AddDays(groupInfo.ValidDay);
                        grouponOrder.GrouponStatus = GrouponStatus.Grouponing;
                        grouponOrder.HeaderId = orderInfo.MemberId;
                        grouponOrder.RequireCount = groupInfo.RequireCount;
                        grouponOrder.JoinCount = 1;
                        var grouponOrderId = _groupOrderRepository.InsertAndGetId(grouponOrder);

                        GrouponMember grouponMember = new GrouponMember();
                        grouponMember.IsHeader = true;
                        grouponMember.MemberId = orderInfo.MemberId;
                        grouponMember.GrouponOrderId = grouponOrder.Id;
                        _groupMemberRepository.Insert(grouponMember);

                        //开团
                        orderInfo.Status = OrderStatus.Grouponing;
                        orderInfo.GrouponStatus = GrouponStatus.Grouponing;
                        orderInfo.JoinGrouponId = grouponOrderId;
                        _orderRepository.Update(orderInfo);

                        UnitOfWorkManager.Current.Completed += (sender, args) =>
                        {
                            new Thread(() => EventBus.Trigger(new GrouponCreateEventData { GrouponOrderId = grouponOrderId })).Start();
                        };
                    }
                    else
                    {
                        //参团
                        var grouponOrder = _groupOrderRepository.FirstOrDefault(t => t.Id == orderInfo.JoinGrouponId);
                        if (grouponOrder == null)
                            throw new UserFriendlyException("拼团订单不存在");

                        GrouponMember grouponMember = new GrouponMember();
                        grouponMember.MemberId = orderInfo.MemberId;
                        grouponMember.GrouponOrderId = grouponOrder.Id;
                        _groupMemberRepository.Insert(grouponMember);

                        grouponOrder.JoinCount++;
                        if (grouponOrder.JoinCount >= grouponOrder.RequireCount)
                        {
                            grouponOrder.GrouponSuccessTime = DateTime.Now;
                            grouponOrder.GrouponStatus = GrouponStatus.GrouponSuccess;

                            orderInfo.GrouponStatus = GrouponStatus.GrouponSuccess;
                            orderInfo.Status = OrderStatus.IsReading;

                            //找出对应该拼团的所有订单，修改状态

                            var ret = _sqlExecuter.Execute("update orders set Status=" + (int)OrderStatus.IsReading +
                                                  ",GrouponStatus=" + (int)GrouponStatus.GrouponSuccess +
                                                  " where JoinGrouponId=" + orderInfo.JoinGrouponId);

                        }
                        else
                        {
                            orderInfo.Status = OrderStatus.Grouponing;
                            orderInfo.GrouponStatus = GrouponStatus.Grouponing;
                        }

                        _groupOrderRepository.Update(grouponOrder);
                        _orderRepository.Update(orderInfo);
                        UnitOfWorkManager.Current.Completed += (sender, args) =>
                        {
                            new Thread(() => EventBus.Trigger(new GrouponJoinEventData { GrouponOrderId = orderInfo.JoinGrouponId, JoinMemberId = orderInfo.MemberId })).Start();
                        };


                    }
                    break;
            }

            var productInfo = _productRepository.FirstOrDefault(t => t.Id == groupInfo.ProductId);
            if (productInfo != null)
                _sqlExecuter.Execute("update Products set SellCount=SellCount+1 where id=" + productInfo.Id);

        }

        public async Task<GrouponOrderInfo> GetGrouponOrder(long id)
        {
            var info = await _groupOrderRepository.GetAll()
                .GroupJoin(_grouponRepository.GetAll(), left => left.GrouponId, right => right.Id,
                    (left, right) => new
                    {
                        GrouponOrder = left,
                        Groupon = right
                    })
                .GroupJoin(_productRepository.GetAll(), left => left.Groupon.FirstOrDefault().ProductId,
                    right => right.Id,
                    (left, right) => new
                    {
                        GrouponOrder = left.GrouponOrder,
                        Groupon = left.Groupon,
                        Product = right
                    })
                .Where(t => t.GrouponOrder.Id == id)
                .FirstOrDefaultAsync();

            if (info == null)
                throw new UserFriendlyException("拼团订单不存在!");

            GrouponOrderInfo grouponOrderInfo = new GrouponOrderInfo();
            grouponOrderInfo.ExpireTime = info.GrouponOrder.ExpireTime;
            grouponOrderInfo.GrouponStatus = info.GrouponOrder.GrouponStatus;
            grouponOrderInfo.JoinCount = info.GrouponOrder.JoinCount;
            grouponOrderInfo.Price = (info.Groupon.FirstOrDefault()?.Price) ?? 0;
            grouponOrderInfo.ProductName = info.Product.FirstOrDefault()?.Name;
            grouponOrderInfo.Summary = info.Groupon.FirstOrDefault()?.Summary;
            grouponOrderInfo.RequireCount = info.GrouponOrder.RequireCount;
            grouponOrderInfo.Thumbnail = info.Product.FirstOrDefault()?.Thumbnail;
            grouponOrderInfo.Unit = info.Product.FirstOrDefault()?.Unit;
            grouponOrderInfo.GrouponId = info.GrouponOrder.GrouponId;
            grouponOrderInfo.Id = info.GrouponOrder.Id;


            grouponOrderInfo.JoinMembers = new List<GrouponOrderMemberInfo>();

            var members = await _groupMemberRepository.GetAll()
                .GroupJoin(_memberRepository.GetAll(), left => left.MemberId, right => right.Id,
                    (left, right) => new
                    {
                        GrouponMember = left,
                        Member = right
                    })
                 .Where(t => t.GrouponMember.GrouponOrderId == grouponOrderInfo.Id)
                .ToListAsync();

            foreach (var member in members)
            {
                grouponOrderInfo.JoinMembers.Add(new GrouponOrderMemberInfo()
                {
                    IsHeader = member.GrouponMember.IsHeader,
                    JoinTime = member.GrouponMember.CreationTime,
                    Name = member.Member.FirstOrDefault()?.Name,
                    Thumbnail = member.Member.FirstOrDefault()?.Thumbnail,
                    Id = (member.Member.FirstOrDefault()?.Id) ?? 0
                });
            }


            return grouponOrderInfo;
        }

        public async Task<OrderDetail> GetOrderDetail(long id)
        {
            var orderInfo = await _orderRepository.FirstOrDefaultAsync(t => t.Id == id);
            if (orderInfo == null)
                throw new UserFriendlyException("订单不存在!");

            OrderDetail orderDetail = new OrderDetail();
            orderDetail.Id = orderInfo.Id;
            orderDetail.Type = orderInfo.Type;
            orderDetail.Address = orderInfo.Address;
            orderDetail.Amount = orderInfo.Amount;
            orderDetail.Contact = orderInfo.Contact;
            orderDetail.CreationTime = orderInfo.CreationTime;
            orderDetail.GrouponId = orderInfo.GrouponId;
            orderDetail.GrouponOrderId = orderInfo.JoinGrouponId;
            orderDetail.GrouponStatus = orderInfo.GrouponStatus;
            orderDetail.Mobile = orderInfo.Mobile;
            orderDetail.OrderNo = orderInfo.OrderNo;
            orderDetail.Status = orderInfo.Status;
            orderDetail.MemberId = orderInfo.MemberId;
            orderDetail.ClientRemark = orderInfo.ClientRemark;
            orderDetail.RefundStatus = orderInfo.RefundStatus;

            var payinfo = await _orderPayRepository.FirstOrDefaultAsync(t => t.OrderId == orderInfo.Id);
            if (payinfo != null)
            {
                orderDetail.PayChannel = payinfo.Channel;
                orderDetail.RefundsPayTime = payinfo.RefundsPayTime;
                orderDetail.RefundsTradeNo = payinfo.RefundsTradeNo;
            }

            var productInfo = await _orderProductRepository.FirstOrDefaultAsync(t => t.OrderId == orderInfo.Id);
            if (productInfo != null)
            {
                orderDetail.ProductName = productInfo.Name;
                orderDetail.Num = productInfo.Num;
                orderDetail.Thumbnail = productInfo.Thumbnail;
            }

            return orderDetail;

        }

        public async Task<PageListResultDto<GrouponOrderListItem>> GetGrouponOrders(int type, GetPageListRequstDto dto)
        {

            string sql = @"
select OrderProducts.Name as ProductName,OrderProducts.Thumbnail,orders.Amount,GrouponOrders.RequireCount,
Orders.Id as OrderId,GrouponOrders.Id as GrouponOrderId,GrouponOrders.GrouponStatus as Status
from GrouponMembers left join GrouponOrders on GrouponMembers.GrouponOrderId=GrouponOrders.Id left join Orders on Orders.JoinGrouponId=GrouponOrders.Id left join OrderProducts on OrderProducts.OrderId=Orders.Id";

            sql += " where GrouponMembers.MemberId=" + _weixinSession.MemberId + " and Orders.MemberId=" + _weixinSession.MemberId;

            if (type != 0)
            {
                if (type == 1)
                    sql += " and GrouponOrders.GrouponStatus = " + (int)GrouponStatus.Grouponing;
                if (type == 2)
                    sql += " and  GrouponOrders.GrouponStatus = " + (int)GrouponStatus.GrouponSuccess;
                if (type == 3)
                    sql += " and  GrouponOrders.GrouponStatus in ('" + (int)GrouponStatus.GrouponFail + "','" +
                           (int)GrouponStatus.GrouponFail + "')";
            }

            sql += " order by GrouponMembers.CreationTime desc";

            var pageList = _sqlExecuter.SqlQuery<GrouponOrderListItem>(sql).ToList();

            return new PageListResultDto<GrouponOrderListItem>(10, pageList, dto.CurrentPage, dto.PageSize);
        }

        public async Task<PageListResultDto<OrderListItem>> GetOrders(int type, GetPageListRequstDto dto)
        {
            var query = _orderRepository.GetAll();
            var where = FilterExpression.FindByGroup<Order>(dto.Filter);



            var queryCount = query.Where(where)
                 .GroupJoin(_orderProductRepository.GetAll(), left => left.Id, right => right.OrderId,
                   (left, right) => new
                   {
                       Order = left,
                       OrderProduct = right
                   });

            queryCount = queryCount.Where(t => t.Order.MemberId == _weixinSession.MemberId);

            if (type > 0)
            {
                if (type == 1)
                    queryCount = queryCount.Where(t => t.Order.Status == OrderStatus.WaitingPay);
                if (type == 2)
                    queryCount = queryCount.Where(t => t.Order.Status == OrderStatus.Grouponing);
                if (type == 3)
                    queryCount = queryCount.Where(t => t.Order.Status == OrderStatus.IsReading);
                if (type == 4)
                    queryCount = queryCount.Where(t => t.Order.Status == OrderStatus.HasShip);
                if (type == 5)
                    queryCount = queryCount.Where(t => t.Order.Status == OrderStatus.Received);
            }

            int count = await queryCount.CountAsync();

            var queryList = query.Where(where)
               .GroupJoin(_orderProductRepository.GetAll(), left => left.Id, right => right.OrderId,
                   (left, right) => new
                   {
                       Order = left,
                       OrderProduct = right
                   });


            queryList = queryList.Where(t => t.Order.MemberId == _weixinSession.MemberId);


            if (type > 0)
            {
                if (type == 1)
                    queryList = queryList.Where(t => t.Order.Status == OrderStatus.WaitingPay);
                if (type == 2)
                    queryList = queryList.Where(t => t.Order.Status == OrderStatus.Grouponing);
                if (type == 3)
                    queryList = queryList.Where(t => t.Order.Status == OrderStatus.IsReading);
                if (type == 4)
                    queryList = queryList.Where(t => t.Order.Status == OrderStatus.HasShip);
                if (type == 5)
                    queryList = queryList.Where(t => t.Order.Status == OrderStatus.Received);
            }



            var tts = await queryList.OrderByDescending(t => t.Order.CreationTime).PageBy(dto).ToListAsync();
            var pageList = new List<OrderListItem>();
            foreach (var tt in tts)
            {
                pageList.Add(new OrderListItem()
                {
                    Amount = tt.Order.Amount,
                    GrouponStatus = tt.Order.GrouponStatus,
                    OrderId = tt.Order.Id,
                    ProductName = tt.OrderProduct.FirstOrDefault().Name,
                    Status = tt.Order.Status,
                    Type = tt.Order.Type,
                    Thumbnail = tt.OrderProduct.FirstOrDefault().Thumbnail,
                    OrderNo = tt.Order.OrderNo,
                    RefundStatus=tt.Order.RefundStatus
                });
            }

            return new PageListResultDto<OrderListItem>(count, pageList, dto.CurrentPage, dto.PageSize);
        }


        public async Task ConfirmReceived(long id)
        {
            var shipInfo = await _orderShipRepository.FirstOrDefaultAsync(t => t.OrderId == id);
            Order orderInfo = await _orderRepository.FirstOrDefaultAsync(t => t.Id == id);
            if (orderInfo != null && orderInfo.Status == OrderStatus.HasShip && shipInfo != null)
            {
                if (orderInfo.MemberId != _weixinSession.MemberId)
                    throw new UserFriendlyException("你没有权限操作此订单.");

                orderInfo.Status = OrderStatus.Received;
                await _orderRepository.UpdateAsync(orderInfo);

                shipInfo.ReceivedTime = DateTime.Now;
                shipInfo.ReceivedType = OrderReceivedType.User;

                await _orderShipRepository.UpdateAsync(shipInfo);
            }
        }

        public async Task CancelOrder(long id)
        {
            Order orderInfo = await _orderRepository.FirstOrDefaultAsync(t => t.Id == id);
            if (orderInfo == null)
                throw new UserFriendlyException("订单不存在!");
            if (orderInfo.MemberId != _weixinSession.MemberId)
                throw new UserFriendlyException("你没有权限操作此订单.");

            if (orderInfo.Status == OrderStatus.WaitingPay)
            {
                //对应未支付的订单，直接关闭订单，没有其他流程
                orderInfo.Status = OrderStatus.Cancel;
                await _orderRepository.UpdateAsync(orderInfo);
            }

            if (orderInfo.Status == OrderStatus.Grouponing || orderInfo.Status == OrderStatus.IsReading)
            {
                //团购中,已成团/已支付的订单 状态设置为 退款申请中，并产生售后纪录
            }

        }

        public async Task OrderComment(OrderCommentInfo info)
        {
            if (info.Content.Length < 5 || info.Content.Length > 100)
            {
                throw new UserFriendlyException("评论内容长度请在5~100之间");
            }
            Order orderInfo = await _orderRepository.FirstOrDefaultAsync(t => t.Id == info.OrderId);
            if (orderInfo == null)
                throw new UserFriendlyException("订单不存在!");

            var productInfo = await _orderProductRepository.FirstOrDefaultAsync(t => t.OrderId == orderInfo.Id);
            if (productInfo == null)
            {
                throw new UserFriendlyException("商品信息不存在!");
            }

            ProductComment productComment = new ProductComment();
            productComment.Content = info.Content;
            productComment.MemberId = _weixinSession.MemberId;
            productComment.ProductId = productInfo.ProductId;
            productComment.SourceOrderId = orderInfo.Id;

            orderInfo.Status = OrderStatus.Done;

            await _orderRepository.UpdateAsync(orderInfo);
            await _productCommentRepository.InsertAsync(productComment);
        }

        public async Task<PageListResultDto<GrouponDetail.CommentItem>> GetProductComments(long productId, GetPageListRequstDto dto)
        {

            var query = _productCommentRepository.GetAll();
            var where = FilterExpression.FindByGroup<ProductComment>(dto.Filter);


            var queryCount = query.Where(where)
                .GroupJoin(_memberRepository.GetAll(), left => left.MemberId, right => right.Id, (left, right) => new
                {
                    ProductComment = left,
                    Member = right
                });

            queryCount = queryCount.Where(t => t.ProductComment.ProductId == productId);
            int count = await queryCount.CountAsync();




            var queryList = query.Where(where)
             .GroupJoin(_memberRepository.GetAll(), left => left.MemberId, right => right.Id, (left, right) => new
             {
                 ProductComment = left,
                 Member = right
             });


            queryList = queryList.Where(t => t.ProductComment.ProductId == productId);


            var comments = await queryList.OrderByDescending(t => t.ProductComment.CreationTime).PageBy(dto).ToListAsync();


            List<GrouponDetail.CommentItem> Comments = comments.Select(comment => new GrouponDetail.CommentItem()
            {
                Name = comment.Member.FirstOrDefault()?.Name,
                Thumbnail = comment.Member.FirstOrDefault()?.Thumbnail,
                Content = comment.ProductComment.Content,
                CreationTime = comment.ProductComment.CreationTime
            }).ToList();

            return new PageListResultDto<GrouponDetail.CommentItem>(count, Comments, dto.CurrentPage, dto.PageSize);
        }

        public async Task LikeProduct(long productId)
        {
            if (
                await
                    _productLikeRepository.FirstOrDefaultAsync(
                        t => t.MemberId == _weixinSession.MemberId && t.ProductId == productId) == null)
            {
                ProductLike productLike = new ProductLike();
                productLike.ProductId = productId;
                productLike.MemberId = _weixinSession.MemberId;
                await _productLikeRepository.InsertAsync(productLike);
            }
        }

        public async Task UnLikeProduct(long productId)
        {
            var info = await _productLikeRepository.FirstOrDefaultAsync(
                    t => t.MemberId == _weixinSession.MemberId && t.ProductId == productId);
            if (info != null)
            {
                await _productLikeRepository.DeleteAsync(info);
            }

        }

        public async Task<PageListResultDto<GrouponItem>> GetMyLikeGrouponItems(GetPageListRequstDto dto)
        {
            var query = _grouponRepository.GetAll();
            var where = FilterExpression.FindByGroup<Groupon>(dto.Filter);

            var queryCount = query.Where(where)
                .GroupJoin(_productRepository.GetAll(), left => left.ProductId, right => right.Id,
                    (left, right) => new
                    {
                        Groupon = left,
                        Product = right
                    })
                .GroupJoin(_productLikeRepository.GetAll(), left => left.Product.FirstOrDefault().Id,
                    right => right.ProductId,
                    (left, right) => new
                    {
                        Groupon = left.Groupon,
                        Product = left.Product,
                        ProductLike = right
                    });

            queryCount = queryCount.Where(t => t.Groupon.IsSale && t.Product.FirstOrDefault().IsSale && t.ProductLike.FirstOrDefault().MemberId == _weixinSession.MemberId);

            int count = await queryCount.CountAsync();

            var queryList = query.Where(where)
               .GroupJoin(_productRepository.GetAll(), left => left.ProductId, right => right.Id,
                   (left, right) => new
                   {
                       Groupon = left,
                       Product = right
                   })
                   .GroupJoin(_productLikeRepository.GetAll(), left => left.Product.FirstOrDefault().Id,
                    right => right.ProductId,
                    (left, right) => new
                    {
                        Groupon = left.Groupon,
                        Product = left.Product,
                        ProductLike = right
                    });




            queryList = queryList.Where(t => t.Groupon.IsSale && t.Product.FirstOrDefault().IsSale && t.ProductLike.FirstOrDefault().MemberId == _weixinSession.MemberId);

            var tts = await queryList.Select(t => new GrouponAndProduct()
            {
                Groupon = t.Groupon,
                Product = t.Product.FirstOrDefault()
            }).OrderByDescending(t => t.Groupon.CreationTime).PageBy(dto).ToListAsync();

            var grouponListDtos = tts.Select(
                result =>
                {
                    var grouponInfo = result.Groupon.MapTo<GrouponItem>();
                    grouponInfo.Name = result.Product?.Name;
                    grouponInfo.Unit = result.Product?.Unit;
                    grouponInfo.Thumbnail = result.Product?.Thumbnail;
                    if (result.Product?.SellCount != null)
                        grouponInfo.SellCount = (result.Product?.SellCount).Value;
                    return grouponInfo;
                }).ToList();

            var pageList = grouponListDtos.MapTo<List<GrouponItem>>();
            return new PageListResultDto<GrouponItem>(count, pageList, dto.CurrentPage, dto.PageSize);
        }

        public async Task<long> RefundApply(RefundApplyDto dto)
        {
            Order orderInfo = await _orderRepository.FirstOrDefaultAsync(t => t.Id == dto.Id);
            if (orderInfo == null)
                throw new UserFriendlyException("订单不存在!");
            if (orderInfo.MemberId != _weixinSession.MemberId)
                throw new UserFriendlyException("你没有权限操作此订单.");
            if (orderInfo.RefundStatus != RefundStatus.None)
                throw new UserFriendlyException("当前订单状态不允许此操作!");

            var orderProduct = await _orderProductRepository.FirstOrDefaultAsync(t => t.OrderId == orderInfo.Id);
            if (orderProduct == null)
                throw new UserFriendlyException("订单商品不存在!");

            RefundOrder refundOrder = new RefundOrder();
            refundOrder.Amount = orderInfo.Amount;
            refundOrder.OrderId = orderInfo.Id;
            refundOrder.Reason = RefundReason.Other;
            refundOrder.RefundMoney = orderInfo.Amount;
            refundOrder.ProductId = orderProduct.ProductId;
            refundOrder.Specification = orderProduct.Specification;
            refundOrder.Unit = orderProduct.Unit;
            refundOrder.MemberId = orderInfo.MemberId;
            refundOrder.Name = orderProduct.Name;
            refundOrder.Mobile = orderInfo.Mobile;
            refundOrder.Num = orderProduct.Num;
            refundOrder.OrderNo = orderInfo.OrderNo;
            refundOrder.Price = orderProduct.Price;
            refundOrder.Specification = orderProduct.Specification;
            refundOrder.Thumbnail = orderProduct.Thumbnail;
            refundOrder.Type = (RefundType)dto.Type;
            refundOrder.Status = RefundStatus.Processing;
            refundOrder.Remark = dto.Remark;

             await _refundOrderRepository.InsertAsync(refundOrder);

            foreach (var image in dto.Images)
            {
                RefundOrderImage refundOrderImage = new RefundOrderImage();
                refundOrderImage.RefundOrderId = refundOrder.Id;
                refundOrderImage.Thumbnail = image;
               await _refundOrderImageRepository.InsertAsync(refundOrderImage);
            }

            orderInfo.RefundStatus = RefundStatus.Processing;
            await _orderRepository.UpdateAsync(orderInfo);

            return refundOrder.Id;

        }

        public GetEventCountResult GetEventCount(string Type)
        {
            return _wuyeApiAppSrvice.GetEventCount(Type, _weixinSession.MemberId.ToString());
        }

        public GetEventItemResult GetEventItem(string Type, string EType, string EDType, int PageIndex, int PageSize)
        {
            return _wuyeApiAppSrvice.GetEventItem(Type, _weixinSession.MemberId.ToString(), EType, EDType, PageIndex,
                PageSize);
        }

        public GetEventInfoResult GetEventInfo(string EType, string EDType, string BillCode)
        {
            return _wuyeApiAppSrvice.GetEventInfo(EType, EDType, BillCode);
        }

        public InvokeResultDto RetrievePassWord(string Phone, string NewPwd, string VerCode)
        {
            NewPwd = MD5Helper.EnCode(NewPwd).ToLower();
            return _wuyeApiAppSrvice.RetrievePassWord(Phone, NewPwd, VerCode);
        }

        public SearchHousingManagementResult SearchHousingManagement(string PRoomCode)
        {
            return _wuyeApiAppSrvice.SearchHousingManagement(PRoomCode);
        }

        public GetPProjectListResult GetPProjectList()
        {
            return _wuyeApiAppSrvice.GetPProjectList();
        }

        public GetPBuildingListResult GetPBuildingList(string pProjectCode)
        {
            return _wuyeApiAppSrvice.GetPBuildingList(pProjectCode);
        }

        public GetPUnitListResult GetPUnitList(string pProjectCode, string pBuildingCode)
        {
            return _wuyeApiAppSrvice.GetPUnitList(pProjectCode, pBuildingCode);
        }

        public GetPFloorListByUnitResult GetPFloorListByUnit(string pProjectCode, string pBuildingCode, string PUnitCode)
        {
            return _wuyeApiAppSrvice.GetPFloorListByUnit(pProjectCode, pBuildingCode, PUnitCode);
        }

        public GetPRoomListByFloorResult GetPRoomListByFloor(string pProjectCode, string pBuildingCode, string PUnitCode,
            string pFloorName)
        {
            return _wuyeApiAppSrvice.GetPRoomListByFloor(pProjectCode, pBuildingCode, PUnitCode, pFloorName);
        }

        public InvokeResultDto AddLinkBillMemberMSG(string billcode, string chartinfo)
        {
            return _wuyeApiAppSrvice.AddLinkBillMemberMSG(billcode, _weixinSession.MemberId.ToString(), chartinfo);
        }

        public InvokeResultDto UpdateLinkBillEvaluation(string billcode, string evalLevel, string suggest)
        {
            return _wuyeApiAppSrvice.UpdateLinkBillEvaluation(billcode, evalLevel, suggest);
        }

        public long GetFeeService(string projectCode)
        {
            var projectInfo = _projectRepository.FirstOrDefault(t => t.Code == projectCode);
            if (projectInfo == null)
                return 0;


            var query = _feeServiceRepository.GetAll();

            var data =
                query.GroupJoin(_feeServiceProjectRepository.GetAll(), left => left.Id, right => right.FeeServiceId,
                    (left, right) => new
                    {
                        FeeService = left,
                        FeeServiceProjects = right
                    })
                    .Where(
                        t =>
                            t.FeeService.AllProjects ||
                            t.FeeServiceProjects.FirstOrDefault().ProjectId == projectInfo.Id)
                    .OrderBy(t => t.FeeService.CreationTime)
                    .FirstOrDefault();
            if (data != null)
                return data.FeeService.Id;

            return 0;
        }

        public async Task<FeeServiceItemDto> GetFeeServiceForView(long id)
        {
            var news = await _feeServiceRepository.GetAsync(id);
            var dto = news.MapTo<FeeServiceItemDto>();
            dto.Content = _detailManager.Get(DetailType.FeeService, news.Id);
            return dto;
        }

        public async Task<RefundViewDto> GetRefund(long id)
        {
            var order = await _refundOrderRepository.FirstOrDefaultAsync(t => t.OrderId == id);
            if (order == null)
                throw new UserFriendlyException("退款信息不存在!");
            return order.MapTo<RefundViewDto>();
        }

        public async Task<PageListResultDto<RefundViewDto>> GetRefundOrders(int type, GetPageListRequstDto dto)
        {
            var query = _refundOrderRepository.GetAll();
            var where = FilterExpression.FindByGroup<RefundOrder>(dto.Filter);
            var queryCount = query.Where(t => t.MemberId == _weixinSession.MemberId);

            if (type == 1)
            {
               queryCount = queryCount.Where(t=>t.Status==RefundStatus.Processing || t.Status == RefundStatus.Accept);
            }

            int count = await queryCount.CountAsync();
            var queryList = query.Where(t => t.MemberId == _weixinSession.MemberId);

            if (type == 1)
            {
                queryList = queryList.Where(t => t.Status == RefundStatus.Processing || t.Status == RefundStatus.Accept);
            }



            var tts = await queryList.OrderByDescending(t => t.CreationTime).PageBy(dto).ToListAsync();
        
            var pageList = tts.MapTo<List<RefundViewDto>>();

            return new PageListResultDto<RefundViewDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }
    }
}
