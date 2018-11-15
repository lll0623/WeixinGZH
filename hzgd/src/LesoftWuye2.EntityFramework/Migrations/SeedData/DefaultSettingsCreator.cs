using System.Linq;
using Abp.Configuration;
using Abp.Localization;
using Abp.Net.Mail;

using LesoftWuye2.EntityFramework.EntityFramework;

namespace LesoftWuye2.EntityFramework.Migrations.SeedData
{
    public class DefaultSettingsCreator
    {
        private readonly LesoftWuye2DbContext _context;

        public DefaultSettingsCreator(LesoftWuye2DbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            //Emailing
            AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, "admin@mydomain.com");
            AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, "mydomain.com mailer");

            //Languages
            AddSettingIfNotExists(LocalizationSettingNames.DefaultLanguage, "en");
        }

        private void AddSettingIfNotExists(string name, string value, int? tenantId = null)
        {
            if (_context.Settings.Any(s => s.Name == name && s.TenantId == tenantId && s.UserId == null))
            {
                return;
            }

            _context.Settings.Add(new Setting(tenantId, null, name, value));
            _context.SaveChanges();
        }
    }
}