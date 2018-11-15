namespace LesoftWuye2.Web.Models
{
    public class WxPayModel
    {
        public string appId { get; set; }

        public string timeStamp { get; set; }

        public string nonceStr { get; set; }

        public string package { get; set; }

        public string paySign { get; set; }

        public string returnUrl { get; set; }

    }
}