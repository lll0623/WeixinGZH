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
using LesoftWuye2.Application.Estateinfos.Dto;
using LesoftWuye2.Core.Details;

using LesoftWuye2.Core.Estateinfos;
using LesoftWuye2.Core.NoticeProjects;
using LesoftWuye2.Core.Projects;

namespace LesoftWuye2.Application.Estateinfos
{
    public class EstateinfoAppService : LesoftWuye2AppServiceBase, IEstateinfoAppService
    {
        private readonly IRepository<Estateinfo, long> _estateinfoRepository;
        private readonly IRepository<EstateinfoImage, long> _estateinfoImageRepository;
        private readonly IRepository<EstateinfoComment, long> _estateinfoCommentRepository;
        private readonly IDetailManager _detailManager;

        public EstateinfoAppService(IRepository<Estateinfo, long> estateinfoRepository,
            IRepository<EstateinfoImage, long> estateinfoImageRepository,
            IRepository<EstateinfoComment, long> estateinfoCommentRepository,
            IDetailManager detailManager)
        {
            _estateinfoRepository = estateinfoRepository;
            _estateinfoImageRepository = estateinfoImageRepository;
            _estateinfoCommentRepository = estateinfoCommentRepository;
            _detailManager = detailManager;
        }

        public async Task CreateEstateinfo(CreateEstateinfoInput input)
        {

            var estateinfo = input.MapTo<Estateinfo>();
            var dataid = await _estateinfoRepository.InsertAndGetIdAsync(estateinfo);
            _detailManager.Save(DetailType.Estateinfo, dataid, input.Content);
        }

        public async Task DeleteEstateinfo(long id)
        {
            await _estateinfoCommentRepository.DeleteAsync(t => t.EstateinfoId == id);
            await _estateinfoImageRepository.DeleteAsync(t => t.EstateinfoId == id);
            await _estateinfoRepository.DeleteAsync(id);
            _detailManager.Delete(DetailType.Estateinfo, id);
        }



        public async Task<PageListResultDto<EstateinfoListDto>> GetEstateinfos(GetPageListRequstDto dto)
        {
            var query = _estateinfoRepository.GetAll();
            var where = FilterExpression.FindByGroup<Estateinfo>(dto.Filter);
            var count = await query.Where(where).CountAsync();
            var list = await query.Where(where).OrderByDescending(t => t.CreationTime).PageBy(dto).ToListAsync();
            var pageList = list.MapTo<List<EstateinfoListDto>>();

            return new PageListResultDto<EstateinfoListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }

        public async Task<EstateinfoItemDto> GetEstateinfo(long id)
        {
            var estateinfo = await _estateinfoRepository.GetAsync(id);
            var dto = estateinfo.MapTo<EstateinfoItemDto>();
            dto.Content = _detailManager.Get(DetailType.Estateinfo, estateinfo.Id);
            var images = await _estateinfoImageRepository.GetAll().Where(t => t.EstateinfoId == id).ToListAsync();
            dto.Images = images.Select(t => t.Image).ToList();
            return dto;
        }

        public async Task ShowEstateinfo(long id)
        {
            var estateinfo = await _estateinfoRepository.GetAsync(id);
            estateinfo.IsShow = true;
            await _estateinfoRepository.UpdateAsync(estateinfo);
        }

        public async Task HideEstateinfo(long id)
        {
            var estateinfo = await _estateinfoRepository.GetAsync(id);
            estateinfo.IsShow = false;
            await _estateinfoRepository.UpdateAsync(estateinfo);
        }
    }
}
