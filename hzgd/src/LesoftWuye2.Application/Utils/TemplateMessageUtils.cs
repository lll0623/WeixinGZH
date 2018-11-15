using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.UI;
using Castle.Core.Logging;
using LesoftWuye2.Core.Configuration;
using LesoftWuye2.Core.TemplateKeys;
using LesoftWuye2.Core.Wuyebase.Members;
using Senparc.Weixin.Entities.TemplateMessage;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.Containers;

namespace LesoftWuye2.Application.Utils
{
    public class TemplateMessageUtils : ITransientDependency
    {
        private readonly ISettingManager _settingManager;
        private readonly ILogger _logger;
        private readonly IRepository<Member, long> _memberRepository;
        private readonly IRepository<TemplateKey, long> _templateKeyRepository;

        public TemplateMessageUtils(ISettingManager settingManager,
            ILogger logger,
             IRepository<Member, long> memberRepository,
             IRepository<TemplateKey, long> templateKeyRepository)
        {
            _settingManager = settingManager;
            _logger = logger;
            _memberRepository = memberRepository;
            _templateKeyRepository = templateKeyRepository;
        }

        public void SendWorkOrderTemplateMessage(long memberId, string billCode, string title, string progress, string person)
        {
            var memberInfo = _memberRepository.FirstOrDefault(t => t.Id == memberId);
            if (memberInfo == null)
                throw new UserFriendlyException("会员信息不存在!");
            if (string.IsNullOrEmpty(memberInfo.Openid))
                throw new UserFriendlyException("会员未绑定微信!");
            string openid = memberInfo.Openid;
            var host = _settingManager.GetSettingValue(AppSettings.Wuye.Host);
            string url = host.TrimEnd('/') + "/weixin/linkbill/view?EType=2&EDType=3&BillCode=" + billCode;

            try
            {
                var appid = _settingManager.GetSettingValue(AppSettings.Weixin.AppId);
                var appsecret = _settingManager.GetSettingValue(AppSettings.Weixin.AppSecret);
                var workOrderTemplateId = GetTemplateMessageId(_settingManager.GetSettingValue(AppSettings.TemplateKey.WorkOrder));
                if (!AccessTokenContainer.CheckRegistered(appid))
                    AccessTokenContainer.Register(appid, appsecret);

                var result = TemplateApi.SendTemplateMessage(appid, openid, new TemplateMessage_WorkOrderNotice(title, billCode, progress, person, "点击查看详情", workOrderTemplateId, url));

            }
            catch (Exception ex)
            {
                _logger.Error("发送模板消息", ex);
                throw new UserFriendlyException(ex.Message, ex);
            }
        }

        public void SendVoteTemplateMessage(long memberId, string title, string name, string content, string range, string beginTime, string endTime, string url)
        {
            var memberInfo = _memberRepository.FirstOrDefault(t => t.Id == memberId);
            if (memberInfo == null)
                throw new UserFriendlyException("会员信息不存在!");
            if (string.IsNullOrEmpty(memberInfo.Openid))
                throw new UserFriendlyException("会员未绑定微信!");
            string openid = memberInfo.Openid;

            try
            {
                var appid = _settingManager.GetSettingValue(AppSettings.Weixin.AppId);
                var appsecret = _settingManager.GetSettingValue(AppSettings.Weixin.AppSecret);
                var noticeTemplateId = GetTemplateMessageId(_settingManager.GetSettingValue(AppSettings.TemplateKey.Vote));
                if (!AccessTokenContainer.CheckRegistered(appid))
                    AccessTokenContainer.Register(appid, appsecret);

                var result = TemplateApi.SendTemplateMessage(appid, openid, new TemplateMessage_VoteNotice(title, name, content, range, beginTime, endTime, "点击查看详情", noticeTemplateId, url));

            }
            catch (Exception ex)
            {
                _logger.Error("发送模板消息", ex);
                throw new UserFriendlyException(ex.Message, ex);
            }
        }

