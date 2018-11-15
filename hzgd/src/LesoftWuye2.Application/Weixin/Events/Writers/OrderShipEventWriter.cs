using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Events.Bus.Handlers;
using LesoftWuye2.Application.Utils;
using LesoftWuye2.Application.Weixin.Events.EventDatas;

namespace LesoftWuye2.Application.Weixin.Events.Writers
{
    public class OrderShipEventWriter : IEventHandler<OrderShipEventData>, ITransientDependency
    {
        private readonly IWeixinService _weixinService;

        private readonly TemplateMessageUtils _templateMessageUtils;
        public OrderShipEventWriter(IWeixinService weixinService, TemplateMessageUtils templateMessageUtils)
        {
            _weixinService = weixinService;
            _templateMessageUtils = templateMessageUtils;
        }
        public void HandleEvent(OrderShipEventData eventData)
        {
            var orderInfo = _weixinService.GetOrderDetail(eventData.OrderId).Result;
            if(orderInfo==null)return;

            _templateMessageUtils.SendOrderShipMessage(orderInfo.MemberId, orderInfo.Id, orderInfo.ProductName, "快递送货上门");
            
        }
    }
}
