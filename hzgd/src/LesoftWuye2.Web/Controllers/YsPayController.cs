using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Abp.Auditing;
using Abp.Configuration;
using Abp.Json;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Web.Models;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.Malls;
using LesoftWuye2.Application.Malls.Dto;
using LesoftWuye2.Application.PayNoifys;
using LesoftWuye2.Application.YsPay;
using LesoftWuye2.Core.Configuration;
using Obs;
using Obs.Dto;
using Obs.IO;

namespace LesoftWuye2.Web.Controllers
{
    public class YsPayController : LesoftWuye2ControllerBase
    {
        private readonly IPayNotifyAppService _payNotifyAppService;
        private readonly IMallService _mallService;
        public YsPayController(IPayNotifyAppService payNotifyAppService,
            IMallService mallService)
        {
            _payNotifyAppService = payNotifyAppService;
            _mallService = mallService;
        }

        public ActionResult Pay(string orderNo, decimal money)
        {
            string password = SettingManager.GetSettingValue(AppSettings.YsPay.KeyPassword);
            string priKey = Server.MapPath("/flyfoxglf.pfx");
            string pubKey = Server.MapPath("/businessgate.cer");
            string returnKey = Server.MapPath("/businessgate.cer");

            string method = "ysepay.online.wap.directpay.createbyuser";

            string server_url = "https://openapi.ysepay.com/gateway.do";


            string partner_id = SettingManager.GetSettingValue(AppSettings.YsPay.PartnerId);
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string charset = "utf-8";
            string sign_type = "RSA";
            string notify_url = Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port + "/YsPay/PayNotify";
            string return_url = Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port + "/YsPay/PayReturn";
            string version = "3.0";

            string out_trade_no = orderNo;
            string subject = "物业缴费";
            string total_amount = money.ToString("F2");
            string seller_id = SettingManager.GetSettingValue(AppSettings.YsPay.PartnerId);
            string seller_name = SettingManager.GetSettingValue(AppSettings.YsPay.SellerName);
            string timeout_express = "6m";
            string pay_mode = "internetbank";
            string bank_type = "1031000";
            string bank_account_type = "personal";
            string support_card_type = "debit";
            string extend_params = "";
            string business_code = "01000010";
            string extra_common_param = "23";

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("method", method);
            dic.Add("partner_id", partner_id);
            dic.Add("timestamp", timestamp);
            dic.Add("charset", charset);
            dic.Add("sign_type", sign_type);
            dic.Add("notify_url", notify_url);
            dic.Add("return_url", return_url);
            dic.Add("version", version);

            dic.Add("out_trade_no", out_trade_no);
            dic.Add("subject", subject);
            dic.Add("total_amount", total_amount);
            dic.Add("seller_id", seller_id);
            dic.Add("seller_name", seller_name);
            dic.Add("timeout_express", timeout_express);
            dic.Add("extend_params", extend_params);
            dic.Add("business_code", business_code);
            dic.Add("extra_common_param", extra_common_param);
            dic.Add("remark", "");

            dic = Common.SortDictionary(dic);
            string unsign = Common.CreateString(dic);
            string privatekey = SecurityUtil.loadKey(priKey, password, true);
            string sign = SecurityUtil.RSASignString(unsign, privatekey);

            dic.Add("sign", sign);

            string paras = Common.CreateLinkString(dic);

            StringBuilder Html = new StringBuilder();

            Html.Append("<form id='yssubmit' name='yssubmit' action='" + server_url + "' method='post'>");

            foreach (KeyValuePair<string, string> temp in dic)
            {
                Html.Append("<input type='hidden' name='" + temp.Key + "' value='" + temp.Value + "'/>");
            }

            Html.Append("<input type='submit' value='确认' style='display:none;'></form>");
            Html.Append("<script>document.forms['yssubmit'].submit();</script>");

            return Content(Html.ToString(), "text/html");
        }

        public ActionResult PayOrder(string orderNo, decimal money)
        {
            string password = SettingManager.GetSettingValue(AppSettings.YsPay.KeyPassword);
            string priKey = Server.MapPath("/flyfoxglf.pfx");
            string pubKey = Server.MapPath("/businessgate.cer");
            string returnKey = Server.MapPath("/businessgate.cer");

            string method = "ysepay.online.wap.directpay.createbyuser";

            string server_url = "https://openapi.ysepay.com/gateway.do";


            string partner_id = SettingManager.GetSettingValue(AppSettings.YsPay.PartnerId);
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string charset = "utf-8";
            string sign_type = "RSA";
            string notify_url = Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port + "/YsPay/PayOrderNotify";
            string return_url = Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port + "/YsPay/PayOrderReturn";
            string version = "3.0";

            //根据订单号获取商品信息

            string out_trade_no = orderNo;
            string subject = "拼团费用";
            string total_amount = money.ToString("F2");
            string seller_id = SettingManager.GetSettingValue(AppSettings.YsPay.PartnerId);
            string seller_name = SettingManager.GetSettingValue(AppSettings.YsPay.SellerName);
            string timeout_express = "6m";
            string pay_mode = "internetbank";
            string bank_type = "1031000";
            string bank_account_type = "personal";
            string support_card_type = "debit";
            string extend_params = "";
            string business_code = "01000010";
            string extra_common_param = "23";

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("method", method);
            dic.Add("partner_id", partner_id);
            dic.Add("timestamp", timestamp);
            dic.Add("charset", charset);
            dic.Add("sign_type", sign_type);
            dic.Add("notify_url", notify_url);
            dic.Add("return_url", return_url);
            dic.Add("version", version);

            dic.Add("out_trade_no", out_trade_no);
            dic.Add("subject", subject);
            dic.Add("total_amount", total_amount);
            dic.Add("seller_id", seller_id);
            dic.Add("seller_name", seller_name);
            dic.Add("timeout_express", timeout_express);
            dic.Add("extend_params", extend_params);
            dic.Add("business_code", business_code);
            dic.Add("extra_common_param", extra_common_param);
            dic.Add("remark", "");

            dic = Common.SortDictionary(dic);
            string unsign = Common.CreateString(dic);
            string privatekey = SecurityUtil.loadKey(priKey, password, true);
            string sign = SecurityUtil.RSASignString(unsign, privatekey);

            dic.Add("sign", sign);

            string paras = Common.CreateLinkString(dic);

            StringBuilder Html = new StringBuilder();

            Html.Append("<form id='yssubmit' name='yssubmit' action='" + server_url + "' method='post'>");

            foreach (KeyValuePair<string, string> temp in dic)
            {
                Html.Append("<input type='hidden' name='" + temp.Key + "' value='" + temp.Value + "'/>");
            }

            Html.Append("<input type='submit' value='确认' style='display:none;'></form>");
            Html.Append("<script>document.forms['yssubmit'].submit();</script>");

            return Content(Html.ToString(), "text/html");
        }