        public void SendCreateGrouponMessage(long memberId, long grouponOrderId, string productName, decimal price, string leader, int memberCount, DateTime expireTime)
        {
            var memberInfo = _memberRepository.FirstOrDefault(t => t.Id == memberId);
            if (memberInfo == null)
                throw new UserFriendlyException("会员信息不存在!");

            if (string.IsNullOrEmpty(memberInfo.Openid))
                throw new UserFriendlyException("会员未绑定微信!");

            string openid = memberInfo.Openid;

            try
            {
                var appid = _settingManager.GetSettingValue(AppSettings.Weixin.AppId);
                var appsecret = _settingManager.GetSettingValue(AppSettings.Weixin.AppSecret);
                var host = _settingManager.GetSettingValue(AppSettings.Wuye.Host);
                var noticeTemplateId = GetTemplateMessageId(_settingManager.GetSettingValue(AppSettings.TemplateKey.CreateGroupon));
                if (!AccessTokenContainer.CheckRegistered(appid))
                    AccessTokenContainer.Register(appid, appsecret);
                string url = host + "/weixin/mall/groupon/order?id=" + grouponOrderId;
                var result = TemplateApi.SendTemplateMessage(appid, openid, new TemplateMessage_CreateGroupon(productName, price, leader, memberCount, expireTime, noticeTemplateId, url));

            }
            catch (Exception ex)
            {
                _logger.Error("发送模板消息", ex);
            }
        }

        public void SendJoinGrouponMessage(long memberId, long grouponOrderId, string productName, decimal price, string leader, int memberCount, DateTime expireTime)
        {
            var memberInfo = _memberRepository.FirstOrDefault(t => t.Id == memberId);
            if (memberInfo == null)
                throw new UserFriendlyException("会员信息不存在!");

            if (string.IsNullOrEmpty(memberInfo.Openid))
                throw new UserFriendlyException("会员未绑定微信!");

            string openid = memberInfo.Openid;

            try
            {
                var appid = _settingManager.GetSettingValue(AppSettings.Weixin.AppId);
                var appsecret = _settingManager.GetSettingValue(AppSettings.Weixin.AppSecret);
                var host = _settingManager.GetSettingValue(AppSettings.Wuye.Host);
                var noticeTemplateId = GetTemplateMessageId(_settingManager.GetSettingValue(AppSettings.TemplateKey.JoinGroupon));
                if (!AccessTokenContainer.CheckRegistered(appid))
                    AccessTokenContainer.Register(appid, appsecret);
                string url = host + "/weixin/mall/groupon/order?id=" + grouponOrderId;
                var result = TemplateApi.SendTemplateMessage(appid, openid, new TemplateMessage_JoinGroupon(productName, price, leader, memberCount, expireTime, noticeTemplateId, url));

            }
            catch (Exception ex)
            {
                _logger.Error("发送模板消息", ex);
            }
        }

        public void SendGrouponSuccessMessage(long memberId, long grouponOrderId, string productName, string memberstop2, string members, string shipTime)
        {
            var memberInfo = _memberRepository.FirstOrDefault(t => t.Id == memberId);
            if (memberInfo == null)
                throw new UserFriendlyException("会员信息不存在!");

            if (string.IsNullOrEmpty(memberInfo.Openid))
                throw new UserFriendlyException("会员未绑定微信!");

            string openid = memberInfo.Openid;

            try
            {
                var appid = _settingManager.GetSettingValue(AppSettings.Weixin.AppId);
                var appsecret = _settingManager.GetSettingValue(AppSettings.Weixin.AppSecret);
                var host = _settingManager.GetSettingValue(AppSettings.Wuye.Host);
                var noticeTemplateId = GetTemplateMessageId(_settingManager.GetSettingValue(AppSettings.TemplateKey.GrouponSuccess));
                if (!AccessTokenContainer.CheckRegistered(appid))
                    AccessTokenContainer.Register(appid, appsecret);
                string url = host + "/weixin/mall/groupon/order?id=" + grouponOrderId;
                var result = TemplateApi.SendTemplateMessage(appid, openid, new TemplateMessage_GrouponSuccess(productName,memberstop2,members,shipTime, noticeTemplateId, url));

            }
            catch (Exception ex)
            {
                _logger.Error("发送模板消息", ex);
            }
        }

