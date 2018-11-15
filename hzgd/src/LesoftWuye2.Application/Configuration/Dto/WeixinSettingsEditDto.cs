namespace LesoftWuye2.Application.Configuration.Dto
{
    public class WeixinSettingsEditDto
    {
        public string Token { get; set; }

        public string AppId { get; set; }

        public string AppSecret { get; set; }

        public string MchId { get; set; }

        public string PayKey { get; set; }

        public string OriginalId { get; set; }


        public  string MenuService { get; set; }
        public  string ServiceTemplate { get; set; }
        public  string MenuMall { get; set; }
        public  string MallTemplate { get; set; }
        public  string MenuMy { get; set; }
        public  string MyTemplate { get; set; }
    }
}
