using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.Estateinfos;
using LesoftWuye2.Application.Estateinfos.Dto;
using LesoftWuye2.Application.EstateinfoTypes;
using LesoftWuye2.Application.ForumPosts;
using LesoftWuye2.Application.RefundOrders;
using LesoftWuye2.Application.Projects;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers
{
    public class RefundOrderController : LesoftWuye2ControllerBase
    {
        private readonly IRefundOrderService _RefundOrderService;
        


        public RefundOrderController(IRefundOrderService RefundOrderService)
        {
            _RefundOrderService = RefundOrderService;
        }

        public async Task<ActionResult> Index()
        {
            
            var output = await _RefundOrderService.GetRefundOrders(BuildPageListRequstDto());
            return View(output);
        }

        public async Task<ActionResult> View(long id)
        {
            var output = await _RefundOrderService.GetRefundOrder(id);
            return View(output);
        }
    }
}
