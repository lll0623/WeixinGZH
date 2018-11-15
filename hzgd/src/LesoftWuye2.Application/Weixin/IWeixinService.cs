using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LesoftWuye2.Application.Activitys.Dto;
using LesoftWuye2.Application.Estateinfos.Dto;
using LesoftWuye2.Application.FeeServices.Dto;
using LesoftWuye2.Application.Weixin.Dto;
using LesoftWuye2.Application.LifeInfos.Dto;
using LesoftWuye2.Application.Malls.Dto;
using LesoftWuye2.Application.Newss.Dto;
using LesoftWuye2.Application.Notices.Dto;
using LesoftWuye2.Application.Rentsaleinfos.Dto;
using LesoftWuye2.Application.Substations.Dto;
using LesoftWuye2.Application.WuyeApis.Dto;
using LesoftWuye2.Core.Estateinfos;
using LesoftWuye2.Core.MemberAddresss;
using LesoftWuye2.Core.Wuyebase.Members;
using Obs.Dto;

namespace LesoftWuye2.Application.Weixin
{

    public interface IWeixinService : IApplicationService
    {
        #region 首页
        Task<HomeDataDto> GetHomeData();

        #endregion

        bool Login(string username, string password);

        void Logout();

        #region 社区公告

        Task<PageListResultDto<NoticeListDto>> GetNotices(GetPageListRequstDto dto);
        Task<NoticeItemDto> GetNotice(long id);

        #endregion

        #region 社区活动

        Task<PageListResultDto<ActivityListDto>> GetActivitys(GetPageListRequstDto dto);

        Task<ActivityItemDto> GetActivity(long id);

        Task JoinActivity(CreateActivityPersonInput dto);

        Task<PageListResultDto<ActivityListDto>> GetMyActivitys(GetPageListRequstDto dto);

        Task<bool> CheckIsJoinActivity(long id);

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

        //Task<SubstationItemDto> GetSubstations(long id);

        #endregion

        #region 跳蚤市场

        Task<List<ComboboxItemDto>> GetEstateinfoTypesForCombo();

        Task CreateEstateinfo(CreateEstateinfoInput input);

        Task<PageListResultDto<EstateinfoListDto>> GetEstateinfos(int mode, GetPageListRequstDto dto);

        Task<EstateinfoItemDto> GetEstateinfo(long id);

        Task CreateEstateinfoComment(CreateEstateinfoCommentInput input);

        Task<EstateinfoMyInfoDto> GetEstateinfoMyInfo();

        Task<PageListResultDto<EstateinfoListDto>> GetMyEstateinfos(int type, GetPageListRequstDto dto);

        Task SetEstateinfoSale( long id);

        Task SetEstateinfoUnSale( long id);

        #endregion

        #region 租售信息

        Task<List<ComboboxItemDto>> GetRentsaleinfoTypesForCombo();

        Task CreateRentsaleinfo(CreateRentsaleinfoInput input);

        Task<PageListResultDto<RentsaleinfoListDto>> GetRentsaleinfos(int mode, GetPageListRequstDto dto);

        Task<RentsaleinfoItemDto> GetRentsaleinfo(long id);


        Task<RentsaleinfoMyInfoDto> GetRentsaleinfoMyInfo();

        Task<PageListResultDto<RentsaleinfoListDto>> GetMyRentsaleinfos(int type, GetPageListRequstDto dto);

        Task SetRentsaleinfoSale(long id);

        Task SetRentsaleinfoUnSale(long id);

        #endregion

        #region 统计个人信息
        Task<MyCountInfo> GetMyCountInfo(string memberId);
        #endregion

        #region 发起支付

        Task<string> CreatePay();

        #endregion


        #region 物业服务

        List<MemberRoomItem> GetMyRooms();
        AddLinkBillResult AddService(AddServiceDto dto);

        GetNoPayFeeByMemberResult GetNoPayFeeByMember(string PRoomCode);

        void SetCurrentRoomInfo(string PRoomCode);

        GetPRoomInfoByPhoneResult GetPRoomInfoByPhone(string Phone);

        GetPRoomInfoByPhoneResult GetPRoomInfoByQRCode(string content);

        GetPRoomInfoByPhoneResult GetPRoomInfoByUserPass(string username, string password);