        public void SendGrouponFailMessage(long memberId, long orderId, string productName, decimal price)
        {
            var memberInfo = _memberRepository.FirstOrDefault(t => t.Id == memberId);
            if (memberInfo == null)
                throw new UserFriendlyException("会员信息不存在!");
  
            if (string.IsNullOrEmpty(memberInfo.Openid))
                throw new UserFriendlyException("会员未绑定微信!");

            string openid = memberInfo.Openid;

            try
            {
                var appid = _settingManager.GetSettingValue(AppSettings.Weixin.AppId);
                var appsecret = _settingManager.GetSettingValue(AppSettings.Weixin.AppSecret);
                var host = _settingManager.GetSettingValue(AppSettings.Wuye.Host);
                var noticeTemplateId = GetTemplateMessageId(_settingManager.GetSettingValue(AppSettings.TemplateKey.GrouponFail));
                if (!AccessTokenContainer.CheckRegistered(appid))
                    AccessTokenContainer.Register(appid, appsecret);
                string url = host + "/weixin/mall/order/view?id=" + orderId;
                var result = TemplateApi.SendTemplateMessage(appid, openid, new TemplateMessage_GrouponFail(productName, price, noticeTemplateId, url));

            }
            catch (Exception ex)
            {
                _logger.Error("发送模板消息", ex);
            }
        }

        public void SendOrderShipMessage(long memberId, long orderId, string productName,string shipType)
        {
            var memberInfo = _memberRepository.FirstOrDefault(t => t.Id == memberId);
            if (memberInfo == null)
                throw new UserFriendlyException("会员信息不存在!");

            if (string.IsNullOrEmpty(memberInfo.Openid))
                throw new UserFriendlyException("会员未绑定微信!");

            string openid = memberInfo.Openid;

            try
            {
                var appid = _settingManager.GetSettingValue(AppSettings.Weixin.AppId);
                var appsecret = _settingManager.GetSettingValue(AppSettings.Weixin.AppSecret);
                var host = _settingManager.GetSettingValue(AppSettings.Wuye.Host);
                var noticeTemplateId = GetTemplateMessageId(_settingManager.GetSettingValue(AppSettings.TemplateKey.OrderShip));
                if (!AccessTokenContainer.CheckRegistered(appid))
                    AccessTokenContainer.Register(appid, appsecret);
                string url = host + "/weixin/mall/order/view?id=" + orderId;
                var result = TemplateApi.SendTemplateMessage(appid, openid, new TemplateMessage_OrderShip(productName, shipType, noticeTemplateId, url));

            }
            catch (Exception ex)
            {
                _logger.Error("发送模板消息", ex);
            }
        }

        public void SendRefundOrderRejectMessage(long memberId, long orderId,string amount,string productName, string orderNo)
        {
            var memberInfo = _memberRepository.FirstOrDefault(t => t.Id == memberId);
            if (memberInfo == null)
                throw new UserFriendlyException("会员信息不存在!");

            if (string.IsNullOrEmpty(memberInfo.Openid))
                throw new UserFriendlyException("会员未绑定微信!");

            string openid = memberInfo.Openid;

            try
            {
                var appid = _settingManager.GetSettingValue(AppSettings.Weixin.AppId);
                var appsecret = _settingManager.GetSettingValue(AppSettings.Weixin.AppSecret);
                var host = _settingManager.GetSettingValue(AppSettings.Wuye.Host);
                var noticeTemplateId = GetTemplateMessageId(_settingManager.GetSettingValue(AppSettings.TemplateKey.RefundOrderReject));
                if (!AccessTokenContainer.CheckRegistered(appid))
                    AccessTokenContainer.Register(appid, appsecret);
                string url = host + "/weixin/mall/refund/view?id=" + orderId;
                var result = TemplateApi.SendTemplateMessage(appid, openid, new TemplateMessage_RefundOrderReject(amount,productName,orderNo,noticeTemplateId, url));

            }
            catch (Exception ex)
            {
                _logger.Error("发送模板消息", ex);
            }
        }



