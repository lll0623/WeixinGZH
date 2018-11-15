using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;
using Obs;

namespace LesoftWuye2.Core.Authorization
{
    public class LesoftWuye2AuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //Common permissions
            var pages = context.GetPermissionOrNull(PermissionNames.Pages);
            if (pages == null)
            {
                pages = context.CreatePermission(PermissionNames.Pages, L("页面"));
            }

            var roles = pages.CreateChildPermission(Obs.Authorization.PermissionNames.Pages_Roles, L("角色"));
            roles.CreateChildPermission(Obs.Authorization.PermissionNames.Pages_Roles_Create, L("添加角色"));
            roles.CreateChildPermission(Obs.Authorization.PermissionNames.Pages_Roles_Update, L("修改角色"));
            roles.CreateChildPermission(Obs.Authorization.PermissionNames.Pages_Roles_Delete, L("删除角色"));

            var users = pages.CreateChildPermission(Obs.Authorization.PermissionNames.Pages_Users, L("系统用户"));
            users.CreateChildPermission(Obs.Authorization.PermissionNames.Pages_Users_Create, L("添加用户"));
            users.CreateChildPermission(Obs.Authorization.PermissionNames.Pages_Users_Update, L("修改用户"));
            users.CreateChildPermission(Obs.Authorization.PermissionNames.Pages_Users_Delete, L("删除用户"));

            pages.CreateChildPermission(Obs.Authorization.PermissionNames.Pages_AuditLogs, L("系统维护"));
            pages.CreateChildPermission(Obs.Authorization.PermissionNames.Pages_Maintenance, L("审计日志"));

            var settings = pages.CreateChildPermission(PermissionNames.Pages_Settings, L("参数设置"));
            settings.CreateChildPermission(PermissionNames.Pages_Settings_View, L("查看参数"));
            settings.CreateChildPermission(PermissionNames.Pages_Settings_Save, L("保存参数"));

            pages.CreateChildPermission(PermissionNames.Pages_ApiLogs, L("API调用日志"));


            var projects = pages.CreateChildPermission(PermissionNames.Pages_Projects, L("小区"));
            //projects.CreateChildPermission(PermissionNames.Pages_Projects_Create, L("添加小区"));
            //projects.CreateChildPermission(PermissionNames.Pages_Projects_Update, L("修改小区"));
            //projects.CreateChildPermission(PermissionNames.Pages_Projects_Delete, L("删除小区"));

            var notices = pages.CreateChildPermission(PermissionNames.Pages_Notices, L("小区公告"));
            //notices.CreateChildPermission(PermissionNames.Pages_Notices_Create, L("添加小区公告"));
            //notices.CreateChildPermission(PermissionNames.Pages_Notices_Update, L("修改小区公告"));
            //notices.CreateChildPermission(PermissionNames.Pages_Notices_Delete, L("删除小区公告"));

            var newss = pages.CreateChildPermission(PermissionNames.Pages_Newss, L("社区资讯"));
            //newss.CreateChildPermission(PermissionNames.Pages_Newss_Create, L("添加小区新闻"));
            //newss.CreateChildPermission(PermissionNames.Pages_Newss_Update, L("修改小区新闻"));
            //newss.CreateChildPermission(PermissionNames.Pages_Newss_Delete, L("删除小区新闻"));

            //var lifeInfoTypes = pages.CreateChildPermission(PermissionNames.Pages_LifeInfoTypes, L("生活信息分类"));
            //lifeInfoTypes.CreateChildPermission(PermissionNames.Pages_LifeInfoTypes_Create, L("添加生活信息分类"));
            //lifeInfoTypes.CreateChildPermission(PermissionNames.Pages_LifeInfoTypes_Update, L("修改生活信息分类"));
            //lifeInfoTypes.CreateChildPermission(PermissionNames.Pages_LifeInfoTypes_Delete, L("删除生活信息分类"));


            //var propertyTypes = pages.CreateChildPermission(PermissionNames.Pages_PropertyTypes, L("项目分类"));
            //propertyTypes.CreateChildPermission(PermissionNames.Pages_PropertyTypes_Create, L("添加项目分类"));
            //propertyTypes.CreateChildPermission(PermissionNames.Pages_PropertyTypes_Update, L("修改项目分类"));
            //propertyTypes.CreateChildPermission(PermissionNames.Pages_PropertyTypes_Delete, L("删除项目分类"));

