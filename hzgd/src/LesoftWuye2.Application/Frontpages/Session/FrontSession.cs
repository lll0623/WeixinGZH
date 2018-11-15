using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using LesoftWuye2.Application.WuyeApis;
using LesoftWuye2.Core.Projects;

namespace LesoftWuye2.Application.Frontpages.Session
{
    public class FrontSession : IFrontSession, ISingletonDependency
    {
        private readonly IRepository<Project, long> _projectRepository;
        private readonly IWuyeApiAppSrvice _wuyeApiAppSrvice;
        

        public FrontSession(IRepository<Project, long> projectRepository, IWuyeApiAppSrvice wuyeApiAppSrvice)
        {
            _projectRepository = projectRepository;
            _wuyeApiAppSrvice = wuyeApiAppSrvice;
        }

        public string AppMemberId { get; set; }

        public long ProjectId { get; private set; }

        public long MemberId { get; private set; }


        public void Prepare()
        {
            //根据单元编号等数据获取小区id
            ProjectId = 0;

            var userInfo= _wuyeApiAppSrvice.GetUserInfo(AppMemberId);
            if (userInfo.Success)
            {
                MemberId = long.Parse(AppMemberId);
            }

            var result = _wuyeApiAppSrvice.GetMemberInfoByAPPUserID(AppMemberId);
            if (result.Success)
            {
                var project = _projectRepository.FirstOrDefault(t => t.Code == result.PProjectCode);
                if (project != null)
                    ProjectId = project.Id;
            }
        }
    }
}