        InvokeResultDto MemberAddPRoom(string pRoomCode,string role);

        InvokeResultDto MemberRemovePRoom(string pRoomCode);

        SendSMSResult SendSMS(string Phone, int Type);

        void Registered(string Phone, string Password, string Name, string NickName);

        Member GetMy();

        CreatePayBillResult CreatePayBill(string FeedIds,string PRoomCode, string Amount);

        GetPRoomPrePayItemResult GetPRoomPrePayItem(string pRoomCode);

        CreatePayOrderPrePayResult CreatePayOrderPrePay(string pRoomCode, string PrePayItems, string AmountSummary);

        CreateUserByWXResult CreateUserByWX(string phone, string nickname, string name);

        #endregion


        #region 商品

        Task<MallHomeData> GetMallHomeData();


        Task<List<ComboboxItemDto>> GetCategories();

        Task<PageListResultDto<GrouponItem>> GetGrouponItems(int cid, GetPageListRequstDto dto);

        Task<GrouponDetail> GetGrouponDetail(long id);
        
        Task<List<ComboboxItemDto>> GetProvinces();
        Task<List<ComboboxItemDto>> GetCities(long provinceId);
        Task<List<ComboboxItemDto>> GetDistricts(long cityId);

        Task AddAddress(MemberAddress address);

        Task EditAddress(MemberAddress address);

        Task<List<MemberAddressListItem>> GetAddresss();

        Task DeleteAddress(long id);

        Task SetDefaultAddress(long id);

        Task<MemberAddress> GetAddress(long id);
  

        Task<bool> CheckMemberIsOwner(string id);

        Task<MemberAddressListItem> GetAddressForSubmit(long selectAddressId);


        Task<OrderSubmitResult> SubmitOrder(OrderSubmitInfo info);

        void PayOrder(OrderPayInfo payInfo);

        Task<GrouponOrderInfo> GetGrouponOrder(long id);

        Task<OrderDetail> GetOrderDetail(long id);

        Task<PageListResultDto<GrouponOrderListItem>> GetGrouponOrders( int type, GetPageListRequstDto dto);

        Task<PageListResultDto<OrderListItem>> GetOrders(int type, GetPageListRequstDto dto);


        Task ConfirmReceived(long id);

        Task CancelOrder(long id);

        Task OrderComment(OrderCommentInfo info);

        Task<PageListResultDto<GrouponDetail.CommentItem>> GetProductComments(long productId, GetPageListRequstDto dto);

        Task LikeProduct(long productId);

        Task UnLikeProduct(long productId);

        Task<PageListResultDto<GrouponItem>> GetMyLikeGrouponItems(GetPageListRequstDto dto);


        Task<long> RefundApply(RefundApplyDto dto);

        Task<RefundViewDto> GetRefund(long id);

        Task<PageListResultDto<RefundViewDto>> GetRefundOrders(int type, GetPageListRequstDto dto);

        #endregion

        #region 我的工单

        GetEventCountResult GetEventCount(string Type);

        GetEventItemResult GetEventItem(string Type, string EType, string EDType, int PageIndex, int PageSize);

        GetEventInfoResult GetEventInfo(string EType, string EDType, string BillCode);

        #endregion

        InvokeResultDto RetrievePassWord(string Phone, string NewPwd, string VerCode);

        SearchHousingManagementResult SearchHousingManagement(string PRoomCode);


        #region 获取楼盘/楼盘/单元/房间等信息
        GetPProjectListResult GetPProjectList();

        GetPBuildingListResult GetPBuildingList(string pProjectCode);

        GetPUnitListResult GetPUnitList(string pProjectCode, string pBuildingCode);

        GetPFloorListByUnitResult GetPFloorListByUnit(string pProjectCode, string pBuildingCode, string PUnitCode);

        GetPRoomListByFloorResult GetPRoomListByFloor(string pProjectCode, string pBuildingCode, string PUnitCode, string pFloorName);
        #endregion

        InvokeResultDto AddLinkBillMemberMSG(string billcode, string chartinfo);

        InvokeResultDto UpdateLinkBillEvaluation(string billcode, string evalLevel, string suggest);

        long GetFeeService(string projectCode);
        Task<FeeServiceItemDto> GetFeeServiceForView(long id);
    }
}
