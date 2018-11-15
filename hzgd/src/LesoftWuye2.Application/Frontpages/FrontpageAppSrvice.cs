using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using LesoftWuye2.Application.Activitys.Dto;
using LesoftWuye2.Application.Estateinfos.Dto;
using LesoftWuye2.Application.Frontpages.Dto;
using LesoftWuye2.Application.Frontpages.Session;
using LesoftWuye2.Application.LifeInfos.Dto;
using LesoftWuye2.Application.LifeInfoTypes.Dto;
using LesoftWuye2.Application.Newss.Dto;
using LesoftWuye2.Application.Notices.Dto;
using LesoftWuye2.Application.Rentsaleinfos.Dto;
using LesoftWuye2.Application.ServiceTels.Dto;
using LesoftWuye2.Application.Substations.Dto;
using LesoftWuye2.Application.YsPay;
using LesoftWuye2.Core.ActivityProjects;
using LesoftWuye2.Core.Activitys;
using LesoftWuye2.Core.Details;
using LesoftWuye2.Core.Estateinfos;
using LesoftWuye2.Core.EstateinfoTypes;
using LesoftWuye2.Core.LifeInfoProjects;
using LesoftWuye2.Core.LifeInfos;
using LesoftWuye2.Core.LifeInfoTypes;
using LesoftWuye2.Core.NewsProjects;
using LesoftWuye2.Core.Newss;
using LesoftWuye2.Core.NoticeProjects;
using LesoftWuye2.Core.Notices;
using LesoftWuye2.Core.Rentsaleinfos;
using LesoftWuye2.Core.RentsaleinfoTypes;
using LesoftWuye2.Core.ServiceTels;
using LesoftWuye2.Core.SlideImages;
using LesoftWuye2.Core.Substations;
using Obs.Dto;
using Obs.Filter;

namespace LesoftWuye2.Application.Frontpages
{
    public class FrontpageAppSrvice : LesoftWuye2AppServiceBase, IFrontpageAppSrvice
    {
        private readonly IFrontSession _frontSession;
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

        

