using System.ComponentModel.DataAnnotations;

namespace LesoftWuye2.Application.Configuration.Dto
{
    public class SettingsEditDto
    {
        [Required]
        public WeixinSettingsEditDto Weixin { get; set; }

        [Required]
        public WuyeSettingsEditDto Wuye { get; set; }

        [Required]
        public YsPaySettingsEditDto YsPay { get; set; }
        
        [Required]
        public MallSettingsEditDto Mall { get; set; }

        [Required]
        public AlipaySettingsEditDto Alipay { get; set; }
       
    }
}
