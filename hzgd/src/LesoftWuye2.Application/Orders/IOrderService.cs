using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using LesoftWuye2.Application.ForumPosts.Dto;
using LesoftWuye2.Application.Orders.Dto;
using Obs.Dto;

namespace LesoftWuye2.Application.Orders
{
    public interface IOrderService : IApplicationService
    {
        Task<PageListResultDto<OrderListDto>> GetOrders(GetPageListRequstDto dto);
        Task<OrderItemDto> GetOrder(long id);
        Task Ship(OrderShipDto dto);
      
    }
}
