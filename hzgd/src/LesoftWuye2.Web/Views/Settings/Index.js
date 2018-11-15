var tUploader = new ThumbnailUploader("uploadImage", "removeImage", "thumbnailImg", "thumbnail", "/Content/img/placehold/640x150.png", 320, 75);
var tUploader2 = new ThumbnailUploader("uploadImage2", "removeImage2", "thumbnailImg2", "thumbnail2", "/Content/img/placehold/640x150.png", 320, 75);

(function () {
    $(function () {

        app.pageButtonPermissionProcess();

        
        $('#saveSettings').click(function (e) {

            var settings = {};
            settings.Wuye = {};
            settings.Weixin = {};
            settings.YsPay = {};
            settings.Mall = {};
            settings.Alipay = {};


            settings.Wuye.ApiAddress = $('#Wuye-ApiAddress').val();
            settings.Wuye.AccountCode = $('#Wuye-AccountCode').val();
            settings.Wuye.Host = $('#Wuye-Host').val();
            settings.Wuye.EnabledPayFee = $('#Wuye-EnabledPayFee').is(':checked');


            settings.Weixin.Token = $('#Weixin-Token').val();
            settings.Weixin.AppId = $('#Weixin-AppId').val();
            settings.Weixin.AppSecret = $('#Weixin-AppSecret').val();
            settings.Weixin.MchId = $('#Weixin-MchId').val();
            settings.Weixin.PayKey = $('#Weixin-PayKey').val();
            settings.Weixin.OriginalId = $('#Weixin-OriginalId').val();

            settings.Weixin.MenuService = $('#Weixin-MenuService').is(':checked');
            settings.Weixin.ServiceTemplate = $('#Weixin-ServiceTemplate').val();
            settings.Weixin.MenuMall = $('#Weixin-MenuMall').is(':checked');
            settings.Weixin.MallTemplate = $('#Weixin-MallTemplate').val();
            settings.Weixin.MenuMy = $('#Weixin-MenuMy').is(':checked');
            settings.Weixin.MyTemplate = $('#Weixin-MyTemplate').val();

            settings.Alipay.APPID = $('#Alipay-APPID').val();
            settings.Alipay.PID = $('#Alipay-PID').val();
            settings.Alipay.ALIPAY_PUBLIC_KEY = $('#Alipay-ALIPAY_PUBLIC_KEY').val();
            settings.Alipay.APP_PRIVATE_KEY = $('#Alipay-APP_PRIVATE_KEY').val();
            
            settings.YsPay.PartnerId = $('#YsPay-PartnerId').val();
            settings.YsPay.SellerName = $('#YsPay-SellerName').val();
            settings.YsPay.KeyPassword = $('#YsPay-KeyPassword').val();

            settings.Mall.ServiceTag = $('#Mall-ServiceTag').val();
            settings.Mall.AdImage = $('#thumbnail').val();
            settings.Mall.AdUrl = $('#Mall-AdUrl').val();
            settings.Mall.AdImage2 = $('#thumbnail2').val();
            settings.Mall.AdUrl2 = $('#Mall-AdUrl2').val();
            settings.Mall.GrouponDescription = $('#Mall-GrouponDescription').val();


            abp.services.app.settings.updateAllSettings(settings).done(function (result) {
                abp.message.success("所有配置保存成功!");
            }).always(function () {

            });

        });


        $('#createWeixinMenu').click(function (e) {

            abp.message.confirm("确认创建微信菜单吗?", function (isConfirmed) {
                if (isConfirmed) {
                    abp.services.app.settings.createWeixinMenu().done(function (result) {
                        abp.message.success("创建微信菜单成功!");
                    });
                }
            }
        );
            
        });

        $('#initTemplateMsg').click(function (e) {

            abp.message.confirm("确认初始化模版消息?", function (isConfirmed) {
                if (isConfirmed) {
                    abp.services.app.settings.initTemplateMsg().done(function (result) {
                        abp.message.success("初始化模版消息成功!");
                    });
                }
            }
     );

        });

    });
})();