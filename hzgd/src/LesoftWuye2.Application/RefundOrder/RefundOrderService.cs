using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Events.Bus;
using Abp.Linq.Extensions;
using Abp.UI;
using LesoftWuye2.Application.Malls.Dto;
using LesoftWuye2.Application.RefundOrders.Dto;
using LesoftWuye2.Application.Weixin.Events.EventDatas;
using LesoftWuye2.Application.WuyeApis;
using LesoftWuye2.Core.Areas;
using LesoftWuye2.Core.Categories;
using LesoftWuye2.Core.Groupons;
using LesoftWuye2.Core.Mall.RefundOrders;
using LesoftWuye2.Core.MallSlideImages;
using LesoftWuye2.Core.MemberAddresss;
using LesoftWuye2.Core.RefundOrders;
using LesoftWuye2.Core.Products;
using LesoftWuye2.Core.SqlExecuters;
using LesoftWuye2.Core.Wuyebase.Members;
using Obs;
using Obs.Dto;
using Obs.Filter;
using LesoftWuye2.Application.Orders.Dto;
using LesoftWuye2.Core.Mall.Orders;

namespace LesoftWuye2.Application.RefundOrders
{
    public class RefundOrderService : LesoftWuye2AppServiceBase, IRefundOrderService
    {
        private readonly IRepository<Groupon, long> _grouponRepository;
        private readonly IRepository<Product, long> _productRepository;
        private readonly IRepository<Category, long> _categoryRepository;
        private readonly IRepository<ProductSlideImage, long> _productSlideimageRepository;
        private readonly IRepository<MallSlideImage, long> _mallslideimageRepository;
        private readonly IRepository<MemberAddress, long> _memberAddressRepository;
        private readonly IRepository<Area, long> _areaRepository;
        private readonly IWuyeApiAppSrvice _wuyeApiAppSrvice;
        private readonly IRepository<Member, long> _memberRepository;
        private readonly IRepository<RefundOrder, long> _refundOrderRepository;
        private readonly IRepository<RefundOrderImage, long> _refundOrderImageRepository;
        private readonly IRepository<Order, long> _orderRepository;

        private readonly ISqlExecuter _sqlExecuter;
        private readonly IAppFolders _appFolders;

        public RefundOrderService(IRepository<Groupon, long> grouponRepository,
            IRepository<Product, long> productRepository,
            IRepository<Category, long> categoryRepository,
            IRepository<ProductSlideImage, long> productSlideimageRepository,
            IRepository<MallSlideImage, long> mallslideimageRepository,
            IRepository<MemberAddress, long> memberAddressRepository,
            IRepository<Area, long> areaRepository,
            IWuyeApiAppSrvice wuyeApiAppSrvice,
            IRepository<Member, long> memberRepository,
             IRepository<RefundOrder, long> refundOrderRepository,
             IRepository<RefundOrderImage, long> refundOrderImageRepository,
             IRepository<Order, long> orderRepository,
        ISqlExecuter sqlExecuter,
              IAppFolders appFolders
             )
        {
            _grouponRepository = grouponRepository;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _productSlideimageRepository = productSlideimageRepository;
            _mallslideimageRepository = mallslideimageRepository;
            _memberAddressRepository = memberAddressRepository;
            _areaRepository = areaRepository;
            _wuyeApiAppSrvice = wuyeApiAppSrvice;
            _memberRepository = memberRepository;

            _refundOrderRepository = refundOrderRepository;
            _refundOrderImageRepository = refundOrderImageRepository;
            _orderRepository = orderRepository;
            _sqlExecuter = sqlExecuter;

            _appFolders = appFolders;
        }


        public async Task<PageListResultDto<RefundOrderListDto>> GetRefundOrders(GetPageListRequstDto dto)
        {
            var query = _refundOrderRepository.GetAll();
            var where = FilterExpression.FindByGroup<RefundOrder>(dto.Filter);

            var queryCount = query.Where(where);

            int count = await queryCount.CountAsync();

            var queryList = query.Where(where);

            var tts = await queryList.OrderByDescending(t => t.CreationTime).PageBy(dto).ToListAsync();
            var pageList = tts.MapTo<List<RefundOrderListDto>>();
         

            return new PageListResultDto<RefundOrderListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }

