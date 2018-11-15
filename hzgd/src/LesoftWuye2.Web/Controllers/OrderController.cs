using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.Estateinfos;
using LesoftWuye2.Application.Estateinfos.Dto;
using LesoftWuye2.Application.EstateinfoTypes;
using LesoftWuye2.Application.ForumPosts;
using LesoftWuye2.Application.Orders;
using LesoftWuye2.Application.Projects;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers
{
    public class OrderController : LesoftWuye2ControllerBase
    {
        private readonly IOrderService _orderService;
        


        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<ActionResult> Index()
        {
            
            var output = await _orderService.GetOrders(BuildPageListRequstDto());
            return View(output);
        }

        public async Task<ActionResult> View(long id)
        {
            var output = await _orderService.GetOrder(id);
            return View(output);
        }
    }
}