        public ActionResult PayReturn()
        {
            string[] strKeys = Request.Form.AllKeys;
            string total_amount = Request.Form["total_amount"];
            string out_trade_no = Request.Form["out_trade_no"];
            string trade_no = Request.Form["trade_no"];

            ViewData["total_amount"] = total_amount;
            ViewData["out_trade_no"] = out_trade_no;
            ViewData["trade_no"] = trade_no;

            return View();
        }

        public ActionResult PayOrderReturn()
        {
            string[] strKeys = Request.Form.AllKeys;
            string total_amount = Request.Form["total_amount"];
            string out_trade_no = Request.Form["out_trade_no"];
            string trade_no = Request.Form["trade_no"];

            ViewData["total_amount"] = total_amount;
            ViewData["out_trade_no"] = out_trade_no;
            ViewData["trade_no"] = trade_no;

            return View();
        }

        //public ActionResult PayNotify()
        //{

        //    string[] strKeys = Request.Form.AllKeys;
        //    string password = SettingManager.GetSettingValue(AppSettings.YsPay.KeyPassword);
        //    string priKey = Server.MapPath("/flyfoxglf.pfx");
        //    string pubKey = Server.MapPath("/businessgate.cer");
        //    string returnKey = Server.MapPath("/businessgate.cer");
        //    string partner_id = SettingManager.GetSettingValue(AppSettings.YsPay.PartnerId);
        //    string sign = "";

        //    Dictionary<string, string> dic = new Dictionary<string, string>();
        //    foreach (string strKey in strKeys)
        //    {
        //        if (strKey == "sign")
        //        {
        //            sign = Request.Form[strKey];
        //        }
        //        else
        //        {
        //            dic.Add(strKey, Request.Form[strKey]);
        //        }
        //    }

        //    dic = Common.SortDictionary(dic);
        //    string unSign = Common.CreateString(dic);

        //    string publickey = SecurityUtil.loadKey(returnKey, password, false);
        //    if (SecurityUtil.VerifySignedEncoding(unSign, sign, publickey))
        //    {
        //        _payNotifyAppService.Notify("银盛支付", Request.Form["out_trade_no"], decimal.Parse(Request.Form["total_amount"]), unSign, Request.Form["trade_no"]);
        //        return Content("SUCCESS");
        //    }
        //    return Content("FAIL");
        //}

        //public ActionResult PayOrderNotify()
        //{

        //    string[] strKeys = Request.Form.AllKeys;
        //    string password = SettingManager.GetSettingValue(AppSettings.YsPay.KeyPassword);
        //    string priKey = Server.MapPath("/flyfoxglf.pfx");
        //    string pubKey = Server.MapPath("/businessgate.cer");
        //    string returnKey = Server.MapPath("/businessgate.cer");
        //    string partner_id = SettingManager.GetSettingValue(AppSettings.YsPay.PartnerId);
        //    string sign = "";

        //    Dictionary<string, string> dic = new Dictionary<string, string>();
        //    foreach (string strKey in strKeys)
        //    {
        //        if (strKey == "sign")
        //        {
        //            sign = Request.Form[strKey];
        //        }
        //        else
        //        {
        //            dic.Add(strKey, Request.Form[strKey]);
        //        }
        //    }

        //    dic = Common.SortDictionary(dic);
        //    string unSign = Common.CreateString(dic);

        //    string publickey = SecurityUtil.loadKey(returnKey, password, false);
        //    if (SecurityUtil.VerifySignedEncoding(unSign, sign, publickey))
        //    {
        //        _mallService.PayOrder(new OrderPayInfo()
        //        {
        //            Channel = "银盛支付",
        //            Money = decimal.Parse(Request.Form["total_amount"]),
        //            OrderNo = Request.Form["out_trade_no"],
        //            TradeNo = Request.Form["trade_no"]
        //        });
        //        //_payNotifyAppService.Notify("银盛支付", Request.Form["out_trade_no"], decimal.Parse(Request.Form["total_amount"]), unSign, Request.Form["trade_no"]);
        //        return Content("SUCCESS");
        //    }
        //    return Content("FAIL");
        //}
    }

}