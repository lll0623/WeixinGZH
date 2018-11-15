using Abp.Web.Mvc.Views;
using Obs;

namespace LesoftWuye2.Web.Views
{
    public abstract class LesoftWuye2WebViewPageBase : LesoftWuye2WebViewPageBase<dynamic>
    {

    }

    public abstract class LesoftWuye2WebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected LesoftWuye2WebViewPageBase()
        {
            LocalizationSourceName = ObsConsts.LocalizationSourceName;
        }
    }
}












