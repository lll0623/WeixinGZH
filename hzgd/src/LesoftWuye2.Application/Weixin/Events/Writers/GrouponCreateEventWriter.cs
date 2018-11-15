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
    public class GrouponCreateEventWriter : IEventHandler<GrouponCreateEventData>, ITransientDependency
    {
        private readonly IWeixinService _weixinService;
        private readonly TemplateMessageUtils _templateMessageUtils;
        public GrouponCreateEventWriter(IWeixinService weixinService, TemplateMessageUtils templateMessageUtils)
        {
            _weixinService = weixinService;
            _templateMessageUtils = templateMessageUtils;
        }
        public void HandleEvent(GrouponCreateEventData eventData)
        {
           
            var groupInfo =  _weixinService.GetGrouponOrder(eventData.GrouponOrderId).Result;
            var leader = groupInfo.JoinMembers.FirstOrDefault(t => t.IsHeader);
            if (leader == null)
                return;
            _templateMessageUtils.SendCreateGrouponMessage(leader.Id, eventData.GrouponOrderId, groupInfo.ProductName, groupInfo.Price, leader.Name, groupInfo.RequireCount-groupInfo.JoinCount, groupInfo.ExpireTime);
        }
    }
}
