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
    public class GrouponJoinEventWriter : IEventHandler<GrouponJoinEventData>, ITransientDependency
    {
        private readonly IWeixinService _weixinService;
        private readonly TemplateMessageUtils _templateMessageUtils;
        public GrouponJoinEventWriter(IWeixinService weixinService, TemplateMessageUtils templateMessageUtils)
        {
            _weixinService = weixinService;
            _templateMessageUtils = templateMessageUtils;
        }
        public void HandleEvent(GrouponJoinEventData eventData)
        {
           
            var groupInfo =  _weixinService.GetGrouponOrder(eventData.GrouponOrderId).Result;
            var leader = groupInfo.JoinMembers.FirstOrDefault(t => t.IsHeader);
            if (leader == null)
                return;

            var joinMember = groupInfo.JoinMembers.FirstOrDefault(t => t.Id==eventData.JoinMemberId);
            if (joinMember == null)
                return;
            if (groupInfo.GrouponStatus == Core.Mall.Orders.GrouponStatus.GrouponSuccess)
            {
                //拼团成功
                string shipTime = "商品将在48小时内发货。";
                string otherMemberNames = "";
                string members = "";

                foreach (var member in groupInfo.JoinMembers)
                {
                    if (!string.IsNullOrEmpty(members))
                        members += ",";
                    members += member.Name;
                }

                foreach (var member in groupInfo.JoinMembers)
                {
                    var otherMembers= groupInfo.JoinMembers.FindAll(t => t.Id != member.Id);
                    otherMemberNames = "";
                    int idx = 0;
                    foreach (var m in otherMembers)
                    {
                        if(idx++>2)break;
                        if (!string.IsNullOrEmpty(otherMemberNames))
                            otherMemberNames += ",";
                        otherMemberNames += m.Name;
                    }

                    _templateMessageUtils.SendGrouponSuccessMessage(member.Id,eventData.GrouponOrderId,groupInfo.ProductName, otherMemberNames, members,shipTime);
                }
                
                
            }
            else
            {
                //参团成功
                _templateMessageUtils.SendJoinGrouponMessage(joinMember.Id, eventData.GrouponOrderId, groupInfo.ProductName, groupInfo.Price, leader.Name, groupInfo.RequireCount - groupInfo.JoinCount, groupInfo.ExpireTime);
            }

            
        }
    }
}