        public FrontpageAppSrvice(IFrontSession frontSession,
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
            IRepository<RentsaleinfoImage, long> rentsaleinfoImageRepository

            

            )
        {
            _frontSession = frontSession;
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
                     }).OrderByDescending(t => t.Notice.CreationTime).FirstOrDefault(t => t.Notice.AllProjects || t.NoticeProjects.FirstOrDefault().ProjectId == _frontSession.ProjectId);
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
                    }).OrderByDescending(t => t.Activity.CreationTime).FirstOrDefault(t => t.Activity.AllProjects || t.ActivityProjects.FirstOrDefault().ProjectId == _frontSession.ProjectId);
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
                  }).OrderByDescending(t => t.News.CreationTime).FirstOrDefault(t => t.News.AllProjects || t.NewsProjects.FirstOrDefault().ProjectId == _frontSession.ProjectId);
            if (topNews != null)
            {
                homeData.News = new HomeDataDto.ArticleItem() { Id = topNews.News.Id, Title = topNews.News.Title, Thumbnail = topNews.News.Thumbnail, CreationTime = topNews.News.CreationTime };
            }


            return homeData;
        }

        public virtual async Task<PageListResultDto<NoticeListDto>> GetNotices(GetPageListRequstDto dto)
        {
            var query = _noticeRepository.GetAll();
            var count = await query.GroupJoin(_noticeProjectRepository.GetAll(), left => left.Id, right => right.NoticeId,
                    (left, right) => new
                    {
                        Notice = left,
                        NoticeProjects = right
                    }).CountAsync(t => t.Notice.AllProjects || t.NoticeProjects.FirstOrDefault().ProjectId == _frontSession.ProjectId);

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
                            t.NoticeProjects.FirstOrDefault().ProjectId == _frontSession.ProjectId)
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
                    }).CountAsync(t => t.Activity.AllProjects || t.ActivityProjects.FirstOrDefault().ProjectId == _frontSession.ProjectId);

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
                            t.ActivityProjects.FirstOrDefault().ProjectId == _frontSession.ProjectId)
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
            dto.MemberId = _frontSession.MemberId.ToString();
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

        public virtual async Task<PageListResultDto<ActivityListDto>> GetMyActivitys(string memberId, GetPageListRequstDto dto)
        {
            //获取用户参加的活动
            
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

            PageListResultDto<ActivityListDto> result = new PageListResultDto<ActivityListDto>(count, items.ToArray(),dto.PageSize,dto.CurrentPage);
            return result;
        }

        public virtual async Task<bool> CheckIsJoinActivity(long id, string memberId)
        {
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
                    }).CountAsync(t => t.News.AllProjects || t.NewsProjects.FirstOrDefault().ProjectId == _frontSession.ProjectId);

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
                            t.NewsProjects.FirstOrDefault().ProjectId == _frontSession.ProjectId)
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
                    }).CountAsync(t => ((t.LifeInfo.AllProjects || t.LifeInfoProjects.FirstOrDefault().ProjectId == _frontSession.ProjectId) && t.LifeInfo.LifeInfoTypeId == lifeInfoTypeId));

            var list =
                await query.GroupJoin(_lifeInfoProjectRepository.GetAll(), left => left.Id, right => right.LifeInfoId,
                    (left, right) => new
                    {
                        LifeInfo = left,
                        LifeInfoProjects = right
                    })
                    .Where(
                        t => ((t.LifeInfo.AllProjects || t.LifeInfoProjects.FirstOrDefault().ProjectId == _frontSession.ProjectId) && t.LifeInfo.LifeInfoTypeId == lifeInfoTypeId))
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
            var estateinfoComment = input.MapTo<EstateinfoComment>();
            await _estateinfoCommentRepository.InsertAsync(estateinfoComment);
        }

        public virtual async Task<EstateinfoMyInfoDto> GetEstateinfoMyInfo(string memberId)
        {
            EstateinfoMyInfoDto result = new EstateinfoMyInfoDto();
            result.UnShow = await _estateinfoRepository.CountAsync(t => t.MemberId == memberId && !t.IsShow);
            result.IsSale = await _estateinfoRepository.CountAsync(t => t.MemberId == memberId && t.IsShow && t.IsSale);
            result.UnSale = await _estateinfoRepository.CountAsync(t => t.MemberId == memberId && t.IsShow && !t.IsSale);
            return result;
        }

        public virtual async Task<PageListResultDto<EstateinfoListDto>> GetMyEstateinfos(string memberId, int type, GetPageListRequstDto dto)
        {
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

        public virtual async Task SetEstateinfoSale(string memberId, long id)
        {
            var info = await _estateinfoRepository.GetAsync(id);
            if (info == null || info.MemberId != memberId)
                throw new UserFriendlyException("你无权操作此功能!");
            info.IsSale = true;
            await _estateinfoRepository.UpdateAsync(info);
        }

        public virtual async Task SetEstateinfoUnSale(string memberId, long id)
        {
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


        public virtual async Task<RentsaleinfoMyInfoDto> GetRentsaleinfoMyInfo(string memberId)
        {
            RentsaleinfoMyInfoDto result = new RentsaleinfoMyInfoDto();
            result.UnShow = await _rentsaleinfoRepository.CountAsync(t => t.MemberId == memberId && !t.IsShow);
            result.IsSale = await _rentsaleinfoRepository.CountAsync(t => t.MemberId == memberId && t.IsShow && t.IsSale);
            result.UnSale = await _rentsaleinfoRepository.CountAsync(t => t.MemberId == memberId && t.IsShow && !t.IsSale);
            return result;
        }

        public virtual async Task<PageListResultDto<RentsaleinfoListDto>> GetMyRentsaleinfos(string memberId, int type, GetPageListRequstDto dto)
        {
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

        public virtual async Task SetRentsaleinfoSale(string memberId, long id)
        {
            var info = await _rentsaleinfoRepository.GetAsync(id);
            if (info == null || info.MemberId != memberId)
                throw new UserFriendlyException("你无权操作此功能!");
            info.IsSale = true;
            await _rentsaleinfoRepository.UpdateAsync(info);
        }

        public virtual async Task SetRentsaleinfoUnSale(string memberId, long id)
        {
            var info = await _rentsaleinfoRepository.GetAsync(id);
            if (info == null || info.MemberId != memberId)
                throw new UserFriendlyException("你无权操作此功能!");
            info.IsSale = false;
            await _rentsaleinfoRepository.UpdateAsync(info);
        }

        public virtual async Task<MyCountInfo> GetMyCountInfo(string memberId)
        {
            MyCountInfo info=new MyCountInfo();

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
    }
}