            //var propertyCitys = pages.CreateChildPermission(PermissionNames.Pages_PropertyCitys, L("小区项目城市"));
            //propertyCitys.CreateChildPermission(PermissionNames.Pages_PropertyCitys_Create, L("添加小区项目城市"));
            //propertyCitys.CreateChildPermission(PermissionNames.Pages_PropertyCitys_Update, L("修改小区项目城市"));
            //propertyCitys.CreateChildPermission(PermissionNames.Pages_PropertyCitys_Delete, L("删除小区项目城市"));

            //var propertys = pages.CreateChildPermission(PermissionNames.Pages_Propertys, L("项目"));
            //propertys.CreateChildPermission(PermissionNames.Pages_Propertys_Create, L("添加项目"));
            //propertys.CreateChildPermission(PermissionNames.Pages_Propertys_Update, L("修改项目"));
            //propertys.CreateChildPermission(PermissionNames.Pages_Propertys_Delete, L("删除项目"));

            //var lifeInfos = pages.CreateChildPermission(PermissionNames.Pages_LifeInfos, L("生活信息"));
            //lifeInfos.CreateChildPermission(PermissionNames.Pages_LifeInfos_Create, L("添加生活信息"));
            //lifeInfos.CreateChildPermission(PermissionNames.Pages_LifeInfos_Update, L("修改生活信息"));
            //lifeInfos.CreateChildPermission(PermissionNames.Pages_LifeInfos_Delete, L("删除生活信息"));

            var activitys = pages.CreateChildPermission(PermissionNames.Pages_Activitys, L("小区活动"));
            //activitys.CreateChildPermission(PermissionNames.Pages_Activitys_Create, L("添加小区活动"));
            //activitys.CreateChildPermission(PermissionNames.Pages_Activitys_Update, L("修改小区活动"));
            //activitys.CreateChildPermission(PermissionNames.Pages_Activitys_Delete, L("删除小区活动"));

            var slideImages = pages.CreateChildPermission(PermissionNames.Pages_SlideImages, L("轮播图"));
            //slideImages.CreateChildPermission(PermissionNames.Pages_SlideImages_Create, L("添加轮播图"));
            //slideImages.CreateChildPermission(PermissionNames.Pages_SlideImages_Update, L("修改轮播图"));
            //slideImages.CreateChildPermission(PermissionNames.Pages_SlideImages_Delete, L("删除轮播图"));

            //var serviceTels = pages.CreateChildPermission(PermissionNames.Pages_ServiceTels, L("服务电话"));
            //serviceTels.CreateChildPermission(PermissionNames.Pages_ServiceTels_Create, L("添加服务电话"));
            //serviceTels.CreateChildPermission(PermissionNames.Pages_ServiceTels_Update, L("修改服务电话"));
            //serviceTels.CreateChildPermission(PermissionNames.Pages_ServiceTels_Delete, L("删除服务电话"));

            var substations = pages.CreateChildPermission(PermissionNames.Pages_Substations, L("关于我们"));
            //substations.CreateChildPermission(PermissionNames.Pages_Substations_Create, L("添加关于我们"));
            //substations.CreateChildPermission(PermissionNames.Pages_Substations_Update, L("修改关于我们"));
            //substations.CreateChildPermission(PermissionNames.Pages_Substations_Delete, L("删除关于我们"));

            var estateinfos = pages.CreateChildPermission(PermissionNames.Pages_Estateinfos, L("跳蚤信息"));
            //estateinfos.CreateChildPermission(PermissionNames.Pages_Estateinfos_Create, L("添加跳蚤信息"));
            //estateinfos.CreateChildPermission(PermissionNames.Pages_Estateinfos_Update, L("修改跳蚤信息"));
            //estateinfos.CreateChildPermission(PermissionNames.Pages_Estateinfos_Delete, L("删除跳蚤信息"));


            var estateinfoTypes = pages.CreateChildPermission(PermissionNames.Pages_EstateinfoTypes, L("跳蚤信息分类"));
            //estateinfoTypes.CreateChildPermission(PermissionNames.Pages_EstateinfoTypes_Create, L("添加跳蚤信息分类"));
            //estateinfoTypes.CreateChildPermission(PermissionNames.Pages_EstateinfoTypes_Update, L("修改跳蚤信息分类"));
            //estateinfoTypes.CreateChildPermission(PermissionNames.Pages_EstateinfoTypes_Delete, L("删除跳蚤信息分类"));