        public void SendRefundOrderAcceptMessage(long memberId, long orderId, string amount, string productName, string orderNo)
        {
            var memberInfo = _memberRepository.FirstOrDefault(t => t.Id == memberId);
            if (memberInfo == null)
                throw new UserFriendlyException("会员信息不存在!");

            if (string.IsNullOrEmpty(memberInfo.Openid))
                throw new UserFriendlyException("会员未绑定微信!");

            string openid = memberInfo.Openid;

            try
            {
                var appid = _settingManager.GetSettingValue(AppSettings.Weixin.AppId);
                var appsecret = _settingManager.GetSettingValue(AppSettings.Weixin.AppSecret);
                var host = _settingManager.GetSettingValue(AppSettings.Wuye.Host);
                var noticeTemplateId = GetTemplateMessageId(_settingManager.GetSettingValue(AppSettings.TemplateKey.RefundOrderAccept));
                if (!AccessTokenContainer.CheckRegistered(appid))
                    AccessTokenContainer.Register(appid, appsecret);
                string url = host + "/weixin/mall/refund/view?id=" + orderId;
                var result = TemplateApi.SendTemplateMessage(appid, openid, new TemplateMessage_RefundOrderAccept(productName, amount, noticeTemplateId, url));

            }
            catch (Exception ex)
            {
                _logger.Error("发送模板消息", ex);
            }
        }

        /// <summary>
        /// 获取模板消息的id
        /// </summary>
        /// <param name="templateKey"></param>
        /// <returns></returns>
        string GetTemplateMessageId(string templateKey)
        {
            var appid = _settingManager.GetSettingValue(AppSettings.Weixin.AppId);
            var appsecret = _settingManager.GetSettingValue(AppSettings.Weixin.AppSecret);

            if (!AccessTokenContainer.CheckRegistered(appid))
                AccessTokenContainer.Register(appid, appsecret);



            //生成获取 template id
            try
            {
                var tempInfo = _templateKeyRepository.FirstOrDefault(t => t.TempMsgKey == templateKey);
                if (tempInfo != null && !string.IsNullOrEmpty(tempInfo.TempMsgId))
                    return tempInfo.TempMsgId;

                #region 设置行业(调用频率限制)

                try
                {
                    var temps = TemplateApi.GetIndustry(appid);
                    if (temps.primary_industry.first_class == "IT科技" && temps.primary_industry.second_class == "互联网|电子商务"
                        && temps.secondary_industry.first_class == "房地产" && temps.secondary_industry.second_class == "物业")
                    {

                    }
                    else
                    {
                        TemplateApi.SetIndustry(appid, IndustryCode.IT科技_互联网_电子商务, IndustryCode.房地产_物业);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error("初始化模板消息", ex);
                }

                #endregion

                var addResult = TemplateApi.Addtemplate(appid, templateKey);
                if (tempInfo != null)
                {
                    tempInfo.TempMsgId = addResult.template_id;
                    _templateKeyRepository.Update(tempInfo);
                }
                else
                {
                    tempInfo = new TemplateKey();
                    tempInfo.TempMsgKey = templateKey;
                    tempInfo.TempMsgId = addResult.template_id;
                    _templateKeyRepository.Insert(tempInfo);
                }
                return tempInfo.TempMsgId;
            }
            catch (Exception ex)
            {

                _logger.Error("获取模板ID失败", ex);
                throw new UserFriendlyException("获取模板ID失败" + Environment.NewLine + ex.Message, ex);
            }


        }
    }



    #region 模版消息实体类

    /// <summary>
    /// “工单进度通知”模板消息数据定义
    /// </summary>
    public class TemplateMessage_WorkOrderNotice : TemplateMessageBase
    {
        public TemplateDataItem first { get; set; }
        public TemplateDataItem keyword1 { get; set; }
        public TemplateDataItem keyword2 { get; set; }
        public TemplateDataItem keyword3 { get; set; }
        public TemplateDataItem remark { get; set; }


        public TemplateMessage_WorkOrderNotice(string title, string linkBillNo, string progress, string person,
            string _remark,
            string templateId,
            string url = null)
            : base(templateId, url, "工单进度通知")
        {
            //{ { first.DATA} }
            //工单号：{ { keyword1.DATA} }
            //工单进度：{ { keyword2.DATA} }
            //工单处理人：{ { keyword3.DATA} }
            //{ { remark.DATA} }

            first = new TemplateDataItem(title, "#ff0000");
            keyword1 = new TemplateDataItem(linkBillNo);
            keyword2 = new TemplateDataItem(progress);
            keyword3 = new TemplateDataItem(person);//显示为红色
            remark = new TemplateDataItem(_remark);
        }
    }


