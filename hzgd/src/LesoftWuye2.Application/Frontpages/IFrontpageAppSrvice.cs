using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LesoftWuye2.Application.Activitys.Dto;
using LesoftWuye2.Application.Estateinfos.Dto;
using LesoftWuye2.Application.Frontpages.Dto;
using LesoftWuye2.Application.LifeInfos.Dto;
using LesoftWuye2.Application.Newss.Dto;
using LesoftWuye2.Application.Notices.Dto;
using LesoftWuye2.Application.Rentsaleinfos.Dto;
using LesoftWuye2.Application.Substations.Dto;
using LesoftWuye2.Application.WuyeApis.Dto;
using LesoftWuye2.Core.Estateinfos;
using Obs.Dto;

namespace LesoftWuye2.Application.Frontpages
{

    public interface IFrontpageAppSrvice : IApplicationService
    {
        #region 首页
        Task<HomeDataDto> GetHomeData(); 
        #endregion

        #region 社区公告

        Task<PageListResultDto<NoticeListDto>> GetNotices(GetPageListRequstDto dto);
        Task<NoticeItemDto> GetNotice(long id);

        #endregion

        #region 社区活动
        Task<PageListResultDto<ActivityListDto>> GetActivitys(GetPageListRequstDto dto);

        Task<ActivityItemDto> GetActivity(long id);

        Task JoinActivity(CreateActivityPersonInput dto);

        Task<PageListResultDto<ActivityListDto>> GetMyActivitys(string memberId, GetPageListRequstDto dto);

        Task<bool> CheckIsJoinActivity(long id, string memberId);

            #endregion

        #region 社区新闻
        Task<PageListResultDto<NewsListDto>> GetNewss(GetPageListRequstDto dto);

        Task<NewsItemDto> GetNews(long id);
        #endregion

        #region 服务电话
        Task<ServiceTelDto> GetServiceTel();
        #endregion

        #region 生活服务
        Task<LifeInfoNavDto> GetLifeInfoNav();

        Task<PageListResultDto<LifeInfoListDto>> GetLifeInfoList(GetPageListRequstDto dto, long lifeInfoTypeId);

        Task<LifeInfoItemDto> GetLifeInfo(long id);
        #endregion

        #region 轮播图片
        Task<List<string>> GetSlideImages();
        #endregion

        #region 关于我们

        Task<PageListResultDto<SubstationListDto>> GetSubstations(GetPageListRequstDto dto);

        Task<SubstationItemDto> GetSubstations(long id);

        #endregion

        #region 跳蚤市场

        Task<List<ComboboxItemDto>> GetEstateinfoTypesForCombo();

        Task CreateEstateinfo(CreateEstateinfoInput input);

        Task<PageListResultDto<EstateinfoListDto>> GetEstateinfos(int mode, GetPageListRequstDto dto);

        Task<EstateinfoItemDto> GetEstateinfo(long id);

        Task CreateEstateinfoComment(CreateEstateinfoCommentInput input);

        Task<EstateinfoMyInfoDto> GetEstateinfoMyInfo(string memberId);

        Task<PageListResultDto<EstateinfoListDto>> GetMyEstateinfos(string memberId,int type,GetPageListRequstDto dto);

        Task SetEstateinfoSale(string memberId,long id);

        Task SetEstateinfoUnSale(string memberId, long id);

        #endregion

        #region 租售信息

        Task<List<ComboboxItemDto>> GetRentsaleinfoTypesForCombo();

        Task CreateRentsaleinfo(CreateRentsaleinfoInput input);

        Task<PageListResultDto<RentsaleinfoListDto>> GetRentsaleinfos(int mode, GetPageListRequstDto dto);

        Task<RentsaleinfoItemDto> GetRentsaleinfo(long id);
        

        Task<RentsaleinfoMyInfoDto> GetRentsaleinfoMyInfo(string memberId);

        Task<PageListResultDto<RentsaleinfoListDto>> GetMyRentsaleinfos(string memberId, int type, GetPageListRequstDto dto);

        Task SetRentsaleinfoSale(string memberId, long id);

        Task SetRentsaleinfoUnSale(string memberId, long id);

        #endregion

        #region 统计个人信息
        Task<MyCountInfo> GetMyCountInfo(string memberId);
        #endregion

        #region 发起支付

        Task<string> CreatePay(); 

        #endregion
    }
}