            var rentsaleinfos = pages.CreateChildPermission(PermissionNames.Pages_Rentsaleinfos, L("租售信息"));
            //rentsaleinfos.CreateChildPermission(PermissionNames.Pages_Rentsaleinfos_Create, L("添加租售信息"));
            //rentsaleinfos.CreateChildPermission(PermissionNames.Pages_Rentsaleinfos_Update, L("修改租售信息"));
            //rentsaleinfos.CreateChildPermission(PermissionNames.Pages_Rentsaleinfos_Delete, L("删除租售信息"));


            var rentsaleinfoTypes = pages.CreateChildPermission(PermissionNames.Pages_RentsaleinfoTypes, L("租售信息分类"));
            //rentsaleinfoTypes.CreateChildPermission(PermissionNames.Pages_RentsaleinfoTypes_Create, L("添加租售信息分类"));
            //rentsaleinfoTypes.CreateChildPermission(PermissionNames.Pages_RentsaleinfoTypes_Update, L("修改租售信息分类"));
            //rentsaleinfoTypes.CreateChildPermission(PermissionNames.Pages_RentsaleinfoTypes_Delete, L("删除租售信息分类"));

            var suppliers = pages.CreateChildPermission(PermissionNames.Pages_Suppliers, L("供应商"));
            //suppliers.CreateChildPermission(PermissionNames.Pages_Suppliers_Create, L("添加供应商"));
            //suppliers.CreateChildPermission(PermissionNames.Pages_Suppliers_Update, L("修改供应商"));
            //suppliers.CreateChildPermission(PermissionNames.Pages_Suppliers_Delete, L("删除供应商"));

            var categorys = pages.CreateChildPermission(PermissionNames.Pages_Categorys, L("商品分类"));
            //categorys.CreateChildPermission(PermissionNames.Pages_Categorys_Create, L("添加商品分类"));
            //categorys.CreateChildPermission(PermissionNames.Pages_Categorys_Update, L("修改商品分类"));
            //categorys.CreateChildPermission(PermissionNames.Pages_Categorys_Delete, L("删除商品分类"));

            var products = pages.CreateChildPermission(PermissionNames.Pages_Products, L("商品"));
            //products.CreateChildPermission(PermissionNames.Pages_Products_Create, L("添加商品"));
            //products.CreateChildPermission(PermissionNames.Pages_Products_Update, L("修改商品"));
            //products.CreateChildPermission(PermissionNames.Pages_Products_Delete, L("删除商品"));


            var groupons = pages.CreateChildPermission(PermissionNames.Pages_Groupons, L("团购"));
            //groupons.CreateChildPermission(PermissionNames.Pages_Groupons_Create, L("添加团购"));
            //groupons.CreateChildPermission(PermissionNames.Pages_Groupons_Update, L("修改团购"));
            //groupons.CreateChildPermission(PermissionNames.Pages_Groupons_Delete, L("删除团购"));


            var mallslideimages = pages.CreateChildPermission(PermissionNames.Pages_MallSlideImages, L("商城轮播图"));
            //mallslideimages.CreateChildPermission(PermissionNames.Pages_MallSlideImages_Create, L("添加商城轮播图"));
            //mallslideimages.CreateChildPermission(PermissionNames.Pages_MallSlideImages_Update, L("修改商城轮播图"));
            //mallslideimages.CreateChildPermission(PermissionNames.Pages_MallSlideImages_Delete, L("删除商城轮播图"));


            var plates = pages.CreateChildPermission(PermissionNames.Pages_Plates, L("论坛版块"));
            //plates.CreateChildPermission(PermissionNames.Pages_Plates_Create, L("添加论坛版块"));
            //plates.CreateChildPermission(PermissionNames.Pages_Plates_Update, L("修改论坛版块"));
            //plates.CreateChildPermission(PermissionNames.Pages_Plates_Delete, L("删除论坛版块"));

            var froumPosts = pages.CreateChildPermission(PermissionNames.Pages_ForumPosts, L("论坛帖子"));
            var orders = pages.CreateChildPermission(PermissionNames.Pages_Orders, L("订单列表"));
            var refundOrders = pages.CreateChildPermission(PermissionNames.Pages_RefundOrders, L("退款列表"));
            var productComments = pages.CreateChildPermission(PermissionNames.Pages_ProductComments, L("商品评论管理"));
            var weixinSubscribes = pages.CreateChildPermission(PermissionNames.Pages_WeixinSubscribes, L("关注回复"));

            var feeServices = pages.CreateChildPermission(PermissionNames.Pages_FeeServices, L("有偿服务"));
        }

        private static ILocalizableString L(string name)
        {
            return new FixedLocalizableString(name);
        }
    }
}
