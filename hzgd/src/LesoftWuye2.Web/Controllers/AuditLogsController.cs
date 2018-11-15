using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using Obs.Auditing;
using Obs.Users;

using PermissionNames = Obs.Authorization.PermissionNames;

namespace LesoftWuye2.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_AuditLogs)]
    public class AuditLogsController : LesoftWuye2ControllerBase
    {
        private readonly IAuditLogAppService _auditLogAppService;

        public AuditLogsController(IAuditLogAppService auditLogAppService)
        {
            _auditLogAppService = auditLogAppService;
        }

        public async Task<ActionResult> Index()
        {
            var output = await _auditLogAppService.GetAuditLogs(BuildPageListRequstDto());
            return View(output);
        }
    }
}