    /// <summary>
    /// 投票通知
    /// </summary>
    public class TemplateMessage_VoteNotice : TemplateMessageBase
    {
        public TemplateDataItem first { get; set; }
        public TemplateDataItem keyword1 { get; set; }
        public TemplateDataItem keyword2 { get; set; }
        public TemplateDataItem keyword3 { get; set; }
        public TemplateDataItem keyword4 { get; set; }
        public TemplateDataItem keyword5 { get; set; }
        public TemplateDataItem remark { get; set; }


        public TemplateMessage_VoteNotice(string title, string name, string content, string range, string beginTime, string endTime,
            string _remark,
            string templateId,
            string url = null)
            : base(templateId, url, "物业电子投票通知")
        {
            //{ { first.DATA} }
            //工单号：{ { keyword1.DATA} }
            //工单进度：{ { keyword2.DATA} }
            //工单处理人：{ { keyword3.DATA} }
            //{ { remark.DATA} }

            first = new TemplateDataItem(title);
            keyword1 = new TemplateDataItem(name);
            keyword2 = new TemplateDataItem(content);
            keyword3 = new TemplateDataItem(range);
            keyword4 = new TemplateDataItem(beginTime);
            keyword5 = new TemplateDataItem(endTime);
            remark = new TemplateDataItem(_remark);
        }
    }

    /// <summary>
    /// 开团成功 
    /// </summary>
    public class TemplateMessage_CreateGroupon : TemplateMessageBase
    {
        public TemplateDataItem first { get; set; }
        public TemplateDataItem keyword1 { get; set; }
        public TemplateDataItem keyword2 { get; set; }
        public TemplateDataItem keyword3 { get; set; }
        public TemplateDataItem keyword4 { get; set; }
        public TemplateDataItem keyword5 { get; set; }
        public TemplateDataItem remark { get; set; }


        public TemplateMessage_CreateGroupon(string productName, decimal price, string leader, int memberCount, DateTime expireTime,
            string templateId,
            string url = null)
            : base(templateId, url, "开团成功提醒")
        {

            first = new TemplateDataItem("恭喜您完成支付，开团成功啦，邀请好友参团成团更快哦~");
            keyword1 = new TemplateDataItem(productName);
            keyword2 = new TemplateDataItem(price.ToString("F2") + "元");
            keyword3 = new TemplateDataItem(leader);
            keyword4 = new TemplateDataItem($"还差{memberCount}人", "#ff0000");
            keyword5 = new TemplateDataItem(expireTime.ToString("yyyy年MM月dd日 HH:mm"), "#ff0000");
            remark = new TemplateDataItem("赶快点击进入团页面，分享给好友吧。");
        }
    }
    public class TemplateMessage_JoinGroupon : TemplateMessageBase
    {
        public TemplateDataItem first { get; set; }
        public TemplateDataItem keyword1 { get; set; }
        public TemplateDataItem keyword2 { get; set; }
        public TemplateDataItem keyword3 { get; set; }
        public TemplateDataItem keyword4 { get; set; }
        public TemplateDataItem keyword5 { get; set; }
        public TemplateDataItem remark { get; set; }


        public TemplateMessage_JoinGroupon(string productName, decimal price, string leader, int memberCount, DateTime expireTime,
            string templateId,
            string url = null)
            : base(templateId, url, "参团成功提醒")
        {

            first = new TemplateDataItem($"恭喜您完成支付，成功参加了{leader}开的团，赶快邀请小伙伴一起来参团吧~");
            keyword1 = new TemplateDataItem(productName);
            keyword2 = new TemplateDataItem(price.ToString("F2") + "元");
            keyword3 = new TemplateDataItem(leader);
            keyword4 = new TemplateDataItem($"还差{memberCount}人", "#ff0000");
            keyword5 = new TemplateDataItem(expireTime.ToString("yyyy年MM月dd日 HH:mm"), "#ff0000");
            remark = new TemplateDataItem("赶快点击进入团页面，分享给好友吧。");
        }
    }

    public class TemplateMessage_GrouponSuccess : TemplateMessageBase
    {
        public TemplateDataItem first { get; set; }
        public TemplateDataItem keyword1 { get; set; }
        public TemplateDataItem keyword2 { get; set; }
        public TemplateDataItem keyword3 { get; set; }
        public TemplateDataItem remark { get; set; }


