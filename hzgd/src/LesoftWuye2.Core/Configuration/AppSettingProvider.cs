using System.Collections.Generic;
using Abp.Configuration;

namespace LesoftWuye2.Core.Configuration
{
    public class AppSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
                  {
                    new SettingDefinition(AppSettings.Weixin.Token,"Lesoft"),
                    new SettingDefinition(AppSettings.Weixin.AppId,""),
                    new SettingDefinition(AppSettings.Weixin.AppSecret,""),
                    new SettingDefinition(AppSettings.Weixin.MchId,""),
                    new SettingDefinition(AppSettings.Weixin.PayKey,""),
                    new SettingDefinition(AppSettings.Weixin.OriginalId,""),

                    new SettingDefinition(AppSettings.Weixin.MenuService,""),
                    new SettingDefinition(AppSettings.Weixin.ServiceTemplate,""),
                    new SettingDefinition(AppSettings.Weixin.MenuMall,""),
                    new SettingDefinition(AppSettings.Weixin.MallTemplate,""),
                    new SettingDefinition(AppSettings.Weixin.MenuMy,""),
                    new SettingDefinition(AppSettings.Weixin.MyTemplate,""),

                    //new SettingDefinition(AppSettings.Weixin.WorkOrderTemplateKey,"OPENTM201820050"),
                    //new SettingDefinition(AppSettings.Weixin.WorkOrderTemplateId,""),

                    //new SettingDefinition(AppSettings.Weixin.VoteTemplateKey,"OPENTM400166107"),
                    //new SettingDefinition(AppSettings.Weixin.VoteTemplateId,""),


                    new SettingDefinition(AppSettings.Wuye.ApiAddress,""),
                    new SettingDefinition(AppSettings.Wuye.AccountCode,""),
                    new SettingDefinition(AppSettings.Wuye.Host,""),
                    new SettingDefinition(AppSettings.Wuye.EnabledPayFee,""),

                    new SettingDefinition(AppSettings.YsPay.PartnerId,""),
                    new SettingDefinition(AppSettings.YsPay.SellerName,""),
                    new SettingDefinition(AppSettings.YsPay.KeyPassword,""),

                    new SettingDefinition(AppSettings.Mall.ServiceTag,""),
                    new SettingDefinition(AppSettings.Mall.AdImage,""),
                    new SettingDefinition(AppSettings.Mall.AdUrl,""),
                    new SettingDefinition(AppSettings.Mall.GrouponDescription,""),
                    new SettingDefinition(AppSettings.Mall.AdImage2,""),
                    new SettingDefinition(AppSettings.Mall.AdUrl2,""),

                     new SettingDefinition(AppSettings.Alipay.ALIPAY_PUBLIC_KEY,""),
                    new SettingDefinition(AppSettings.Alipay.APPID,""),
                    new SettingDefinition(AppSettings.Alipay.APP_PRIVATE_KEY,""),
                    new SettingDefinition(AppSettings.Alipay.PID,""),

                    new SettingDefinition(AppSettings.TemplateKey.WorkOrder,"OPENTM201820050"),
                    new SettingDefinition(AppSettings.TemplateKey.Vote,"OPENTM400166107"),
                    new SettingDefinition(AppSettings.TemplateKey.CreateGroupon,"OPENTM409244564"),
                    new SettingDefinition(AppSettings.TemplateKey.JoinGroupon,"OPENTM409244558"),
                    new SettingDefinition(AppSettings.TemplateKey.GrouponSuccess,"OPENTM409244552"),
                    new SettingDefinition(AppSettings.TemplateKey.GrouponFail,"OPENTM401113750"),
                    new SettingDefinition(AppSettings.TemplateKey.OrderShip,"OPENTM406496885"),
                    new SettingDefinition(AppSettings.TemplateKey.RefundOrderReject,"TM00432"),
                    new SettingDefinition(AppSettings.TemplateKey.RefundOrderAccept,"OPENTM202735558"),

                };
        }
    }
}
