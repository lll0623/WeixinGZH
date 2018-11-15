using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.Suppliers;
using LesoftWuye2.Application.Suppliers.Dto;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers 
 {  public class SuppliersController :LesoftWuye2ControllerBase 
{
private readonly ISupplierAppService _supplierAppService;
public SuppliersController(ISupplierAppService supplierAppService){_supplierAppService = supplierAppService;}
public async Task<ActionResult> Index(){var output = await _supplierAppService.GetSuppliers(BuildPageListRequstDto());return View(output);}
}
}
