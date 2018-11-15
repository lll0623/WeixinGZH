namespace LesoftWuye2.Core.Configuration
{
    public static class AppSettings
    {
        public static class Weixin
        {
            public const string Token = "App.Weixin.Token";
            public const string AppId = "App.Weixin.AppId";
            public const string AppSecret = "App.Weixin.AppSecret";
            public const string MchId = "App.Weixin.MchId";
            public const string PayKey = "App.Weixin.PayKey";
            public const string OriginalId = "App.Weixin.OriginalId";
            public const string MenuService = "App.Weixin.MenuService";
            public const string ServiceTemplate = "App.Weixin.ServiceTemplate";
            public const string MenuMall = "App.Weixin.MenuMall";
            public const string MallTemplate = "App.Weixin.MallTemplate";
            public const string MenuMy = "App.Weixin.MenuMy";
            public const string MyTemplate = "App.Weixin.MyTemplate";


            //public const string WorkOrderTemplateKey = "App.Weixin.WorkOrderTemplateKey";//工单模板消息key(固定的)
            //public const string WorkOrderTemplateId = "App.Weixin.WorkOrderTemplateId";//工单模板消息id（每个微信号不一样）

            //public const string VoteTemplateKey = "App.Weixin.VoteTemplateKey";//投票通知模板消息key(固定的)
            //public const string VoteTemplateId = "App.Weixin.VoteTemplateId";//投票通知模板消息id（每个微信号不一样）

        }

        public static class Wuye
        {
            public const string ApiAddress = "App.Wuye.ApiAddress";
            public const string AccountCode = "App.Wuye.AccountCode";
            public const string Host = "App.Wuye.Host";
            public const string EnabledPayFee = "App.Wuye.EnabledPayFee";
        }

        public static class YsPay
        {
            public const string PartnerId = "App.YsPay.PartnerId";
            public const string SellerName = "App.YsPay.SellerName";
            public const string KeyPassword = "App.YsPay.KeyPassword";
        }

        public static class Mall
        {
            public const string ServiceTag = "App.Mall.ServiceTag";
            public const string AdImage = "App.Mall.AdImage";
            public const string AdUrl = "App.Mall.AdUrl";
            public const string GrouponDescription = "App.Mall.GrouponDescription";
            public const string AdImage2 = "App.Mall.AdImage2";
            public const string AdUrl2 = "App.Mall.AdUrl2";

        }

        public static class Alipay
        {
            public const string APPID = "App.Alipay.APPID";
            public const string PID = "App.Alipay.PID";
            public const string APP_PRIVATE_KEY = "App.Alipay.APP_PRIVATE_KEY";
            public const string ALIPAY_PUBLIC_KEY = "App.Alipay.ALIPAY_PUBLIC_KEY";
        }

        public static class TemplateKey
        {
            public const string WorkOrder = "App.TemplateKey.WorkOrder";
            public const string Vote = "App.TemplateKey.Vote";
            public const string CreateGroupon = "App.TemplateKey.CreateGroupon";
            public const string JoinGroupon = "App.TemplateKey.JoinGroupon";
            public const string GrouponSuccess = "App.TemplateKey.GrouponSuccess";
            public const string GrouponFail = "App.TemplateKey.GrouponFail";
            public const string OrderShip = "App.TemplateKey.OrderShip";
            public const string RefundOrderReject = "App.TemplateKey.RefundOrderReject";
            public const string RefundOrderAccept = "App.TemplateKey.RefundOrderAccept";

        }
    }
}
