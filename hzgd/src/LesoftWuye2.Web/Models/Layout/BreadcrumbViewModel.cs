using System.Collections.Generic;
using Abp.Application.Navigation;

namespace LesoftWuye2.Web.Models.Layout
{
    public class BreadcrumbViewModel
    {
        public BreadcrumbViewModel()
        {
            Items=new List<BreadcrumbViewModelItem>();
        }

        public List<BreadcrumbViewModelItem> Items { get;private set; }

    }

    public class BreadcrumbViewModelItem
    {
        public string Icon { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }

        public bool IsActive { get; set; }
    }
}












