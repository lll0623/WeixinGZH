using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using LesoftWuye2.Application.WuyeApis;
using LesoftWuye2.Core.Projects;
using LesoftWuye2.Core.Wuyebase.Members;

namespace LesoftWuye2.Application.Weixin.Session
{
    public class WeixinSession : IWeixinSession, ISingletonDependency
    {
        private readonly IRepository<Project, long> _projectRepository;
        private readonly IRepository<Member, long> _memberRepository;
        private readonly IWuyeApiAppSrvice _wuyeApiAppSrvice;


        public WeixinSession(IRepository<Project, long> projectRepository,
            IRepository<Member, long> memberRepository,
            IWuyeApiAppSrvice wuyeApiAppSrvice)
        {
            _projectRepository = projectRepository;
            _memberRepository = memberRepository;
            _wuyeApiAppSrvice = wuyeApiAppSrvice;
        }

        public string OpenId { get; set; }

        public long ProjectId { get; private set; }

        public long MemberId { get; private set; }
        public bool IsBindRoom { get; private set; }
        public string MemberName { get; private set; }

        public void Reset()
        {
            ProjectId = 0;
            MemberId = 0;
            IsBindRoom = false;
            MemberName = ""; 
        }

        public void Prepare()
        {
            if (!string.IsNullOrEmpty(OpenId))
            {
                //获取用户相关信息
                var memberInfo = _memberRepository.FirstOrDefault(t => t.Openid == OpenId);
                if (memberInfo != null)
                {
                    MemberId = memberInfo.Id;
                    MemberName = memberInfo.Name;
                    IsBindRoom = !string.IsNullOrEmpty(memberInfo.PRoomFullName);
                    if (!string.IsNullOrEmpty(memberInfo.ProjectCode))
                    {
                        var project = _projectRepository.FirstOrDefault(t => t.Code == memberInfo.ProjectCode);
                        if (project != null)
                            ProjectId = project.Id;
                    }

                    //
                }
                else
                {
                    Reset();
                }
            }
            else
            {
                Reset();
            }
        }
    }
}
