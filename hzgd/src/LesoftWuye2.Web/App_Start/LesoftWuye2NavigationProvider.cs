using Abp.Application.Navigation;
using Abp.Localization;
using Obs;
using Obs.Authorization;

namespace LesoftWuye2.Web
{
    /// <summary>
    /// This class defines menus for the application.
    /// It uses ABP's menu system.
    /// When you add menu items here, they are automatically appear in angular application.
    /// See Views/Layout/_TopMenu.cshtml file to know how to render menu.
    /// </summary>
    public class LesoftWuye2NavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {

            var homeMenu = new MenuItemDefinition("Home", new FixedLocalizableString("首页"), url: "", icon: "fa fa-home", requiresAuthentication: true);

            var baseMenu = new MenuItemDefinition("Base", new FixedLocalizableString("基础数据"), url: "", icon: "fa  fa-puzzle-piece", requiresAuthentication: true);
            baseMenu.AddItem(new MenuItemDefinition("Projects", new FixedLocalizableString("小区数据"), url: "Projects", icon: "fa fa-building-o", requiredPermissionName: LesoftWuye2.Core.Authorization.PermissionNames.Pages_Projects));
            //baseMenu.AddItem(new MenuItemDefinition("LifeInfoTypes", new FixedLocalizableString("生活信息分类"), url: "LifeInfoTypes", icon: "fa  fa-inbox", requiredPermissionName: LesoftWuye2.Core.Authorization.PermissionNames.Pages_LifeInfoTypes));
            //baseMenu.AddItem(new MenuItemDefinition("PropertyCitys", new FixedLocalizableString("项目城市"), url: "PropertyCitys", icon: "fa fa-flag", requiredPermissionName: LesoftWuye2.Core.Authorization.PermissionNames.Pages_PropertyCitys));
            //baseMenu.AddItem(new MenuItemDefinition("PropertyTypes", new FixedLocalizableString("项目分类"), url: "PropertyTypes", icon: "fa fa-flag-checkered", requiredPermissionName: LesoftWuye2.Core.Authorization.PermissionNames.Pages_PropertyTypes));
            baseMenu.AddItem(new MenuItemDefinition("SlideImages", new FixedLocalizableString("轮播图"), url: "SlideImages", icon: "fa fa-picture-o", requiredPermissionName: LesoftWuye2.Core.Authorization.PermissionNames.Pages_SlideImages));
            //baseMenu.AddItem(new MenuItemDefinition("ServiceTels", new FixedLocalizableString("服务电话"), url: "ServiceTels", icon: "fa fa-phone", requiredPermissionName: LesoftWuye2.Core.Authorization.PermissionNames.Pages_ServiceTels));
            baseMenu.AddItem(new MenuItemDefinition("EstateinfoTypes", new FixedLocalizableString("跳蚤信息分类"), url: "EstateinfoTypes", icon: "fa  fa-inbox", requiredPermissionName: LesoftWuye2.Core.Authorization.PermissionNames.Pages_EstateinfoTypes));
            baseMenu.AddItem(new MenuItemDefinition("RentsaleinfoTypes", new FixedLocalizableString("租售信息分类"), url: "RentsaleinfoTypes", icon: "fa  fa-suitcase", requiredPermissionName: LesoftWuye2.Core.Authorization.PermissionNames.Pages_RentsaleinfoTypes));
            baseMenu.AddItem(new MenuItemDefinition("FeeServices", new FixedLocalizableString("有偿服务"), url: "FeeServices", icon: "fa fa-volume-up", requiredPermissionName: LesoftWuye2.Core.Authorization.PermissionNames.Pages_FeeServices));
            baseMenu.AddItem(new MenuItemDefinition("WeixinSubscribes", new FixedLocalizableString("关注回复"), url: "WeixinSubscribes", icon: "fa  fa-suitcase", requiredPermissionName: LesoftWuye2.Core.Authorization.PermissionNames.Pages_WeixinSubscribes));



            var articlesMenu = new MenuItemDefinition("Aricle", new FixedLocalizableString("物业资讯"), url: "", icon: "fa  fa-info-circle", requiresAuthentication: true);
            articlesMenu.AddItem(new MenuItemDefinition("Notices", new FixedLocalizableString("社区公告"), url: "Notices", icon: "fa fa-volume-up", requiredPermissionName: LesoftWuye2.Core.Authorization.PermissionNames.Pages_Projects));
            articlesMenu.AddItem(new MenuItemDefinition("Newss", new FixedLocalizableString("社区资讯"), url: "Newss", icon: "fa  fa-copy", requiredPermissionName: LesoftWuye2.Core.Authorization.PermissionNames.Pages_Newss));
            //articlesMenu.AddItem(new MenuItemDefinition("LifeInfos", new FixedLocalizableString("生活信息"), url: "LifeInfos", icon: "fa   fa-tag", requiredPermissionName: LesoftWuye2.Core.Authorization.PermissionNames.Pages_LifeInfos));
            //articlesMenu.AddItem(new MenuItemDefinition("Propertys", new FixedLocalizableString("物业项目"), url: "Propertys", icon: "fa  fa-bookmark-o", requiredPermissionName: LesoftWuye2.Core.Authorization.PermissionNames.Pages_Propertys));
            articlesMenu.AddItem(new MenuItemDefinition("Activitys", new FixedLocalizableString("社区活动"), url: "Activitys", icon: "fa  fa-heart", requiredPermissionName: LesoftWuye2.Core.Authorization.PermissionNames.Pages_Activitys));
            articlesMenu.AddItem(new MenuItemDefinition("Substations", new FixedLocalizableString("关于我们"), url: "Substations", icon: "fa fa-phone", requiredPermissionName: LesoftWuye2.Core.Authorization.PermissionNames.Pages_Substations));
            articlesMenu.AddItem(new MenuItemDefinition("Estateinfos", new FixedLocalizableString("跳蚤信息"), url: "Estateinfos", icon: "fa fa-shopping-cart", requiredPermissionName: LesoftWuye2.Core.Authorization.PermissionNames.Pages_Estateinfos));
            articlesMenu.AddItem(new MenuItemDefinition("Rentsaleinfos", new FixedLocalizableString("租售信息"), url: "Rentsaleinfos", icon: "fa fa-suitcase", requiredPermissionName: LesoftWuye2.Core.Authorization.PermissionNames.Pages_Rentsaleinfos));

