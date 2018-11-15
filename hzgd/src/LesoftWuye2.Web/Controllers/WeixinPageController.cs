using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Dynamic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Abp.Dependency;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.Frontpages;
using LesoftWuye2.Application.Frontpages.Session;
using LesoftWuye2.Application.WuyeApis;
using LesoftWuye2.Core.Estateinfos;
using Obs;
using Obs.Extensions;
using Obs.Users;

using PermissionNames = Obs.Authorization.PermissionNames;
using Abp.Application.Services.Dto;
using Abp.Configuration;
using LesoftWuye2.Application.Forums;
using LesoftWuye2.Application.Malls;
using LesoftWuye2.Application.Plates;
using LesoftWuye2.Application.Utils;
using LesoftWuye2.Application.Weixin;
using LesoftWuye2.Application.Weixin.Session;
using LesoftWuye2.Core.Configuration;
using Abp.Extensions;
using Abp.Json;
using Abp.UI;
using Abp.Web.Models;
using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Util;
using LesoftWuye2.Application.PayNoifys;
using LesoftWuye2.Application.WeixinSubscribes;
using LesoftWuye2.Web.Models;
using Obs.Dto;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.TenPayLibV3;
using RequestHandler = Senparc.Weixin.MP.TenPayLib.RequestHandler;

namespace LesoftWuye2.Web.Controllers
{
    [RoutePrefix("weixin")]
    //[Route("{action}")]
    public class WeixinPageController : LesoftWuye2ControllerBase
    {

        private readonly IIocManager _iocManager;
        private readonly IWeixinService _weixinAppSrvice;
        private readonly IWuyeApiAppSrvice _wuyeApiAppSrvice;
        readonly List<string> noauthActions = new List<string>();
        private readonly WeiXinApi _weiXinApi;
        private readonly TemplateMessageUtils _templateMessageUtils;
        readonly List<string> forbiddenWhenLoginedActions = new List<string>();//当前已经绑定账号，不允许访问的页面
        private readonly IPayNotifyAppService _payNotifyAppService;
        private readonly IWeixinSubscribeAppService _weixinSubscribeAppService;

        public WeixinPageController(IIocManager iocManager,
           IWeixinService weixinAppSrvice,
           WeiXinApi weiXinApi,
           TemplateMessageUtils templateMessageUtils,
            IWuyeApiAppSrvice wuyeApiAppSrvice,
            IPayNotifyAppService payNotifyAppService,
            IWeixinSubscribeAppService weixinSubscribeAppService)
        {
            _iocManager = iocManager;
            _weixinAppSrvice = weixinAppSrvice;
            _weiXinApi = weiXinApi;
            _templateMessageUtils = templateMessageUtils;
            _wuyeApiAppSrvice = wuyeApiAppSrvice;
            _payNotifyAppService = payNotifyAppService;
            _weixinSubscribeAppService = weixinSubscribeAppService;

            noauthActions.Add("Login");
            noauthActions.Add("Api");
            noauthActions.Add("Auth");
            noauthActions.Add("Register");
            noauthActions.Add("ForgetPassword");
            noauthActions.Add("Paynotify");
            noauthActions.Add("Alipay");
            noauthActions.Add("AlipayNotify");
            noauthActions.Add("AlipayReturn");


            forbiddenWhenLoginedActions.Add("Login");
            forbiddenWhenLoginedActions.Add("Register");
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            base.OnActionExecuting(filterContext);
            string actionName = filterContext.RequestContext.RouteData.Values["action"].ToString();
            if (actionName == "Api"
                || actionName == "Auth"
                || actionName == "Paynotify"
                || actionName == "Alipay"
                || actionName == "AlipayNotify"
                || actionName == "AlipayReturn"
                || actionName == "WorkOrderNotice"
                || actionName == "VoteNotice")
                return;

            //检查cookie中没有openid,就授权
            string openid = GetCookie("_token_");
            if (openid.IsNullOrEmpty() && this.Request["authing"] != "1" && this.Request.Url.Host != "localhost")
            {
                //跳转到授权页面
                string authurl = _weiXinApi.GetAuthorizeUrl(this.Request.Url.Scheme + "://" + this.Request.Url.Host + "/weixin/auth?authing=1&refurl=" + this.Request.Url.PathAndQuery);
                filterContext.Result = new RedirectResult(authurl);
            }

            var session = _iocManager.Resolve<WeixinSession>();
            session.Reset();
            if (this.Request.Cookies?["_token_"] != null)
            {
                session.OpenId = this.Request.Cookies["_token_"].Value;
                session.Prepare();
            }



            //每个页面都需要授权
            //List< FilterAttribute > filterAttributes=new List<FilterAttribute>(filterContext.ActionDescriptor.GetFilterAttributes(true));
            List<Type> filterTypes = filterContext.ActionDescriptor.GetFilterAttributes(true).Select(attribute => attribute.GetType()).ToList();

            if (session.MemberId == 0 && filterTypes.Contains(typeof(WeixinPageAuthAttribute)))
                filterContext.Result = new RedirectResult("/weixin/register");


            if (session.MemberId != 0 && forbiddenWhenLoginedActions.Contains(actionName))
                filterContext.Result = new RedirectResult("/weixin/my");
        }

        #region 微信接口

        void InitJssdk()
        {
            var jssdkInfo = _weiXinApi.GetJsSdkUiPackage(Request.Url.AbsoluteUri);
            ViewData["AppId"] = jssdkInfo.AppId;
            ViewData["NonceStr"] = jssdkInfo.NonceStr;
            ViewData["Signature"] = jssdkInfo.Signature;
            ViewData["Timestamp"] = jssdkInfo.Timestamp;
        }

        [Route("api")]
        public ActionResult Api()
        {
            if (this.Request.HttpMethod.ToUpper() == "GET")
            {
                return Content(Check());
            }
            else
            {
                string signature = this.Request["signature"];
                string timestamp = this.Request["timestamp"];
                string nonce = this.Request["nonce"];
                string msg_signature = this.Request["msg_signature"];

                string Token = SettingManager.GetSettingValue(AppSettings.Weixin.Token);
                string AppId = SettingManager.GetSettingValue(AppSettings.Weixin.AppId);
                PostModel postModel = new PostModel()
                {
                    Signature = signature,
                    AppId = AppId,
                    Timestamp = timestamp,
                    Token = Token,
                    Nonce = nonce,
                    Msg_Signature = msg_signature,
                };


                if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
                {
                    return new WeixinResult("参数错误！");//v0.8+
                }



                var messageHandler = new CustomMessageHandler.CustomMessageHandler(Request.InputStream, postModel, 10);
                messageHandler.WeixinSubscribeAppService = _weixinSubscribeAppService;
                messageHandler.Host = SettingManager.GetSettingValue(AppSettings.Wuye.Host);

                messageHandler.Execute();//执行微信处理过程

                return new FixWeixinBugWeixinResult(messageHandler);//v0.8+
            }


        }

