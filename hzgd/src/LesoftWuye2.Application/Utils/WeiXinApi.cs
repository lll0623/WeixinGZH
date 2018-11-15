using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.UI;
using Castle.Core.Logging;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Entities.Menu;
using Senparc.Weixin.MP.Helpers;
using LesoftWuye2.Core.Configuration;
using LesoftWuye2.Core.Wuyebase.Members;
using Senparc.Weixin.Entities.TemplateMessage;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;

namespace LesoftWuye2.Application.Utils
{
    public class WeiXinApi : ITransientDependency
    {
        private readonly ISettingManager _settingManager;
        private readonly ILogger _logger;
        private readonly IRepository<Member, long> _memberRepository;

        public WeiXinApi(ISettingManager settingManager,
            ILogger logger,
             IRepository<Member, long> memberRepository)
        {
            _settingManager = settingManager;
            _logger = logger;
            _memberRepository = memberRepository;
        }

        /// <summary>
        /// 验证微信签名
        /// </summary>
        public bool CheckSignature(string token, string signature, string timestamp, string nonce)
        {
            return Senparc.Weixin.MP.CheckSignature.Check(signature, timestamp, nonce, token);
        }

        /// <summary>
        /// 获取微信签名信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <returns></returns>
        public string GetSignature(string token, string timestamp, string nonce)
        {
            return Senparc.Weixin.MP.CheckSignature.GetSignature(timestamp, nonce, token);
        }

        public string GetNickname(string openid, string defaultName = "□□□")
        {
            var appid = _settingManager.GetSettingValue(AppSettings.Weixin.AppId);
            var appsecret = _settingManager.GetSettingValue(AppSettings.Weixin.AppSecret);
            if (!AccessTokenContainer.CheckRegistered(appid))
                AccessTokenContainer.Register(appid, appsecret);
            var userInfo = UserApi.Info(appid, openid);
            if (userInfo == null)
                throw new UserFriendlyException("获取微信昵称失败!");

            Regex regEx = new Regex(@"\p{Cs}");

            string nickname = regEx.Replace(userInfo.nickname, "").Trim();
            if (string.IsNullOrEmpty(nickname))
                return defaultName;
            return nickname;
        }



        public string GetHeadImage(string openid, string defaultImage)
        {
            var appid = _settingManager.GetSettingValue(AppSettings.Weixin.AppId);
            var appsecret = _settingManager.GetSettingValue(AppSettings.Weixin.AppSecret);
            if (!AccessTokenContainer.CheckRegistered(appid))
                AccessTokenContainer.Register(appid, appsecret);
            var userInfo = UserApi.Info(appid, openid);
            if (userInfo == null)
                return defaultImage;

            string image = userInfo.headimgurl;
            if (string.IsNullOrEmpty(image))
                return defaultImage;
            return image;
        }

        public string GetJsTimestamp()
        {
            return JSSDKHelper.GetTimestamp();
        }

        public string GetJsNoncestr()
        {
            return JSSDKHelper.GetNoncestr();
        }

        public string GetJsApi_Ticket()
        {
            var appid = _settingManager.GetSettingValue(AppSettings.Weixin.AppId);
            var appsecret = _settingManager.GetSettingValue(AppSettings.Weixin.AppSecret);
            string ticket = JsApiTicketContainer.TryGetJsApiTicket(appid, appsecret);
            return ticket;
        }

        public JsSdkUiPackage GetJsSdkUiPackage(string url)
        {
            var appid = _settingManager.GetSettingValue(AppSettings.Weixin.AppId);
            var appsecret = _settingManager.GetSettingValue(AppSettings.Weixin.AppSecret);
            var data = JSSDKHelper.GetJsSdkUiPackage(appid, appsecret, url);
            return data;
        }

        public string GetJsapiSignature(string jsapi_ticket, string noncestr, string timestamp, string url)
        {
            //string sign = Senparc.Weixin.MP.Helpers.SHA1UtilHelper.GetSha1(string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}",jsapi_ticket,noncestr,timestamp,url)).ToLower();
            string s1 = JSSDKHelper.GetSignature(jsapi_ticket, noncestr, timestamp, url);
            return s1;
        }

        public string GetAuthorizeUrl(string redirect)
        {
            try
            {
                var appid = _settingManager.GetSettingValue(AppSettings.Weixin.AppId);
                var appsecret = _settingManager.GetSettingValue(AppSettings.Weixin.AppSecret);
                if (!AccessTokenContainer.CheckRegistered(appid))
                    AccessTokenContainer.Register(appid, appsecret);
                var ret = Senparc.Weixin.MP.AdvancedAPIs.OAuthApi.GetAuthorizeUrl(appid, redirect, "0", OAuthScope.snsapi_userinfo);
                return ret;
            }
            catch (Exception ex)
            {
                _logger.Error("获取微信授权地址", ex);
                return "";
            }
        }

