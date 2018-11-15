using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using LesoftWuye2.Application.AppApis.Dto;

namespace LesoftWuye2.Application.AppApis
{
    public interface IAppapiService : IApplicationService
    {
        Task<List<slideimage>> GetSlideImage();
    
        void SetCurrentRoomInfo(SetCurrentRoomInfo info);

        CurrentRoomInfo GetCurrentRoomInfo(string Upk);

        Task<List<HomeNews>> GetHomeNews(string Upk);

        List<NavigationItem> GetNavigationItems();

        
    }
}
