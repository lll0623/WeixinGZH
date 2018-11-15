using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Castle.Components.DictionaryAdapter;
using LesoftWuye2.Application.Malls.Dto;
using Obs.Dto;
using Obs.Filter;
using LesoftWuye2.Application.Products.Dto;
using LesoftWuye2.Core.Configuration;
using LesoftWuye2.Core.Products;
using LesoftWuye2.Core.Wuyebase.Members;

namespace LesoftWuye2.Application.Products
{
    public class ProductAppService : LesoftWuye2AppServiceBase, IProductAppService
    {
        private readonly IRepository<Product, long> _productRepository;
        private readonly IRepository<ProductComment, long> _productCommentRepository;
        private readonly IRepository<Member, long> _memberRepository;

        public ProductAppService(IRepository<Product, long> productRepository,
             IRepository<ProductComment, long> productCommentRepository,
             IRepository<Member, long> memberRepository)
        {
            _productRepository = productRepository;
            _productCommentRepository = productCommentRepository;
            _memberRepository = memberRepository;
        }

        public async Task CreateProduct(CreateProductInput input)
        {
            var product = input.MapTo<Product>();
            product.ServiceTags = string.Join(",", input.Tags);
            await _productRepository.InsertAsync(product);
        }

        public async Task DeleteProduct(long id)
        {
            await _productRepository.DeleteAsync(id);
        }

        public async Task UpdateProduct(UpdateProductInput input)
        {
            var product = await _productRepository.GetAsync(input.Id);
            input.MapTo(product);
            product.ServiceTags = string.Join(",", input.Tags);
            await _productRepository.UpdateAsync(product);
        }
        public async Task<PageListResultDto<ProductListDto>> GetProducts(
            GetPageListRequstDto dto)
        {
            var query = _productRepository.GetAll();
            var where = FilterExpression.FindByGroup<Product>(dto.Filter);
            var count = await query.Where(where).CountAsync();
            var list = await query.Where(where).OrderByDescending(t => t.CreationTime).PageBy(dto).ToListAsync();
            var pageList = list.MapTo<List<ProductListDto>>();
            return new PageListResultDto<ProductListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }

        public async Task<ProductItemDto> GetProduct(long id)
        {
            var product = await _productRepository.GetAsync(id);

            var dto = product.MapTo<ProductItemDto>();
            dto.Tags = new List<string>();
            dto.Tags.AddRange(product.ServiceTags.Split(','));
            return dto;
        }
        public async Task<List<ComboboxItemDto>> GetServiceTagForCombo()
        {
            var serviceTag = await SettingManager.GetSettingValueAsync(AppSettings.Mall.ServiceTag);
            string[] tagItems = serviceTag.Split(',');
            List<ComboboxItemDto> comboboxItemDtos = new EditableList<ComboboxItemDto>();
            comboboxItemDtos.AddRange(tagItems.Select(tag => new ComboboxItemDto(tag, tag)));
            return comboboxItemDtos;
        }

        public async Task<List<ComboboxItemDto>> GetProductsForCombo()
        {
            var items = await _productRepository.GetAll().OrderByDescending(t => t.CreationTime).ToListAsync();
            var list = items.Select(result =>
            {
                var item = new ComboboxItemDto
                {
                    DisplayText = result.Name,
                    Value = result.Id.ToString()
                };
                return item;
            }).ToList();

            return list;
        }

        public async Task<PageListResultDto<ProductCommentListDto>> GetProductComments(GetPageListRequstDto dto)
        {
            var query = _productCommentRepository.GetAll();
            var where = FilterExpression.FindByGroup<ProductComment>(dto.Filter);


            var queryCount = query.Where(where)
                .GroupJoin(_memberRepository.GetAll(), left => left.MemberId, right => right.Id, (left, right) => new
                {
                    ProductComment = left,
                    Member = right
                })
                .GroupJoin(_productRepository.GetAll(), left => left.ProductComment.ProductId, right => right.Id, (left, right) => new
                {
                    ProductComment = left.ProductComment,
                    Member = left.Member,
                    Product = right
                });

          
            int count = await queryCount.CountAsync();




            var queryList = query.Where(where)
             .GroupJoin(_memberRepository.GetAll(), left => left.MemberId, right => right.Id, (left, right) => new
             {
                 ProductComment = left,
                 Member = right
             })
             .GroupJoin(_productRepository.GetAll(), left => left.ProductComment.ProductId, right => right.Id, (left, right) => new
             {
                 ProductComment = left.ProductComment,
                 Member = left.Member,
                 Product = right
             });


          


            var comments = await queryList.OrderByDescending(t => t.ProductComment.CreationTime).PageBy(dto).ToListAsync();


            List<ProductCommentListDto> Comments = comments.Select(comment => new ProductCommentListDto()
            {
                Id = comment.ProductComment.Id,
                ProductName = comment.Product.FirstOrDefault()?.Name,
                MemberName = comment.Member.FirstOrDefault()?.Name,
                Content = comment.ProductComment.Content,
                CreationTime = comment.ProductComment.CreationTime
            }).ToList();

            return new PageListResultDto<ProductCommentListDto>(count, Comments, dto.CurrentPage, dto.PageSize);
        }

        public async Task DeleteProductComment(long id)
        {
            await _productCommentRepository.DeleteAsync(id);
        }
    }
}