            var mallMenu = new MenuItemDefinition("Mall", new FixedLocalizableString("电商模块"), url: "", icon: "fa fa-leaf", requiresAuthentication: true);
            mallMenu.AddItem(new MenuItemDefinition("MallSlideImages", new FixedLocalizableString("商城轮播图管理"), url: "MallSlideImages", icon: "fa fa-picture-o", requiredPermissionName: LesoftWuye2.Core.Authorization.PermissionNames.Pages_MallSlideImages));
            mallMenu.AddItem(new MenuItemDefinition("Suppliers", new FixedLocalizableString("供应商"), url: "Suppliers", icon: "fa fa-shopping-cart", requiredPermissionName: LesoftWuye2.Core.Authorization.PermissionNames.Pages_Suppliers));
            mallMenu.AddItem(new MenuItemDefinition("Categorys", new FixedLocalizableString("商品分类"), url: "Categorys", icon: "fa fa-sitemap", requiredPermissionName: LesoftWuye2.Core.Authorization.PermissionNames.Pages_Categorys));
            mallMenu.AddItem(new MenuItemDefinition("Products", new FixedLocalizableString("商品管理"), url: "Products", icon: "fa fa-bullhorn", requiredPermissionName: LesoftWuye2.Core.Authorization.PermissionNames.Pages_Products));
            mallMenu.AddItem(new MenuItemDefinition("ProductComments", new FixedLocalizableString("商品评论管理"), url: "ProductComments", icon: "fa fa-comments-o", requiredPermissionName: LesoftWuye2.Core.Authorization.PermissionNames.Pages_ProductComments));
            mallMenu.AddItem(new MenuItemDefinition("Groupons", new FixedLocalizableString("团购管理"), url: "Groupons", icon: "fa fa-gift", requiredPermissionName: LesoftWuye2.Core.Authorization.PermissionNames.Pages_Groupons));
            mallMenu.AddItem(new MenuItemDefinition("Orders", new FixedLocalizableString("订单列表"), url: "Order", icon: "fa fa-calendar-o", requiredPermissionName: LesoftWuye2.Core.Authorization.PermissionNames.Pages_Orders));
            mallMenu.AddItem(new MenuItemDefinition("RefundOrders", new FixedLocalizableString("退款列表"), url: "RefundOrder", icon: "fa fa-calendar-o", requiredPermissionName: LesoftWuye2.Core.Authorization.PermissionNames.Pages_RefundOrders));

            var forumMenu = new MenuItemDefinition("Forum", new FixedLocalizableString("业主论坛"), url: "", icon: "fa  fa-book", requiresAuthentication: true);
            forumMenu.AddItem(new MenuItemDefinition("Plates", new FixedLocalizableString("论坛版块"), url: "Plates", icon: "fa  fa-film", requiredPermissionName: LesoftWuye2.Core.Authorization.PermissionNames.Pages_Plates));
             forumMenu.AddItem(new MenuItemDefinition("ForumPosts", new FixedLocalizableString("帖子列表"), url: "ForumPost", icon: "fa fa-paste", requiredPermissionName: LesoftWuye2.Core.Authorization.PermissionNames.Pages_ForumPosts));
            
            


            var managerMenu = new MenuItemDefinition("Manager", new FixedLocalizableString("系统管理"), url: "", icon: "fa fa-wrench", requiresAuthentication: true);
            managerMenu.AddItem(new MenuItemDefinition("Roles", new FixedLocalizableString("角色"), url: "Roles", icon: "fa fa-users", requiredPermissionName: PermissionNames.Pages_Roles));
            managerMenu.AddItem(new MenuItemDefinition("Users", new FixedLocalizableString("系统用户"), url: "Users", icon: "fa fa-user", requiredPermissionName: PermissionNames.Pages_Users));
            managerMenu.AddItem(new MenuItemDefinition("AuditLogs", new FixedLocalizableString("审计日志"), url: "AuditLogs", icon: "fa fa-lock", requiredPermissionName: PermissionNames.Pages_AuditLogs));
            managerMenu.AddItem(new MenuItemDefinition("Maintenance", new FixedLocalizableString("系统维护"), url: "Maintenance", icon: "fa fa-gear", requiredPermissionName: PermissionNames.Pages_Maintenance));
            managerMenu.AddItem(new MenuItemDefinition("Settings", new FixedLocalizableString("参数设置"), url: "Settings", icon: "fa fa-flag", requiredPermissionName: LesoftWuye2.Core.Authorization.PermissionNames.Pages_Settings));
            managerMenu.AddItem(new MenuItemDefinition("ApiLogs", new FixedLocalizableString("API调用日志"), url: "ApiLogs", icon: "fa fa-info-circle", requiredPermissionName: LesoftWuye2.Core.Authorization.PermissionNames.Pages_ApiLogs));

            context.Manager.MainMenu.AddItem(homeMenu)
                .AddItem(baseMenu)
                .AddItem(articlesMenu)
                .AddItem(mallMenu)
                .AddItem(forumMenu)
                .AddItem(managerMenu);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ObsConsts.LocalizationSourceName);
        }
    }
}













