using System.Web.Optimization;

namespace LesoftWuye2.Web
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            //VENDOR RESOURCES

            //~/Bundles/vendor/css
            bundles.Add(
                new StyleBundle("~/Bundles/vendor/css")
                    //.Include("~/Content/toastr.min.css", new CssRewriteUrlTransform())
                   .Include("~/Scripts/sweetalert/sweet-alert.css", new CssRewriteUrlTransform())

                   .Include("~/Content/css/font-awesome.min.css", new CssRewriteUrlTransform())
                   .Include("~/Content/css/weather-icons.min.css", new CssRewriteUrlTransform())
                   .Include("~/Content/css/typicons.min.css", new CssRewriteUrlTransform())
                   .Include("~/Content/css/bootstrap.min.css", new CssRewriteUrlTransform())
                   .Include("~/Content/css/dataTables.bootstrap.css", new CssRewriteUrlTransform())
                   .Include("~/Content/js/jstree/themes/default/style.min.css", new CssRewriteUrlTransform())

                   .Include("~/Content/css/animate.min.css", new CssRewriteUrlTransform())
                   .Include("~/Content/css/beyond.min.css", new CssRewriteUrlTransform())
                   .Include("~/Content/css/demo.min.css", new CssRewriteUrlTransform())
                   .Include("~/Content/css/app.css", new CssRewriteUrlTransform())
                );

            //~/Bundles/vendor/js/top (These scripts should be included in the head of the page)
            bundles.Add(
                new ScriptBundle("~/Bundles/vendor/js/top")
                    .Include(
                        "~/Content/js/skins.min.js"
                    )
                );

            //~/Bundles/vendor/bottom (Included in the bottom for fast page load)
            bundles.Add(
                new ScriptBundle("~/Bundles/vendor/js/bottom")
                    .Include(
                        "~/Scripts/json2.min.js",

                     "~/Scripts/jquery-2.2.0.min.js",

                     //"~/Scripts/bootstrap.min.js",

                     "~/Scripts/moment-with-locales.min.js",
                     //"~/Scripts/jquery.validate.min.js",
                     "~/Scripts/jquery.blockUI.js",
                     "~/Content/js/toastr/toastr.js",
                     "~/Scripts/sweetalert/sweet-alert.min.js",
                     "~/Scripts/others/spinjs/spin.js",
                     "~/Scripts/others/spinjs/jquery.spin.js",

                     "~/Abp/Framework/scripts/abp.js",
                     "~/Abp/Framework/scripts/libs/abp.jquery.js",
                     "~/Abp/Framework/scripts/libs/abp.toastr.js",
                     "~/Abp/Framework/scripts/libs/abp.blockUI.js",
                     "~/Abp/Framework/scripts/libs/abp.spin.js",
                     "~/Abp/Framework/scripts/libs/abp.sweet-alert.js",

                     "~/Content/js/datetime/bootstrap-datepicker.js",
                     "~/Content/js/jstree/jstree.min.js",

                     "~/Content/js/select2/select2.js",
                     "~/Content/js/validation/bootstrapValidator.js",
                     "~/Content/js/bootstrap.min.js",
                     "~/Content/js/skins.min.js",
                     "~/Content/js/beyond.min.js",
                     "~/Content/js/app.js"

                    )
                );

            #region 登录

            //登录页面css
            bundles.Add(
               new StyleBundle("~/Bundles/login/css")
                   .Include("~/Content/toastr.min.css", new CssRewriteUrlTransform())
                   .Include("~/Scripts/sweetalert/sweet-alert.css", new CssRewriteUrlTransform())

                   .Include("~/Content/css/font-awesome.min.css", new CssRewriteUrlTransform())
                   .Include("~/Content/css/bootstrap.min.css", new CssRewriteUrlTransform())
                   .Include("~/Content/css/beyond.min.css", new CssRewriteUrlTransform())
                   .Include("~/Content/css/demo.min.css", new CssRewriteUrlTransform())
                   .Include("~/Content/css/animate.min.css", new CssRewriteUrlTransform())

               );

            //登录页面js
            bundles.Add(
             new ScriptBundle("~/Bundles/vendor/js/login")
                 .Include(
                     "~/Scripts/json2.min.js",

                     "~/Scripts/jquery-2.2.0.min.js",

                     //"~/Scripts/bootstrap.min.js",

                     "~/Scripts/moment-with-locales.min.js",
                     "~/Scripts/jquery.validate.min.js",
                     "~/Scripts/jquery.blockUI.js",
                     "~/Scripts/toastr.min.js",
                     "~/Scripts/sweetalert/sweet-alert.min.js",
                     "~/Scripts/others/spinjs/spin.js",
                     "~/Scripts/others/spinjs/jquery.spin.js",

                     "~/Abp/Framework/scripts/abp.js",
                     "~/Abp/Framework/scripts/libs/abp.jquery.js",
                     "~/Abp/Framework/scripts/libs/abp.toastr.js",
                     "~/Abp/Framework/scripts/libs/abp.blockUI.js",
                     "~/Abp/Framework/scripts/libs/abp.spin.js",
                     "~/Abp/Framework/scripts/libs/abp.sweet-alert.js",

                     "~/Content/js/bootstrap.min.js",
                     "~/Content/js/skins.min.js",
                     "~/Content/js/beyond.min.js",
                     "~/Views/Account/Login.js"
                 )
             ); 

            #endregion

            //APPLICATION RESOURCES

            //~/Bundles/css
            bundles.Add(
                new StyleBundle("~/Bundles/css")
                    .Include("~/css/main.css")
                );

            //~/Bundles/js
            bundles.Add(
                new ScriptBundle("~/Bundles/js")
                    .Include("~/js/main.js")
                );
        }
    }
}












