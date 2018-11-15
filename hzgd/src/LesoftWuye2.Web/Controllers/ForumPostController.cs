using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.Estateinfos;
using LesoftWuye2.Application.Estateinfos.Dto;
using LesoftWuye2.Application.EstateinfoTypes;
using LesoftWuye2.Application.ForumPosts;
using LesoftWuye2.Application.Projects;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers
{
    public class ForumPostController : LesoftWuye2ControllerBase
    {
        private readonly IForumPostService _forumPostService;
        


        public ForumPostController(IForumPostService forumPostService)
        {
            _forumPostService = forumPostService;
        }

        public async Task<ActionResult> Index()
        {
            
            var output = await _forumPostService.GetForumPosts(BuildPageListRequstDto());
            return View(output);
        }

        public async Task<ActionResult> View(long id)
        {
            var output = await _forumPostService.GetForumPost(id);
            return View(output);
        }
    }
}
