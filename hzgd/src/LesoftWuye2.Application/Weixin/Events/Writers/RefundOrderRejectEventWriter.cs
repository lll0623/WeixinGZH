﻿using System;
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
    public class RefundOrderRejectEventWriter : IEventHandler<RefundOrderRejectEventData>, ITransientDependency
    {
        private readonly IWeixinService _weixinService;

        private readonly TemplateMessageUtils _templateMessageUtils;
        public RefundOrderRejectEventWriter(IWeixinService weixinService, TemplateMessageUtils templateMessageUtils)
        {
            _weixinService = weixinService;
            _templateMessageUtils = templateMessageUtils;
        }
        public void HandleEvent(RefundOrderRejectEventData eventData)
        {
            _templateMessageUtils.SendRefundOrderRejectMessage(eventData.MemberId, eventData.OrderId, eventData.Amount, eventData.ProductName, eventData.OrderNo);
        }
    }
}
