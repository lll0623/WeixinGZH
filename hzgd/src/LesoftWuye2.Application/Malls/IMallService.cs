using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LesoftWuye2.Application.Groupons.Dto;
using LesoftWuye2.Application.Malls.Dto;
using LesoftWuye2.Core.MemberAddresss;
using Obs.Dto;

namespace LesoftWuye2.Application.Malls
{
    public interface IMallService : IApplicationService
    {


        Task<List<ComboboxItemDto>> GetCategories();

        Task<PageListResultDto<GrouponItem>> GetGrouponItems(int cid, GetPageListRequstDto dto);

        Task<GrouponDetail> GetGrouponDetail(long id);

        Task<MallHomeData> GetHomeData();

        #region 地址

        Task<List<ComboboxItemDto>> GetProvinces();
        Task<List<ComboboxItemDto>> GetCities(long provinceId);
        Task<List<ComboboxItemDto>> GetDistricts(long cityId);

        Task AddAddress(MemberAddress address);

        Task EditAddress(MemberAddress address);

        Task<List<MemberAddressListItem>> GetAddresss(long memberId);

        Task DeleteAddress(long id);


        Task SetDefaultAddress(long id);

        Task<MemberAddress> GetAddress(long id);

        //Task SyncMemberInfo(string id);

        Task<bool> CheckMemberIsOwner(string id);

        Task<MemberAddressListItem> GetAddressForSubmit(long memberId,long selectAddressId);

        #endregion

        Task<OrderSubmitResult> SubmitOrder(OrderSubmitInfo info);

        void PayOrder(OrderPayInfo payInfo);

        Task<GrouponOrderInfo> GetGrouponOrder(long id);

        Task<OrderDetail> GetOrderDetail(long id);

        Task<PageListResultDto<GrouponOrderListItem>> GetGrouponOrders(long memberId,int type, GetPageListRequstDto dto);

        Task<PageListResultDto<OrderListItem>> GetOrders(long memberId, int type, GetPageListRequstDto dto);


        Task ConfirmReceived(long id);
        
        Task OrderComment(OrderCommentInfo info);

        Task<PageListResultDto<GrouponDetail.CommentItem>> GetProductComments(long productId,GetPageListRequstDto dto);
    }
}