        public async Task<RefundOrderItemDto> GetRefundOrder(long id)
        {
            var RefundOrderInfo = await _refundOrderRepository.FirstOrDefaultAsync(t => t.Id == id);
            if (RefundOrderInfo == null)
                throw new UserFriendlyException("订单不存在!");

            var images = await _refundOrderImageRepository.GetAll().Where(t => t.RefundOrderId == id).ToListAsync();
            var data = RefundOrderInfo.MapTo<RefundOrderItemDto>();
            data.Images = images.Select(t => t.Thumbnail).ToList();
            return data;
        }

        public async Task Reject(RefundOrderRejectDto dto)
        {
            var refundOrderInfo = await _refundOrderRepository.FirstOrDefaultAsync(t => t.Id == dto.OrderId);
            if (refundOrderInfo == null)
                throw new UserFriendlyException("退款订单不存在!");

            if(refundOrderInfo.Status!=RefundStatus.Processing)
                throw new UserFriendlyException("当前退款订单不允许该操作!");

            var orderInfo = await _orderRepository.FirstOrDefaultAsync(t => t.Id == refundOrderInfo.OrderId);
            if (orderInfo == null)
                throw new UserFriendlyException("退款订单不存在!");
            if (orderInfo.RefundStatus != RefundStatus.Processing)
                throw new UserFriendlyException("当前退款订单不允许该操作!");


            refundOrderInfo.Status = RefundStatus.Reject;
            orderInfo.RefundStatus = RefundStatus.Reject;

           await _refundOrderRepository.UpdateAsync(refundOrderInfo);
            await _orderRepository.UpdateAsync(orderInfo);

            //发送模版消息
            UnitOfWorkManager.Current.Completed += (sender, args) =>
            {
                new Thread(() => EventBus.Default.Trigger(new RefundOrderRejectEventData() {MemberId=refundOrderInfo.MemberId,Amount=refundOrderInfo.RefundMoney.ToString("F2"),OrderId = dto.OrderId,ProductName=refundOrderInfo.Name,OrderNo=refundOrderInfo.OrderNo })).Start();
            };
        }

        public async Task Accept(RefundOrderAcceptDto dto)
        {
            var refundOrderInfo = await _refundOrderRepository.FirstOrDefaultAsync(t => t.Id == dto.OrderId);
            if (refundOrderInfo == null)
                throw new UserFriendlyException("退款订单不存在!");

            if (refundOrderInfo.Status != RefundStatus.Processing)
                throw new UserFriendlyException("当前退款订单不允许该操作!");

            var orderInfo = await _orderRepository.FirstOrDefaultAsync(t => t.Id == refundOrderInfo.OrderId);
            if (orderInfo == null)
                throw new UserFriendlyException("退款订单不存在!");
            if (orderInfo.RefundStatus != RefundStatus.Processing)
                throw new UserFriendlyException("当前退款订单不允许该操作!");


            refundOrderInfo.Status = RefundStatus.Accept;
            orderInfo.RefundStatus = RefundStatus.Accept;
            orderInfo.Status = OrderStatus.Cancel;

            await _refundOrderRepository.UpdateAsync(refundOrderInfo);
            await _orderRepository.UpdateAsync(orderInfo);

            //发送模版消息

            //发送模版消息
            UnitOfWorkManager.Current.Completed += (sender, args) =>
            {
                new Thread(() => EventBus.Default.Trigger(new RefundOrderAcceptEventData() { MemberId = refundOrderInfo.MemberId, Amount = refundOrderInfo.RefundMoney.ToString("F2"), OrderId = dto.OrderId, ProductName = refundOrderInfo.Name, OrderNo = refundOrderInfo.OrderNo })).Start();
            };
        }
    }
}