        [Route("auth")]
        public ActionResult Auth()
        {
            var tokenInfo = _weiXinApi.GetOauthResult(this.Request["code"]);
            if (tokenInfo == null)
            {
                return Content("<h1>微信授权失败，请稍后重试.</h1>");
            }
            else
            {
                var wxUserInfo = _weiXinApi.GetAuthInfo(tokenInfo);
                if (wxUserInfo == null)
                {
                    return Content("<h1>微信授权失败，请稍后重试.</h1>");
                }

                this.Response.Cookies.Add(new HttpCookie("_token_", wxUserInfo.openid));
                this.Response.Cookies["_token_"].Expires = DateTime.Now.AddDays(1);

                return Redirect(this.Request["refurl"]);
            }

        }

        [Route("paynotify")]
        public ContentResult Paynotify()
        {
            var paykey = SettingManager.GetSettingValue(AppSettings.Weixin.PayKey);
            //paykey = "a906449d5769fa7361d7ecc6aa3f6d28";

            ResponseHandler resHandler = new ResponseHandler(null);

            string return_code = resHandler.GetParameter("return_code");
            string return_msg = resHandler.GetParameter("return_msg");

            string res = null;

            //CreatePayNotifyInput input = new CreatePayNotifyInput();
            string Request = resHandler.ParseXML();// resHandler.GetDebugInfo() ?? "";
            string OrderNo = resHandler.GetParameter("out_trade_no").IsNullOrEmpty() ? "NONE" : resHandler.GetParameter("out_trade_no");
            string type = resHandler.GetParameter("attach");

            resHandler.SetKey(paykey);
            //验证请求是否从微信发过来（安全）
            if (resHandler.IsTenpaySign())
            {
                res = "success";
                //input.Success = true;
                string transaction_id = resHandler.GetParameter("transaction_id");
                string openid = resHandler.GetParameter("openid");
                //正确的订单处理
                //_wuyeApiAppSrvice.CreateReceipt(OrderNo, transaction_id, resHandler.GetParameter("total_fee"));
                _payNotifyAppService.Notify(int.Parse(type), "微信支付", OrderNo, decimal.Parse(resHandler.GetParameter("total_fee")) / 100, Request, transaction_id);
            }
            else
            {
                res = "wrong";

                //错误的订单处理
            }

            //_payNotifyAppService.CreatePayNotify(input);


            string xml = string.Format(@"<xml>
   <return_code><![CDATA[{0}]]></return_code>
   <return_msg><![CDATA[{1}]]></return_msg>
</xml>", return_code, return_msg);

            return Content(xml, "text/xml");

        }

        string Check()
        {
            string token = SettingManager.GetSettingValue(AppSettings.Weixin.Token);
            string echoString = this.Request["echoStr"];
            string signature = this.Request["signature"];
            string timestamp = this.Request["timestamp"];
            string nonce = this.Request["nonce"];
            //WeiXinApi weiXinApi = new WeiXinApi();
            if (_weiXinApi.CheckSignature(token, signature, timestamp, nonce))
            {
                return echoString;
            }
            else
            {
                return "failed:" + signature + "," + _weiXinApi.GetSignature(timestamp, nonce, token) + ". If you see this message in the browser, indicating that this Url micro-channel can fill the background.";
            }

        }

        #endregion

        #region 个人信息

        //[Route("login")]
        //public ActionResult Login()
        //{

        //    return View("Weixin/Login");
        //}


        [Route("register")]
        public ActionResult Register()
        {
            var session = _iocManager.Resolve<WeixinSession>();
            ViewData["nickname"] = _weiXinApi.GetNickname(session.OpenId);
            return View("Weixin/Register");
        }

        [Route("forgetpassword")]
        public ActionResult ForgetPassword(int type = 0)
        {
            ViewData["title"] = "找回密码";
            if (type == 0)
                ViewData["title"] = "找回密码";
            if (type == 1)
                ViewData["title"] = "修改密码";

            return View("Weixin/ForgetPassword");
        }


        [Route("my")]
        [WeixinPageAuthAttribute]
        public ActionResult My()
        {
            var data = _weixinAppSrvice.GetMy();
            return View("Weixin/My/Index", data);
        }

        #endregion

        #region 我的工单

        [WeixinPageAuthAttribute]
        [Route("linkbill/count")]
        public ActionResult LinkBillCount()
        {
            return View("Weixin/LinkBill/Count");
        }

        [WeixinPageAuthAttribute]
        [Route("linkbill/list")]
        public ActionResult LinkBillList(int EType, int EDType)
        {
            string title = "";

            switch (EType)
            {
                case 1:
                    title = "报事报修";
                    break;
                case 2:
                    title = "投诉";
                    break;
                case 3:
                    title = "其它";
                    break;
                default:
                    title = "未知";
                    break;
            }
            if (EType == 1 || EType == 2)
            {
                switch (EDType)
                {
                    case 1:
                        title += "-未派单";
                        break;
                    case 2:
                        title += "-进行中";
                        break;
                    case 3:
                        title += "-已完成";
                        break;
                }
            }
            if (EType == 3)
            {
                switch (EDType)
                {
                    case 1:
                        title += "-建议";
                        break;
                    case 2:
                        title += "-表扬";
                        break;
                    case 3:
                        title += "-咨询";
                        break;
                }
            }
            ViewData["title"] = title;
            ViewData["EType"] = EType;
            ViewData["EDType"] = EDType;
            return View("Weixin/LinkBill/List");
        }

        [WeixinPageAuthAttribute]
        [Route("linkbill/view")]
        public ActionResult LinkBillView(int EType, int EDType, string BillCode)
        {
            ViewData["EType"] = EType;
            ViewData["EDType"] = EDType;
            ViewData["BillCode"] = BillCode;
            return View("Weixin/LinkBill/View");
        }
        #endregion

        #region 物业服务

        [WeixinPageAuthAttribute]
        [Route("service/repair")]
        public ActionResult Service_Repair()
        {
            InitJssdk();
            return View("Weixin/Service/Repair");
        }

        [WeixinPageAuthAttribute]
        [Route("service/feedback")]
        public ActionResult Service_Feedback()
        {
            InitJssdk();
            return View("Weixin/Service/Feedback");
        }

        [WeixinPageAuthAttribute]
        [Route("service/fee")]
        public ActionResult Service_Fee()
        {
            ViewData["EnabledPayFee"] = SettingManager.GetSettingValue(AppSettings.Wuye.EnabledPayFee) == "true";
            return View("Weixin/Service/Fee");
        }

        [WeixinPageAuthAttribute]
        [Route("service/preparefee")]
        public ActionResult Service_PrepareFee()
        {
            return View("Weixin/Service/PrepareFee");
        }

        [WeixinPageAuthAttribute]
        [Route("service/contact")]
        public ActionResult Service_Contact()
        {
            return View("Weixin/Service/Contact");
        }

        [WeixinPageAuthAttribute]
        [Route("service/paylist")]
        public ActionResult PayList(string type, string money, string orderno)
        {
            money = "0.01";//调试用

            ViewData["type"] = type;
            ViewData["money"] = money;
            ViewData["orderno"] = orderno;
            return View("Weixin/Service/PayList");
        }

        [WeixinPageAuthAttribute]
        [Route("service/createpay")]
        public ActionResult CreatePay()
        {
            string money = this.Request["money"];
            string orderno = this.Request["orderno"];
            int type = int.Parse(this.Request["type"]);
            WxPayModel payModel = new WxPayModel();

            var appid = SettingManager.GetSettingValue(AppSettings.Weixin.AppId);//"wxa47079b8fa2b2c6b";
            var appsecret = SettingManager.GetSettingValue(AppSettings.Weixin.AppSecret);
            var mchid = SettingManager.GetSettingValue(AppSettings.Weixin.MchId);
            var paykey = SettingManager.GetSettingValue(AppSettings.Weixin.PayKey);
            var notifyurl = SettingManager.GetSettingValue(AppSettings.Wuye.Host) + "/weixin/paynotify";
            var openid = GetCookie("_token_");


            //appid = "wx6b2702a701c01967";
            //mchid = "1442348102";
            //paykey = "a906449d5769fa7361d7ecc6aa3f6d28";
            //openid = "oZ0svuK4oaI50SSv7w_c88P8DGPE";

            string timeStamp = "";
            string nonceStr = "";
            string paySign = "";

            string sp_billno = orderno;
            int total_fee = (int)(Math.Round(decimal.Parse(money), 2) * 100);

            timeStamp = TenPayV3Util.GetTimestamp();
            nonceStr = TenPayV3Util.GetNoncestr();

            string subject = "物业缴费";
            switch (type)
            {
                case 0:
                    subject = "物业缴费";
                    break;
                case 1:
                    subject = "商城团购";
                    break;
                case 2:
                    subject = "物业预缴";
                    break;
            }

            string returnUrl = "/weixin/service/fee";
            switch (type)
            {
                case 0:
                    returnUrl = "/weixin/service/fee";
                    break;
                case 1:
                    returnUrl = "/weixin/mall/order/list";
                    break;
                case 2:
                    returnUrl = "/weixin/service/preparefee";
                    break;
            }


            var xmlDataInfo = new TenPayV3UnifiedorderRequestData(appid, mchid, type + ":" + subject,
                sp_billno, total_fee, Request.UserHostAddress, notifyurl, TenPayV3Type.JSAPI, openid, paykey, nonceStr, attach: type.ToString());

            var result = TenPayV3.Unifiedorder(xmlDataInfo);//调用统一订单接口
            var package = string.Format("prepay_id={0}", result.prepay_id);
            var res = result;
            Logger.Info("支付 订单号:" + orderno + ",prepay:" + result.ToString());
            //
            var code = res.return_code;
            if (code == "FAIL")
            {
                string errormsg = res.return_msg;
                return Json(new AjaxResponse(new ErrorInfo(errormsg)));
            }
            var result_code = res.err_code;
            if (result_code == "FAIL")
            {
                string errormsg = res.err_code_des;
                return Json(new AjaxResponse(new ErrorInfo(errormsg)));
            }

            string prepayId = res.prepay_id;

            payModel.appId = appid;
            payModel.timeStamp = timeStamp;
            payModel.nonceStr = nonceStr;
            payModel.package = string.Format("prepay_id={0}", prepayId);
            payModel.paySign = TenPayV3.GetJsPaySign(appid, timeStamp, nonceStr, package, paykey); ;
            payModel.returnUrl = returnUrl;


            return Json(new AjaxResponse(payModel));
        }

        [WeixinPageAuthAttribute]
        [Route("room/list")]
        public async Task<ActionResult> Room_List()
        {
            var slideImages = await _weixinAppSrvice.GetSlideImages();
            return View("Weixin/Room/List", slideImages);
        }

        [WeixinPageAuthAttribute]
        [Route("room/add")]
        public ActionResult Room_Add()
        {
            return View("Weixin/Room/Add");
        }

        [WeixinPageAuthAttribute]
        [Route("room/addwithphone")]
        public ActionResult Room_AddWithPhone()
        {
            return View("Weixin/Room/AddWithPhone");
        }

        [WeixinPageAuthAttribute]
        [Route("room/addwithwizard")]
        public ActionResult Room_AddWithWizard()
        {
            return View("Weixin/Room/AddWithWizard");
        }

        [WeixinPageAuthAttribute]
        [Route("room/addwithcode")]
        public ActionResult Room_AddWithCode()
        {
            InitJssdk();
            return View("Weixin/Room/AddWithCode");
        }

        [WeixinPageAuthAttribute]
        [Route("room/addwithpassword")]
        public ActionResult Room_AddWithPassword()
        {
            return View("Weixin/Room/AddWithPassword");
        }


        #endregion

        #region 首页

        [Route("index")]    
        public async Task<ActionResult> Index()
        {
            var data = await _weixinAppSrvice.GetHomeData();
            var session = _iocManager.Resolve<WeixinSession>();
            ViewData["isLogin"] = session.MemberId != 0;
            ViewData["isBindRoom"] = session.IsBindRoom;
            return View("Weixin/Index", data);
        }

        #endregion

        #region 公告

        [Route("notice/list")]
        public async Task<ActionResult> Notice_List()
        {
            var data = await _weixinAppSrvice.GetNotices(BuildPageListRequstDto());
            return View("Weixin/Notice/List", data);
        }

        [Route("notice/view")]
        public async Task<ActionResult> Notice_View(long id)
        {
            var data = await _weixinAppSrvice.GetNotice(id);
            return View("Weixin/Notice/View", data);
        }
        #endregion

        #region 活动

        [Route("activity/list")]
        public async Task<ActionResult> Activity_List()
        {
            return View("Weixin/Activity/List");
        }


        [Route("activity/view")]
        public async Task<ActionResult> Activity_View(long id)
        {
            var data = await _weixinAppSrvice.GetActivity(id);
            ViewData["is_join"] = await _weixinAppSrvice.CheckIsJoinActivity(id);
            return View("Weixin/Activity/View", data);
        }

        [WeixinPageAuthAttribute]
        [Route("activity/my")]
        public async Task<ActionResult> Activity_My()
        {
            return View("Weixin/Activity/My");
        }

        #endregion

        #region 新闻

        [Route("news/list")]
        public async Task<ActionResult> News_List()
        {

            return View("Weixin/News/List");
        }
        [Route("news/view")]
        public async Task<ActionResult> News_View(long id)
        {
            var data = await _weixinAppSrvice.GetNews(id);
            return View("Weixin/News/View", data);
        }

        #endregion

        #region 有偿服务
        [Route("feeservice/view")]
        public async Task<ActionResult> FeeService_View(long id)
        {
            var data = await _weixinAppSrvice.GetFeeServiceForView(id);
            return View("Weixin/FeeService/View", data);
        }
        #endregion

        #region 置信网站

        [Route("substation/list")]
        public async Task<ActionResult> Substation_List()
        {
            return View("Weixin/Substation/List");
        }

        #endregion

        #region 商城

        [Route("mall")]
        public async Task<ActionResult> Mall()
        {
            var data = await _weixinAppSrvice.GetMallHomeData();

            //广告配置
            var AdImage = await SettingManager.GetSettingValueAsync(AppSettings.Mall.AdImage);
            var AdUrl = await SettingManager.GetSettingValueAsync(AppSettings.Mall.AdUrl);
            var AdImage2 = await SettingManager.GetSettingValueAsync(AppSettings.Mall.AdImage2);
            var AdUrl2 = await SettingManager.GetSettingValueAsync(AppSettings.Mall.AdUrl2);


            ViewData["AdImage"] = AdImage;
            ViewData["AdUrl"] = AdUrl;
            ViewData["AdImage2"] = AdImage2;
            ViewData["AdUrl2"] = AdUrl2;
            return View("Weixin/Mall/index", data);
        }


        [Route("mall/groupon/list")]
        public async Task<ActionResult> GrouponList(int cid = 0)
        {
            ViewData["categories"] = await _weixinAppSrvice.GetCategories();
            ViewData["cid"] = cid;
            return View("Weixin/Mall/Groupon/List");
        }


        [Route("mall/groupon/like")]
        public async Task<ActionResult> GrouponLike(int cid = 0)
        {
            return View("Weixin/Mall/Groupon/Like");
        }

        [Route("mall/groupon/orderlist")]
        public async Task<ActionResult> GrouponOrderList(int type = 0)
        {
            ViewData["type"] = type;
            return View("Weixin/Mall/Groupon/OrderList");

        }

        [Route("mall/groupon/comments")]
        public ActionResult GrouponComments(long id)
        {
            ViewData["id"] = id;
            return View("Weixin/Mall/Groupon/Comments");
        }


        [Route("mall/groupon/view")]
        public async Task<ActionResult> GrouponView(int id)
        {
            var data = await _weixinAppSrvice.GetGrouponDetail(id);
            string GrouponDescription = await SettingManager.GetSettingValueAsync(AppSettings.Mall.GrouponDescription);
            GrouponDescription = GrouponDescription.Replace("\n", "<br/>");
            ViewData["GrouponDescription"] = GrouponDescription;
            ViewData["isyz"] = false;// (_iocManager.Resolve<WeixinSession>()).IsBindRoom;
            InitJssdk();
            ViewData["host"] = await SettingManager.GetSettingValueAsync(AppSettings.Wuye.Host);
            return View("Weixin/Mall/Groupon/View", data);

        }

        [Route("mall/groupon/order")]
        public async Task<ActionResult> GrouponOrder(int id)
        {
            var data = await _weixinAppSrvice.GetGrouponOrder(id);
            InitJssdk();
            ViewData["host"] = await SettingManager.GetSettingValueAsync(AppSettings.Wuye.Host);
            return View("Weixin/Mall/Groupon/Order", data);
        }

        [WeixinPageAuthAttribute]
        [Route("mall/groupon/submit")]
        public async Task<ActionResult> GrouponSubmit(int id, int type, int num, long joinGrouponId = 0)
        {
            var data = await _weixinAppSrvice.GetGrouponDetail(id);
            //if ((_iocManager.Resolve<WeixinSession>()).IsBindRoom)
            //    data.SalePrice = data.MemberPrice;

            ViewData["type"] = type;
            ViewData["num"] = num;
            ViewData["joinGrouponId"] = joinGrouponId;
            return View("Weixin/Mall/Groupon/Submit", data);

        }

        [WeixinPageAuthAttribute]
        [Route("mall/order/view")]
        public async Task<ActionResult> OrderView(int id)
        {
            var data = await _weixinAppSrvice.GetOrderDetail(id);
            return View("Weixin/Mall/Order/View", data);
        }

        [WeixinPageAuthAttribute]
        [Route("mall/order/comment")]
        public ActionResult OrderComment(int id)
        {
            ViewData["id"] = id;
            return View("Weixin/Mall/Order/Comment");
        }

        [WeixinPageAuthAttribute]
        [Route("mall/address/list")]
        public ActionResult AddressList()
        {
            return View("Weixin/Mall/Address/List");
        }

        [WeixinPageAuthAttribute]
        [Route("mall/address/select")]
        public ActionResult AddressSelect()
        {
            return View("Weixin/Mall/Address/Select");
        }

        [WeixinPageAuthAttribute]
        [Route("mall/address/add")]
        public ActionResult AddressAdd()
        {
            return View("Weixin/Mall/Address/Add");
        }

        [WeixinPageAuthAttribute]
        [Route("mall/address/edit")]
        public async Task<ActionResult> AddressEdit(long id)
        {
            var data = await _weixinAppSrvice.GetAddress(id);

            ViewData["provinces"] = await _weixinAppSrvice.GetProvinces();
            ViewData["cities"] = await _weixinAppSrvice.GetCities(data.ProvinceId);
            ViewData["district"] = await _weixinAppSrvice.GetDistricts(data.CityId);

            return View("Weixin/Mall/Address/Edit", data);
        }

        [WeixinPageAuthAttribute]
        [Route("mall/order/list")]
        public ActionResult OrderList(int type = 0)
        {
            ViewData["type"] = type;
            return View("Weixin/Mall/Order/List");

        }

        [Route("mall/waiting")]
        public async Task<ActionResult> Waiting()
        {

            return View("Weixin/Mall/Waiting");
        }

        [WeixinPageAuthAttribute]
        [Route("mall/refund/apply")]
        public async Task<ActionResult> RefundApply(long id)
        {
            InitJssdk();
            var data = await _weixinAppSrvice.GetOrderDetail(id);
            return View("Weixin/Mall/Refund/Apply", data);
        }

        [WeixinPageAuthAttribute]
        [Route("mall/refund/view")]
        public async Task<ActionResult> GetRefund(long id)
        {
            var data = await _weixinAppSrvice.GetRefund(id);
            return View("Weixin/Mall/Refund/View", data);
        }

        [Route("mall/refund/list")]
        public ActionResult RefundList(int type = 0)
        {
            ViewData["type"] = type;
            return View("Weixin/Mall/Refund/List");

        }

        #endregion

        #region 跳蚤市场

        [WeixinPageAuthAttribute]
        [Route("estateinfo/add")]
        public async Task<ActionResult> Estateinfo_Add()
        {
            var data = await _weixinAppSrvice.GetEstateinfoTypesForCombo();
            return View("Weixin/Estateinfo/Add", data);
        }

        [Route("estateinfo/list")]
        public async Task<ActionResult> Estateinfo_List()
        {
            int mode = (this.Request["mode"] ?? "").ParseTo<int>(0);

            List<ComboboxItemDto> nextItems = new List<ComboboxItemDto>();
            string thisMode = "全部";
            var types = await _weixinAppSrvice.GetEstateinfoTypesForCombo();
            types.Add(new ComboboxItemDto("0", "全部"));
            foreach (var dto in types)
            {
                if (dto.Value != mode.ToString())
                {
                    nextItems.Add(dto);
                }
                else
                {
                    thisMode = dto.DisplayText;
                }
            }


            ViewData["thismode"] = thisMode;
            ViewData["nextItems"] = nextItems;
            var data = await _weixinAppSrvice.GetEstateinfos(mode, BuildPageListRequstDto());
            return View("Weixin/Estateinfo/List", data);
            //return null;
        }

        [Route("estateinfo/view")]
        public async Task<ActionResult> Estateinfo_View(long id)
        {
            var data = await _weixinAppSrvice.GetEstateinfo(id);
            return View("Weixin/Estateinfo/View", data);
        }

        [WeixinPageAuthAttribute]
        [Route("estateinfo/myview")]
        public async Task<ActionResult> Estateinfo_MyView(long id)
        {
            var data = await _weixinAppSrvice.GetEstateinfo(id);
            return View("Weixin/Estateinfo/MyView", data);
        }

        [WeixinPageAuthAttribute]
        [Route("estateinfo/my")]
        public ActionResult Estateinfo_My(int type = 0)
        {
            string title = "未知";
            switch (type)
            {
                case 0:
                    title = "未审核的跳蚤商品信息";
                    break;
                case 1:
                    title = "上架的跳蚤商品信息";
                    break;
                case 2:
                    title = "下架的跳蚤商品信息";
                    break;
            }

            ViewData["title"] = title;
            ViewData["type"] = type;
            return View("Weixin/Estateinfo/My");
        }

        #endregion

        #region 租售信息

        [WeixinPageAuthAttribute]
        [Route("rentsaleinfo/add")]
        public async Task<ActionResult> Rentsaleinfo_Add()
        {
            var data = await _weixinAppSrvice.GetRentsaleinfoTypesForCombo();
            return View("Weixin/Rentsaleinfo/Add", data);
        }

        [Route("rentsaleinfo/list")]
        public async Task<ActionResult> Rentsaleinfo_List()
        {
            int mode = (this.Request["mode"] ?? "").ParseTo<int>(0);

            List<ComboboxItemDto> nextItems = new List<ComboboxItemDto>();
            string thisMode = "全部";
            var types = await _weixinAppSrvice.GetRentsaleinfoTypesForCombo();
            types.Add(new ComboboxItemDto("0", "全部"));
            foreach (var dto in types)
            {
                if (dto.Value != mode.ToString())
                {
                    nextItems.Add(dto);
                }
                else
                {
                    thisMode = dto.DisplayText;
                }
            }


            ViewData["thismode"] = thisMode;
            ViewData["nextItems"] = nextItems;
            var data = await _weixinAppSrvice.GetRentsaleinfos(mode, BuildPageListRequstDto());
            return View("Weixin/Rentsaleinfo/List", data);
            //return null;
        }

        [Route("rentsaleinfo/view")]
        public async Task<ActionResult> Rentsaleinfo_View(long id)
        {
            var data = await _weixinAppSrvice.GetRentsaleinfo(id);
            return View("Weixin/Rentsaleinfo/View", data);
        }

        [WeixinPageAuthAttribute]
        [Route("rentsaleinfo/myview")]
        public async Task<ActionResult> Rentsaleinfo_MyView(long id)
        {
            var data = await _weixinAppSrvice.GetRentsaleinfo(id);
            return View("Weixin/Rentsaleinfo/MyView", data);
        }

        [WeixinPageAuthAttribute]
        [Route("rentsaleinfo/my")]
        public ActionResult Rentsaleinfo_My(int type = 0)
        {
            string title = "未知";
            switch (type)
            {
                case 0:
                    title = "未审核的租售信息";
                    break;
                case 1:
                    title = "上架的租售信息";
                    break;
                case 2:
                    title = "下架的租售信息";
                    break;
            }

            ViewData["title"] = title;
            ViewData["type"] = type;
            return View("Weixin/Rentsaleinfo/My");
        }

        #endregion

        #region 支付宝支付

        [Route("service/alipay")]
        public ActionResult Alipay(int type, string orderNo, string money)
        {

            //如果浏览器是微信就显示提示用浏览器打开
            //否则启用支付
            if (this.Request.UserAgent.ToLower().Contains("micromessenger"))
            {

                string returnUrl = "/weixin/service/fee";
                switch (type)
                {
                    case 0:
                        returnUrl = "/weixin/service/fee";
                        break;
                    case 1:
                        returnUrl = "/weixin/mall/order/list";
                        break;
                    case 2:
                        returnUrl = "/weixin/service/preparefee";
                        break;
                }

                ViewData["returnUrl"] = returnUrl;

                return View("Weixin/Service/Alipay");
            }


            if (string.IsNullOrEmpty(orderNo))
                orderNo = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            if (string.IsNullOrEmpty(money))
                money = "0.01";


            string APPID = "2016081501749843";
            string PID = "2088421556051016";
            string APP_PRIVATE_KEY = @"MIIEowIBAAKCAQEArtd47la2jHyV5wbTkn/GtVPvTzTr0w6PxWYRY/dnscHVHyPd56Yc/NMJoavcMvJaH4lUzmTBcj3Nz8cv4gsVqYUqyojtukh9JBqW4njdC3IuggBH1Lmo/6cgaZjOzSEHcwyWNU0TcgfFj+y8Kd47YVdh9+xW9/M3nQn9MAaQgfHZbmLMzV5mg/IsB9JzCG963JSuMGq1CiEtYggllC+bhLtJTi+Mmlf0EcTI7CWd80ibMVgtaCyuEl9mdPo9qf0uUHQvZjPo24H24yhinspNlWYkRz9KeqU+agaIz5VKL7Nqwf5YZMauCzsDvM6wurfAHBClRh/UfAvxHN7+Oeqz8QIDAQABAoIBAF/Vd1Gccf7bIwc4tKsuImqtkRRnO4O6DY/zfEDBETNbvUeOT0lzwZvKyRK2ssGyGTgD/FoM3AOUYMUsttA9pyf9+BB/sV5T8VPixyVnfjGR6nATW0v8X+eRYbC/s0q4ee7TzVl139y26dETv6drSjz2upo8DwdlZuxK116Fmpu+XiMc3u0QCbrnbFamhMXQoaYrYcU+pYYLc7Zzoc9ZN4szULZDDF6wkrktZbAuKyiERYF8vLNS2Gm6A6hMpVPpviSA48hhk4k4Gjn2oihEsvuqDf5Y8yI+5xASBLyqylvpZTNjp6B4V49MjyrY5DEecdpTf1kzFR6clAxaOmoCgCkCgYEA4t/5Ikbvyn3WdZK8Oyqlw9MxQKrZHz//s1OE+QV5Aqf5R3U30HaOGdImvocQb+Shgz7nV9fKN1hVdKNK6ASgQ0clhQGb6FrdxdyvPGrDmjDBvKULNM7rFbzeUSC5wHEHbZ8xkMY+sdai0R/sEk8FvUa+hzU3yyv5SmxJ2ESLjUMCgYEAxUl6Qaa5BO8qsrj33Px3Ag8umzLP7Dco+cPjR+b2D1Eg2RYvvr/+68gXB27nde5GEo+i8mIi9C2vfAsfbHJqLFBWX95gePMnRFoeWylNPqSz1+cGZJTsBWvptj7nHle5MYn8fe4JBLBIOGAUwPwA5SN5pMRkhU1+xT3mcUjRLLsCgYBw7ifm5gSKeOT9lVLY6LumpEOJ+wEkywiOzO4NvqmjptUwuqpTvA+zzqW2hSiradTzraYeVa20quWur3Gj2Fml445LjKd8m251BQq9Oi+vWsG1EzpmyPC/20mWfIG5xwl5iZp0hBnFEB/vlMI/wtIKi2Jfjx/8pCDs6MZBPq1wXQKBgG0lHmbtttRdAJFJtY7jeW+BOLaR4Of9CEVNsxLXWu/UYUjYdmegTobg9qSdHZ5nyQqBvpM76byO/dOxT5wunECR3YdCPrsLQoEVHlAuxFZQxlI+tJG2tfC15+F0YWau/3zBqxd8Ni8K25mcxj6R7GjYPHcEU9xPqD+05CVuNJL7AoGBAM1Xi7gWKaV7bYQdmW2HFfmAaW1z5DUPPZoVQ/wsZwSbBPf3Hn7qE6DM+dRMK6RBL1GIuRdxfCPOT4ktei9dvghM42gU2me8U5B6T5s1Sf99m/iw6svyoU5OAcIPSZ38R4l00VA1w+jYSMdQukhBlgYZQgRxeXKOFiM9IpkVvH9U";
            string ALIPAY_PUBLIC_KEY = @"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAlhGDVAL2+yN9HYkxgLBTI4MswpHi9QLrN/8zkJDFWQOGJEuUic2wPEJ/Mi45wuNMpeE57gzGMEk9AODTb+dzN5uA9gd0GVBcDI+5pBxJASWCouDUQR4Lvuo52vPTJGc2oageP5Of2L7eUazIYKU6jxoJWFbjzRN36p0l3SCvyvkOFaiTyIUFqDe5pprEPRWdxJmbwDnPsF4b3W1NdivoxP9y/ztCGN7ImX/mPLRrwveZe4xqvDOJ0sZ9LgfzJL7POE8lkj7m5LWM/5cNDKB3rtElp6eDWI7blBCvBsqMuzaCjzuYswYttu/j5gVLUd1oQ8v0wdw8lmPskcYEtobF6wIDAQAB";
            string CHARSET = "UTF-8";


            APPID = SettingManager.GetSettingValue(AppSettings.Alipay.APPID);
            PID = SettingManager.GetSettingValue(AppSettings.Alipay.PID);
            APP_PRIVATE_KEY = SettingManager.GetSettingValue(AppSettings.Alipay.APP_PRIVATE_KEY);
            ALIPAY_PUBLIC_KEY = SettingManager.GetSettingValue(AppSettings.Alipay.ALIPAY_PUBLIC_KEY);

            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", APPID, APP_PRIVATE_KEY, "json", "1.0", "RSA2", ALIPAY_PUBLIC_KEY, CHARSET, false);
            //实例化具体API对应的request类,类名称和接口名称对应,当前调用接口名称如：alipay.open.public.template.message.industry.modify 
            AlipayTradeWapPayRequest request = new AlipayTradeWapPayRequest();
            request.SetReturnUrl(SettingManager.GetSettingValue(AppSettings.Wuye.Host) + "/weixin/service/alipayreturn");
            request.SetNotifyUrl(SettingManager.GetSettingValue(AppSettings.Wuye.Host) + "/weixin/service/alipaynotify");

            string subject = "物业缴费";
            switch (type)
            {
                case 0:
                    subject = "物业缴费";
                    break;
                case 1:
                    subject = "商城团购";
                    break;
                case 2:
                    subject = "物业预缴";
                    break;
            }


            //SDK已经封装掉了公共参数，这里只需要传入业务参数
            //此次只是参数展示，未进行字符串转义，实际情况下请转义
            request.BizContent = "{" +
                    "    \"body\":\"" + type + "\"," +
                    "    \"out_trade_no\":\"" + orderNo + "\"," +
                    "    \"total_amount\":\"" + money + "\"," +
                    "    \"subject\":\"" + subject + "\"," +
                    "    \"seller_id\":\"" + PID + "\"," +
                    "    \"product_code\":\"QUICK_WAP_PAY\"" +
                    "  }";//填充业务参数
            var response = client.pageExecute(request);

            var temp = @"<html>
<head>
    <meta http-equiv='Content-Language' content='zh-CN'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no'>
    <meta HTTP-EQUIV='Content-Type' CONTENT='text/html; charset=utf-8'>
    <title>支付跳转中</title>
    <style>
        .spinner {
            width: 60px;
            height: 60px;
            position: relative;
            margin: 100px auto;
        }

        .double-bounce1, .double-bounce2 {
            width: 100%;
            height: 100%;
            border-radius: 50%;
            background-color: #67CF22;
            opacity: 0.6;
            position: absolute;
            top: 0;
            left: 0;
            -webkit-animation: bounce 2.0s infinite ease-in-out;
            animation: bounce 2.0s infinite ease-in-out;
        }

        .double-bounce2 {
            -webkit-animation-delay: -1.0s;
            animation-delay: -1.0s;
        }

        @@-webkit-keyframes bounce {
            0%, 100% {
                -webkit-transform: scale(0.0);
            }

            50% {
                -webkit-transform: scale(1.0);
            }
        }

        @@keyframes bounce {
            0%, 100% {
                transform: scale(0.0);
                -webkit-transform: scale(0.0);
            }

            50% {
                transform: scale(1.0);
                -webkit-transform: scale(1.0);
            }
        }
    </style>
</head>
<body>
    <div class='spinner'>
        <div class='double-bounce1'></div>
        <div class='double-bounce2'></div>
    </div>
    <center>
        <h3>支付跳转中,请稍候...</h3>
    </center><div style='display:none'>" + response.Body
    + @"</div></body>
</html>";

            return Content(temp, "text/html;charset=" + CHARSET);
        }

        [Route("service/alipayreturn")]
        public ActionResult AlipayReturn()
        {
            return View("Weixin/Service/AlipayReturn");
        }

        [Route("service/alipaynotify")]
        public ActionResult AlipayNotify()
        {

            string APPID = "2016081501749843";
            string PID = "2088421556051016";
            string APP_PRIVATE_KEY = @"MIIEowIBAAKCAQEArtd47la2jHyV5wbTkn/GtVPvTzTr0w6PxWYRY/dnscHVHyPd56Yc/NMJoavcMvJaH4lUzmTBcj3Nz8cv4gsVqYUqyojtukh9JBqW4njdC3IuggBH1Lmo/6cgaZjOzSEHcwyWNU0TcgfFj+y8Kd47YVdh9+xW9/M3nQn9MAaQgfHZbmLMzV5mg/IsB9JzCG963JSuMGq1CiEtYggllC+bhLtJTi+Mmlf0EcTI7CWd80ibMVgtaCyuEl9mdPo9qf0uUHQvZjPo24H24yhinspNlWYkRz9KeqU+agaIz5VKL7Nqwf5YZMauCzsDvM6wurfAHBClRh/UfAvxHN7+Oeqz8QIDAQABAoIBAF/Vd1Gccf7bIwc4tKsuImqtkRRnO4O6DY/zfEDBETNbvUeOT0lzwZvKyRK2ssGyGTgD/FoM3AOUYMUsttA9pyf9+BB/sV5T8VPixyVnfjGR6nATW0v8X+eRYbC/s0q4ee7TzVl139y26dETv6drSjz2upo8DwdlZuxK116Fmpu+XiMc3u0QCbrnbFamhMXQoaYrYcU+pYYLc7Zzoc9ZN4szULZDDF6wkrktZbAuKyiERYF8vLNS2Gm6A6hMpVPpviSA48hhk4k4Gjn2oihEsvuqDf5Y8yI+5xASBLyqylvpZTNjp6B4V49MjyrY5DEecdpTf1kzFR6clAxaOmoCgCkCgYEA4t/5Ikbvyn3WdZK8Oyqlw9MxQKrZHz//s1OE+QV5Aqf5R3U30HaOGdImvocQb+Shgz7nV9fKN1hVdKNK6ASgQ0clhQGb6FrdxdyvPGrDmjDBvKULNM7rFbzeUSC5wHEHbZ8xkMY+sdai0R/sEk8FvUa+hzU3yyv5SmxJ2ESLjUMCgYEAxUl6Qaa5BO8qsrj33Px3Ag8umzLP7Dco+cPjR+b2D1Eg2RYvvr/+68gXB27nde5GEo+i8mIi9C2vfAsfbHJqLFBWX95gePMnRFoeWylNPqSz1+cGZJTsBWvptj7nHle5MYn8fe4JBLBIOGAUwPwA5SN5pMRkhU1+xT3mcUjRLLsCgYBw7ifm5gSKeOT9lVLY6LumpEOJ+wEkywiOzO4NvqmjptUwuqpTvA+zzqW2hSiradTzraYeVa20quWur3Gj2Fml445LjKd8m251BQq9Oi+vWsG1EzpmyPC/20mWfIG5xwl5iZp0hBnFEB/vlMI/wtIKi2Jfjx/8pCDs6MZBPq1wXQKBgG0lHmbtttRdAJFJtY7jeW+BOLaR4Of9CEVNsxLXWu/UYUjYdmegTobg9qSdHZ5nyQqBvpM76byO/dOxT5wunECR3YdCPrsLQoEVHlAuxFZQxlI+tJG2tfC15+F0YWau/3zBqxd8Ni8K25mcxj6R7GjYPHcEU9xPqD+05CVuNJL7AoGBAM1Xi7gWKaV7bYQdmW2HFfmAaW1z5DUPPZoVQ/wsZwSbBPf3Hn7qE6DM+dRMK6RBL1GIuRdxfCPOT4ktei9dvghM42gU2me8U5B6T5s1Sf99m/iw6svyoU5OAcIPSZ38R4l00VA1w+jYSMdQukhBlgYZQgRxeXKOFiM9IpkVvH9U";
            string ALIPAY_PUBLIC_KEY = @"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAlhGDVAL2+yN9HYkxgLBTI4MswpHi9QLrN/8zkJDFWQOGJEuUic2wPEJ/Mi45wuNMpeE57gzGMEk9AODTb+dzN5uA9gd0GVBcDI+5pBxJASWCouDUQR4Lvuo52vPTJGc2oageP5Of2L7eUazIYKU6jxoJWFbjzRN36p0l3SCvyvkOFaiTyIUFqDe5pprEPRWdxJmbwDnPsF4b3W1NdivoxP9y/ztCGN7ImX/mPLRrwveZe4xqvDOJ0sZ9LgfzJL7POE8lkj7m5LWM/5cNDKB3rtElp6eDWI7blBCvBsqMuzaCjzuYswYttu/j5gVLUd1oQ8v0wdw8lmPskcYEtobF6wIDAQAB";
            string CHARSET = "UTF-8";


            APPID = SettingManager.GetSettingValue(AppSettings.Alipay.APPID);
            PID = SettingManager.GetSettingValue(AppSettings.Alipay.PID);
            APP_PRIVATE_KEY = SettingManager.GetSettingValue(AppSettings.Alipay.APP_PRIVATE_KEY);
            ALIPAY_PUBLIC_KEY = SettingManager.GetSettingValue(AppSettings.Alipay.ALIPAY_PUBLIC_KEY);

            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", APPID, APP_PRIVATE_KEY, "json", "1.0", "RSA2", ALIPAY_PUBLIC_KEY, CHARSET, false);

            var xx = this.Request.Params;
            //AlipaySignature.RSACheckV2(this.Request.fo, "");

            IDictionary<string, string> param = new Dictionary<string, string>();
            NameValueCollection myCol = this.Request.Form;
            string log = "";
            foreach (string key in myCol.Keys)
            {
                if (key == "sign_type") continue;
                if (!string.IsNullOrEmpty(log))
                    log += "&";
                log += $"{key}:{myCol[key]}";

                param.Add(key, myCol[key]);
            }
            Logger.Debug(log);


            string Request = log;
            string OrderNo = param.ContainsKey("out_trade_no") ? param["out_trade_no"] : "";// .IsNullOrEmpty() ? "NONE" : resHandler.GetParameter("out_trade_no");
            string res = "";

            if (!AlipaySignature.RSACheckV2(param, ALIPAY_PUBLIC_KEY, CHARSET, "RSA2", false))
            {
                Logger.Debug("支付宝签名验证失败");
                res = "fail";
            }
            else
            {
                Logger.Debug("支付宝签名验证成功");
                Logger.Debug("支付处理成功");
                string transaction_id = param["trade_no"];
                string openid = "";// resHandler.GetParameter("openid");
                int type = int.Parse(param["body"]);

                //正确的订单处理
                //_wuyeApiAppSrvice.CreateReceipt(OrderNo, transaction_id, param["total_amount"]);
                _payNotifyAppService.Notify(type, "支付宝", OrderNo, decimal.Parse(param["total_amount"]), Request, transaction_id);

                res = "success";
            }
            return Content(res);

        }


        #endregion

        #region 各种模板

        #region SHOPPING-CENTER-001

        [Route("shopping-center-001/index")]
        public async Task<ActionResult> SHOPPING_CENTER_001_Index()
        {
            var data = await _weixinAppSrvice.GetHomeData();
            var session = _iocManager.Resolve<WeixinSession>();
            ViewData["isLogin"] = session.MemberId != 0;
            ViewData["isBindRoom"] = session.IsBindRoom;
            ViewData["notices"] = _weixinAppSrvice.GetNotices(new GetPageListRequstDto() { CurrentPage = 1, PageSize = 5 }).Result.Items;
            return View("Templates/SHOPPING-CENTER-001/Index", data);
        }

        [Route("shopping-center-001/bill")]
        public async Task<ActionResult> SHOPPING_CENTER_001_Bill()
        {
            var session = _iocManager.Resolve<WeixinSession>();
            var data = _wuyeApiAppSrvice.GetBill(session.MemberId.ToString());
            if (!data.Success)
                return Redirect("/weixin/shopping-center-001/index");

            ViewData["url1"] = data.url1;
            ViewData["url2"] = data.url2;
            return View("Templates/SHOPPING-CENTER-001/Bill", data);
        }

        [Route("shopping-center-001/bill2")]
        public async Task<ActionResult> SHOPPING_CENTER_001_Bill2()
        {
            var session = _iocManager.Resolve<WeixinSession>();
            var data = _wuyeApiAppSrvice.GetBill2(session.MemberId.ToString());
            if (!data.Success)
                return Redirect("/weixin/shopping-center-001/index");

            ViewData["url1"] = data.url1;
            ViewData["url2"] = data.url2;
            return View("Templates/SHOPPING-CENTER-001/Bill", data);
        }

        [Route("shopping-center-001/check")]
        public async Task<ActionResult> SHOPPING_CENTER_001_Check()
        {
            var session = _iocManager.Resolve<WeixinSession>();
            var data = _wuyeApiAppSrvice.GetCheck(session.MemberId.ToString());
            if (!data.Success)
                return Redirect("/weixin/shopping-center-001/index");

            ViewData["url1"] = data.url1;
            ViewData["url2"] = data.url2;
            return View("Templates/SHOPPING-CENTER-001/Check", data);
        }


        [Route("shopping-center-001/my")]
        [WeixinPageAuthAttribute]
        public ActionResult SHOPPING_CENTER_001_My()
        {
            var data = _weixinAppSrvice.GetMy();
            return View("Templates/SHOPPING-CENTER-001/My/Index", data);
        }


        #endregion

        #endregion

        #region MyRegion

        [Route("api/workordernotice")]
        public ActionResult WorkOrderNotice()
        {

            try
            {
                long memberId = long.Parse(this.Request["memberId"]); //会员id
                string billCode = this.Request["billCode"]; //单据编号
                string title = this.Request["title"]; //标题
                string progress = this.Request["progress"]; //处理进度
                string person = this.Request["person"]; //处理人

                _templateMessageUtils.SendWorkOrderTemplateMessage(memberId, billCode, title, progress, person);
            }
            catch (UserFriendlyException ex)
            {
                return Content(new InvokeResult() { Message = ex.Message }.ToJsonString(true, true));
            }
            catch (Exception ex)
            {
                return Content(new InvokeResult() { Message = ex.Message }.ToJsonString(true, true));
            }

            return Content(new InvokeResult() { Success = true }.ToJsonString(true, true));
        }


        [Route("api/votenotice")]
        public ActionResult VoteNotice()
        {

            try
            {
                long memberId = long.Parse(this.Request["memberId"]); //会员id  
                string title = this.Request["title"]; //标题
                string name = this.Request["name"]; //投票议题
                string content = this.Request["content"]; //投票内容
                string range = this.Request["range"]; //投票范围
                string beginTime = this.Request["beginTime"]; //开始时间
                string endTime = this.Request["endTime"]; //结束时间
                string url = this.Request["url"]; //问卷url

                _templateMessageUtils.SendVoteTemplateMessage(memberId, title, name, content, range, beginTime, endTime, url);
            }
            catch (UserFriendlyException ex)
            {
                return Content(new InvokeResult() { Message = ex.Message }.ToJsonString(true, true));
            }
            catch (Exception ex)
            {
                return Content(new InvokeResult() { Message = ex.Message }.ToJsonString(true, true));
            }

            return Content(new InvokeResult() { Success = true }.ToJsonString(true, true));
        }

        #endregion
    }
}












