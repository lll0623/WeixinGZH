using System.Threading.Tasks;
using Abp.Application.Services;
using LesoftWuye2.Application.Configuration.Dto;

namespace LesoftWuye2.Application.Configuration
{
   public  interface ISettingsAppService : IApplicationService
    {
        Task<SettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(SettingsEditDto dto);

        void CreateWeixinMenu();
    }
}
