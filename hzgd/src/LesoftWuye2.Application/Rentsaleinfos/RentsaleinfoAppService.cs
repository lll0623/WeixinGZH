using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Obs.Dto;
using Obs.Filter;
using LesoftWuye2.Application.Rentsaleinfos.Dto;
using LesoftWuye2.Core.Details;

using LesoftWuye2.Core.Rentsaleinfos;
using LesoftWuye2.Core.NoticeProjects;
using LesoftWuye2.Core.Projects;

namespace LesoftWuye2.Application.Rentsaleinfos
{
    public class RentsaleinfoAppService : LesoftWuye2AppServiceBase, IRentsaleinfoAppService
    {
        private readonly IRepository<Rentsaleinfo, long> _rentsaleinfoRepository;
        private readonly IRepository<RentsaleinfoImage, long> _rentsaleinfoImageRepository;

        private readonly IDetailManager _detailManager;

        public RentsaleinfoAppService(IRepository<Rentsaleinfo, long> rentsaleinfoRepository,
            IRepository<RentsaleinfoImage, long> rentsaleinfoImageRepository,
          
            IDetailManager detailManager)
        {
            _rentsaleinfoRepository = rentsaleinfoRepository;
            _rentsaleinfoImageRepository = rentsaleinfoImageRepository;
         
            _detailManager = detailManager;
        }

        public async Task CreateRentsaleinfo(CreateRentsaleinfoInput input)
        {

            var rentsaleinfo = input.MapTo<Rentsaleinfo>();
            rentsaleinfo.Source = RentsaleinfoSource.Admin;
            rentsaleinfo.IsSale = true;
            rentsaleinfo.IsShow = true;

            var dataid = await _rentsaleinfoRepository.InsertAndGetIdAsync(rentsaleinfo);
            _detailManager.Save(DetailType.Rentsaleinfo, dataid, input.Content);
        }

        public async Task DeleteRentsaleinfo(long id)
        {
            await _rentsaleinfoImageRepository.DeleteAsync(t => t.RentsaleinfoId == id);
            await _rentsaleinfoRepository.DeleteAsync(id);
            _detailManager.Delete(DetailType.Rentsaleinfo, id);
        }



        public async Task<PageListResultDto<RentsaleinfoListDto>> GetRentsaleinfos(GetPageListRequstDto dto)
        {
            var query = _rentsaleinfoRepository.GetAll();
            var where = FilterExpression.FindByGroup<Rentsaleinfo>(dto.Filter);
            var count = await query.Where(where).CountAsync();
            var list = await query.Where(where).OrderByDescending(t => t.CreationTime).PageBy(dto).ToListAsync();
            var pageList = list.MapTo<List<RentsaleinfoListDto>>();

            return new PageListResultDto<RentsaleinfoListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }

        public async Task<RentsaleinfoItemDto> GetRentsaleinfo(long id)
        {
            var rentsaleinfo = await _rentsaleinfoRepository.GetAsync(id);
            var dto = rentsaleinfo.MapTo<RentsaleinfoItemDto>();
            dto.Content = _detailManager.Get(DetailType.Rentsaleinfo, rentsaleinfo.Id);

            var images = await _rentsaleinfoImageRepository.GetAll().Where(t => t.RentsaleinfoId == id).ToListAsync();
            dto.Images = images.Select(t => t.Image).ToList();

            return dto;
        }

        public async Task ShowRentsaleinfo(long id)
        {
            var rentsaleinfo = await _rentsaleinfoRepository.GetAsync(id);
            rentsaleinfo.IsShow = true;
            await _rentsaleinfoRepository.UpdateAsync(rentsaleinfo);
        }

        public async Task HideRentsaleinfo(long id)
        {
            var rentsaleinfo = await _rentsaleinfoRepository.GetAsync(id);
            rentsaleinfo.IsShow = false;
            await _rentsaleinfoRepository.UpdateAsync(rentsaleinfo);
        }

        public async Task UpdateRentsaleinfo(UpdateRentsaleinfoInput input)
        {
            var oldData = await _rentsaleinfoRepository.FirstOrDefaultAsync(t => t.Id == input.Id);
            if(oldData==null)
                throw new UserFriendlyException("修改的数据不存在!");
            input.MapTo(oldData);
            await _rentsaleinfoRepository.UpdateAsync(oldData);


            _detailManager.Save(DetailType.Rentsaleinfo, oldData.Id, input.Content);
        }
    }
}