        public TemplateMessage_GrouponSuccess(string productName, string memberstop2, string members, string shipTime,
            string templateId,
            string url = null)
            : base(templateId, url, "拼团成功通知")
        {

            first = new TemplateDataItem($"恭喜您与{memberstop2}等人拼团成功~");
            keyword1 = new TemplateDataItem(productName);
            keyword2 = new TemplateDataItem(members);
            keyword3 = new TemplateDataItem(shipTime);
            remark = new TemplateDataItem("点击进入团页面，查看详情。");
        }
    }
    

    public class TemplateMessage_GrouponFail : TemplateMessageBase
    {
        public TemplateDataItem first { get; set; }
        public TemplateDataItem keyword1 { get; set; }
        public TemplateDataItem keyword2 { get; set; }
        public TemplateDataItem keyword3 { get; set; }
        public TemplateDataItem remark { get; set; }


        public TemplateMessage_GrouponFail (string productName, decimal price,
            string templateId,
            string url = null)
            : base(templateId, url, "拼团失败通知")
        {

            first = new TemplateDataItem($"您参加的拼团因为参团人数不足拼团失败。退款已提交系统处理，最迟5个工作日会退回到您的支付账户。");
            keyword1 = new TemplateDataItem(productName);
            keyword2 = new TemplateDataItem(price.ToString("F2")+"元");
            keyword3 = new TemplateDataItem(price.ToString("F2") + "元");
            remark = new TemplateDataItem("如您使用的是微信零钱支付，退款会退至您的账号内（可以微信：我-钱包-零钱内查看）；信用卡与储蓄卡支付的款项会在1-3个工作日内原路径返回。");
        }
    }

    public class TemplateMessage_OrderShip : TemplateMessageBase
    {
        public TemplateDataItem first { get; set; }
        public TemplateDataItem keyword1 { get; set; }
        public TemplateDataItem keyword2 { get; set; }
        public TemplateDataItem keyword3 { get; set; }
        public TemplateDataItem remark { get; set; }


        public TemplateMessage_OrderShip(string productName, string shiptype,
            string templateId,
            string url = null)
            : base(templateId, url, "商品发货通知")
        {

            first = new TemplateDataItem("您购买的商品已经发货啦","#ff0000");
            keyword1 = new TemplateDataItem(productName);
            keyword2 = new TemplateDataItem(shiptype);
            remark = new TemplateDataItem("点击查看订单详情");
        }
    }


    public class TemplateMessage_RefundOrderReject : TemplateMessageBase
    {
        public TemplateDataItem first { get; set; }
        public TemplateDataItem orderProductPrice { get; set; }
        public TemplateDataItem orderProductName { get; set; }
        public TemplateDataItem orderName { get; set; }
        public TemplateDataItem remark { get; set; }


        public TemplateMessage_RefundOrderReject(string amount, string productName, string orderNo,
            string templateId,
            string url = null)
            : base(templateId, url, "退款申请驳回通知")
        {

            first = new TemplateDataItem("您的退款申请被商家驳回。", "#ff0000");
            orderProductPrice = new TemplateDataItem(amount + "元");
            orderProductName = new TemplateDataItem(productName);
            orderName = new TemplateDataItem(orderNo);
            remark = new TemplateDataItem("点击查看订单详情");
        }
    }

    public class TemplateMessage_RefundOrderAccept : TemplateMessageBase
    {
        public TemplateDataItem first { get; set; }
        public TemplateDataItem keyword1 { get; set; }
        public TemplateDataItem keyword2 { get; set; }
        public TemplateDataItem keyword3 { get; set; }
        public TemplateDataItem remark { get; set; }


        public TemplateMessage_RefundOrderAccept(string productName, string price,
            string templateId,
            string url = null)
            : base(templateId, url, "退款申请审核结果")
        {

            first = new TemplateDataItem($"平台已同意您的退款申请");
            keyword1 = new TemplateDataItem("退款审核通过");
            keyword2 = new TemplateDataItem(price + "元");
            keyword3 = new TemplateDataItem(productName);
            remark = new TemplateDataItem("平台已同意您的退款申请,系统会在1-2天内提交微信/支付宝处理，微信/支付宝审核后再2-5个工作日自动原路退款至您的支付帐号。");
        }
    }


    #endregion
}