        public OAuthAccessTokenResult GetOauthResult(string code)
        {
            try
            {
                var appid = _settingManager.GetSettingValue(AppSettings.Weixin.AppId);
                var appsecret = _settingManager.GetSettingValue(AppSettings.Weixin.AppSecret);
                if (!AccessTokenContainer.CheckRegistered(appid))
                    AccessTokenContainer.Register(appid, appsecret);
                var ret = Senparc.Weixin.MP.AdvancedAPIs.OAuthApi.GetAccessToken(appid, appsecret, code);

                return ret;
            }
            catch (Exception ex)
            {
                _logger.Error("获取微信授权Token", ex);
                return null;
            }
        }

        public OAuthUserInfo GetAuthInfo(OAuthAccessTokenResult token)
        {
            try
            {
                var appid = _settingManager.GetSettingValue(AppSettings.Weixin.AppId);
                var appsecret = _settingManager.GetSettingValue(AppSettings.Weixin.AppSecret);
                if (!AccessTokenContainer.CheckRegistered(appid))
                    AccessTokenContainer.Register(appid, appsecret);
                var ret = Senparc.Weixin.MP.AdvancedAPIs.OAuthApi.GetUserInfo(token.access_token, token.openid);
                return ret;
            }
            catch (Exception ex)
            {
                _logger.Error("获取微信授权信息", ex);
                return null;
            }
        }

        public void CreateMenu()
        {
            try
            {

                var appid = _settingManager.GetSettingValue(AppSettings.Weixin.AppId);
                var appsecret = _settingManager.GetSettingValue(AppSettings.Weixin.AppSecret);
                var siteAddress = _settingManager.GetSettingValue(AppSettings.Wuye.Host);

                var menuService = _settingManager.GetSettingValue(AppSettings.Weixin.MenuService);
                var menuMall = _settingManager.GetSettingValue(AppSettings.Weixin.MenuMall);
                var menuMy = _settingManager.GetSettingValue(AppSettings.Weixin.MenuMy);
                var serviceTemplate = _settingManager.GetSettingValue(AppSettings.Weixin.ServiceTemplate);
                var mallTemplate = _settingManager.GetSettingValue(AppSettings.Weixin.MallTemplate);
                var myTemplate = _settingManager.GetSettingValue(AppSettings.Weixin.MyTemplate);


                if (!AccessTokenContainer.CheckRegistered(appid))
                    AccessTokenContainer.Register(appid, appsecret);
                ButtonGroup bg = new ButtonGroup();

                if (menuService == "true")
                {
                    string url = siteAddress + "/weixin/index";
                    switch (serviceTemplate)
                    {
                        case "SHOPPING-CENTER-001":
                            url = siteAddress + "/weixin/shopping-center-001/index";
                            break;
                    }
                    bg.button.Add(new SingleViewButton() { name = "商户服务", type = "view", url = url });
                }

                if (menuMall == "true")
                {
                    string url = siteAddress + "/weixin/mall";
                    bg.button.Add(new SingleViewButton() { name = "商城", type = "view", url = url });
                }

                if (menuMy == "true")
                {
                    string url = siteAddress + "/weixin/my";
                    switch (myTemplate)
                    {
                        case "SHOPPING-CENTER-001":
                            url = siteAddress + "/weixin/shopping-center-001/my";
                            break;
                    }
                    bg.button.Add(new SingleViewButton() { name = "我的", type = "view", url = url });
                }



                var doresult = CommonApi.CreateMenu(AccessTokenContainer.GetAccessToken(appid), bg);

            }
            catch (Exception ex)
            {
                _logger.Error("创建微信菜单", ex);
                throw new UserFriendlyException(ex.Message, ex);
            }

        }

        public void DownImageFromWeixinServer(string serverId, string savepath)
        {
            try
            {
                var appid = _settingManager.GetSettingValue(AppSettings.Weixin.AppId);
                var appsecret = _settingManager.GetSettingValue(AppSettings.Weixin.AppSecret);
                string accessToken = AccessTokenContainer.TryGetAccessToken(appid, appsecret, true);
                var stream = new MemoryStream();
                MediaApi.Get(accessToken, serverId, stream);
                Image image = Image.FromStream(stream);
                image.Save(savepath);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("从微信服务器下载图片失败!");
            }
        }
        
    }



}