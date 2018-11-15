using Abp.Application.Navigation;

namespace LesoftWuye2.Web.Models.Layout
{
    public class TopMenuViewModel
    {
        public UserMenu MainMenu { get; set; }

        public string ActiveMenuItemName { get; set; }

        public string ActiveMenuItemParentName { get; set; }
    }
}












