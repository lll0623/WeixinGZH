using System.Threading.Tasks;
using Abp.Configuration;
using LesoftWuye2.Application.Configuration.Dto;
using LesoftWuye2.Application.Utils;
using LesoftWuye2.Core.Configuration;

namespace LesoftWuye2.Application.Configuration
{
    public class SettingsAppService : LesoftWuye2AppServiceBase, ISettingsAppService
    {
        private readonly WeiXinApi _weiXinApi;
        public SettingsAppService(WeiXinApi weiXinApi)
        {
            _weiXinApi = weiXinApi;
        }


        public async Task<SettingsEditDto> GetAllSettings()
        {
            var settings = new SettingsEditDto
            {
                Wuye = new WuyeSettingsEditDto()
                {
                    ApiAddress = await SettingManager.GetSettingValueAsync(AppSettings.Wuye.ApiAddress),
                    AccountCode = await SettingManager.GetSettingValueAsync(AppSettings.Wuye.AccountCode),
                    Host = await SettingManager.GetSettingValueAsync(AppSettings.Wuye.Host),
                    EnabledPayFee = await SettingManager.GetSettingValueAsync(AppSettings.Wuye.EnabledPayFee)
                },
                Weixin = new WeixinSettingsEditDto()
                {
                    Token = await SettingManager.GetSettingValueAsync(AppSettings.Weixin.Token),
                    AppId = await SettingManager.GetSettingValueAsync(AppSettings.Weixin.AppId),
                    AppSecret = await SettingManager.GetSettingValueAsync(AppSettings.Weixin.AppSecret),
                    OriginalId = await SettingManager.GetSettingValueAsync(AppSettings.Weixin.OriginalId),
                    MchId = await SettingManager.GetSettingValueAsync(AppSettings.Weixin.MchId),
                    PayKey = await SettingManager.GetSettingValueAsync(AppSettings.Weixin.PayKey),

                    MenuService = await SettingManager.GetSettingValueAsync(AppSettings.Weixin.MenuService),
                    ServiceTemplate = await SettingManager.GetSettingValueAsync(AppSettings.Weixin.ServiceTemplate),
                    MenuMall = await SettingManager.GetSettingValueAsync(AppSettings.Weixin.MenuMall),
                    MallTemplate = await SettingManager.GetSettingValueAsync(AppSettings.Weixin.MallTemplate),
                    MenuMy = await SettingManager.GetSettingValueAsync(AppSettings.Weixin.MenuMy),
                    MyTemplate = await SettingManager.GetSettingValueAsync(AppSettings.Weixin.MyTemplate)
                },
                YsPay = new YsPaySettingsEditDto()
                {
                    PartnerId = await SettingManager.GetSettingValueAsync(AppSettings.YsPay.PartnerId),
                    SellerName = await SettingManager.GetSettingValueAsync(AppSettings.YsPay.SellerName),
                    KeyPassword = await SettingManager.GetSettingValueAsync(AppSettings.YsPay.KeyPassword)
                }
                ,
                Mall = new MallSettingsEditDto()
                {
                    ServiceTag = await SettingManager.GetSettingValueAsync(AppSettings.Mall.ServiceTag),
                    AdImage = await SettingManager.GetSettingValueAsync(AppSettings.Mall.AdImage),
                    AdUrl = await SettingManager.GetSettingValueAsync(AppSettings.Mall.AdUrl),
                    GrouponDescription = await SettingManager.GetSettingValueAsync(AppSettings.Mall.GrouponDescription),
                    AdImage2 = await SettingManager.GetSettingValueAsync(AppSettings.Mall.AdImage2),
                    AdUrl2 = await SettingManager.GetSettingValueAsync(AppSettings.Mall.AdUrl2),
                },
                Alipay = new AlipaySettingsEditDto()
                {
                    PID = await SettingManager.GetSettingValueAsync(AppSettings.Alipay.PID),
                    APPID = await SettingManager.GetSettingValueAsync(AppSettings.Alipay.APPID),
                    ALIPAY_PUBLIC_KEY = await SettingManager.GetSettingValueAsync(AppSettings.Alipay.ALIPAY_PUBLIC_KEY),
                    APP_PRIVATE_KEY = await SettingManager.GetSettingValueAsync(AppSettings.Alipay.APP_PRIVATE_KEY),
                }
            };

            return settings;
        }

        public async Task UpdateAllSettings(SettingsEditDto dto)
        {
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.Wuye.ApiAddress, dto.Wuye.ApiAddress);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.Wuye.AccountCode, dto.Wuye.AccountCode);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.Wuye.Host, dto.Wuye.Host);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.Wuye.EnabledPayFee, dto.Wuye.EnabledPayFee);


            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.Weixin.Token, dto.Weixin.Token);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.Weixin.AppId, dto.Weixin.AppId);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.Weixin.AppSecret, dto.Weixin.AppSecret);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.Weixin.OriginalId, dto.Weixin.OriginalId);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.Weixin.MchId, dto.Weixin.MchId);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.Weixin.PayKey, dto.Weixin.PayKey);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.Weixin.MenuService, dto.Weixin.MenuService);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.Weixin.ServiceTemplate, dto.Weixin.ServiceTemplate);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.Weixin.MenuMall, dto.Weixin.MenuMall);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.Weixin.MallTemplate, dto.Weixin.MenuMall);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.Weixin.MenuMy, dto.Weixin.MenuMy);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.Weixin.MyTemplate, dto.Weixin.MyTemplate);

            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.YsPay.PartnerId, dto.YsPay.PartnerId);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.YsPay.SellerName, dto.YsPay.SellerName);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.YsPay.KeyPassword, dto.YsPay.KeyPassword);

            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.Mall.ServiceTag, dto.Mall.ServiceTag);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.Mall.AdImage, dto.Mall.AdImage);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.Mall.AdUrl, dto.Mall.AdUrl);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.Mall.AdImage2, dto.Mall.AdImage2);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.Mall.AdUrl2, dto.Mall.AdUrl2);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.Mall.GrouponDescription, dto.Mall.GrouponDescription);

            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.Alipay.APPID, dto.Alipay.APPID);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.Alipay.APP_PRIVATE_KEY, dto.Alipay.APP_PRIVATE_KEY);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.Alipay.ALIPAY_PUBLIC_KEY, dto.Alipay.ALIPAY_PUBLIC_KEY);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.Alipay.PID, dto.Alipay.PID);
        }

        public void CreateWeixinMenu()
        {

            _weiXinApi.CreateMenu();
        }

       
    }


}
