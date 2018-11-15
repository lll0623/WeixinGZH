using Obs.Sessions.Dto;

namespace LesoftWuye2.Web.Models.Layout
{
    public class UserMenuModel
    {
        public GetCurrentLoginInformationsOutput LoginInformations { get; set; }
        public bool IsMultiTenancyEnabled { get; set; }

        public string GetShownLoginName()
        {
            var userName = "" + LoginInformations.User.Surname + "";

            if (!IsMultiTenancyEnabled)
            {
                return userName;
            }

            return userName;
        }

        public string GetName()
        {
            return LoginInformations.User.UserName;
        }

        public string GetEmail()
        {
            return LoginInformations.User.EmailAddress;
        }
    }
}












