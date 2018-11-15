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
    public class GrouponFailEventWriter : IEventHandler<GrouponFailEventData>, ITransientDependency
    {
        private readonly IWeixinService _weixinService;
        private readonly TemplateMessageUtils _templateMessageUtils;
        public GrouponFailEventWriter(IWeixinService weixinService, TemplateMessageUtils templateMessageUtils)
        {
            _weixinService = weixinService;
            _templateMessageUtils = templateMessageUtils;
        }
        public void HandleEvent(GrouponFailEventData eventData)
        {

            foreach (var order in eventData.Orders)
            {
                _templateMessageUtils.SendGrouponFailMessage(order.MemberId,order.OrderId,order.ProductName,order.Price);
            }

            
        }
    }
}
