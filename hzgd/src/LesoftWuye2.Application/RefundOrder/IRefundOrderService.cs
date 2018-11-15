using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using LesoftWuye2.Application.ForumPosts.Dto;
using LesoftWuye2.Application.RefundOrders.Dto;
using Obs.Dto;
using LesoftWuye2.Application.Orders.Dto;

namespace LesoftWuye2.Application.RefundOrders
{
    public interface IRefundOrderService : IApplicationService
    {
        Task<PageListResultDto<RefundOrderListDto>> GetRefundOrders(GetPageListRequstDto dto);

        Task<RefundOrderItemDto> GetRefundOrder(long id);

        Task Reject(RefundOrderRejectDto dto);

        Task Accept(RefundOrderAcceptDto dto);
    }
}
