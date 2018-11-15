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
using LesoftWuye2.Application.Orders.Dto;
using LesoftWuye2.Application.Weixin.Events.EventDatas;
using LesoftWuye2.Application.WuyeApis;
using LesoftWuye2.Core.Areas;
using LesoftWuye2.Core.Categories;
using LesoftWuye2.Core.Groupons;
using LesoftWuye2.Core.Mall.Orders;
using LesoftWuye2.Core.MallSlideImages;
using LesoftWuye2.Core.MemberAddresss;
using LesoftWuye2.Core.Orders;
using LesoftWuye2.Core.Products;
using LesoftWuye2.Core.SqlExecuters;
using LesoftWuye2.Core.Wuyebase.Members;
using Obs;
using Obs.Dto;
using Obs.Filter;
using LesoftWuye2.Core.Mall.RefundOrders;

namespace LesoftWuye2.Application.Orders
{
    public class OrderService : LesoftWuye2AppServiceBase, IOrderService
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
        private readonly IRepository<Order, long> _orderRepository;
        private readonly IRepository<OrderPay, long> _orderPayRepository;
        private readonly IRepository<OrderProduct, long> _orderProductRepository;
        private readonly IRepository<GrouponOrder, long> _groupOrderRepository;
        private readonly IRepository<GrouponMember, long> _groupMemberRepository;
        private readonly IRepository<OrderShip, long> _orderShipRepository;
        private readonly IRepository<RefundOrder, long> _refundOrderRepository;

        private readonly ISqlExecuter _sqlExecuter;
        private readonly IAppFolders _appFolders;

        public OrderService(IRepository<Groupon, long> grouponRepository,
            IRepository<Product, long> productRepository,
            IRepository<Category, long> categoryRepository,
            IRepository<ProductSlideImage, long> productSlideimageRepository,
            IRepository<MallSlideImage, long> mallslideimageRepository,
            IRepository<MemberAddress, long> memberAddressRepository,
            IRepository<Area, long> areaRepository,
            IWuyeApiAppSrvice wuyeApiAppSrvice,
            IRepository<Member, long> memberRepository,
             IRepository<Order, long> orderRepository,
             IRepository<OrderPay, long> orderPayRepository,
             IRepository<OrderProduct, long> orderProductRepository,
             IRepository<GrouponOrder, long> groupOrderRepository,
             IRepository<GrouponMember, long> groupMemberRepository,
             IRepository<OrderShip, long> orderShipRepository,
             IRepository<RefundOrder, long> refundOrderRepository,
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

            _orderRepository = orderRepository;
            _orderPayRepository = orderPayRepository;
            _orderProductRepository = orderProductRepository;
            _groupOrderRepository = groupOrderRepository;
            _groupMemberRepository = groupMemberRepository;
            _refundOrderRepository = refundOrderRepository;

            _orderShipRepository = orderShipRepository;
            _sqlExecuter = sqlExecuter;

            _appFolders = appFolders;
        }


        public async Task<PageListResultDto<OrderListDto>> GetOrders(GetPageListRequstDto dto)
        {
            var query = _orderRepository.GetAll();
            var where = FilterExpression.FindByGroup<Order>(dto.Filter);

            var queryCount = query.Where(where)
                 .GroupJoin(_orderProductRepository.GetAll(), left => left.Id, right => right.OrderId,
                   (left, right) => new
                   {
                       Order = left,
                       OrderProduct = right
                   })
                   .GroupJoin(_memberRepository.GetAll(), left => left.Order.MemberId, right => right.Id,
                   (left, right) => new
                   {
                       Order = left.Order,
                       OrderProduct = left.OrderProduct,
                       Member = right
                   });

            int count = await queryCount.CountAsync();

            var queryList = query.Where(where)
               .GroupJoin(_orderProductRepository.GetAll(), left => left.Id, right => right.OrderId,
                   (left, right) => new
                   {
                       Order = left,
                       OrderProduct = right
                   }).GroupJoin(_memberRepository.GetAll(), left => left.Order.MemberId, right => right.Id,
                   (left, right) => new
                   {
                       Order = left.Order,
                       OrderProduct = left.OrderProduct,
                       Member = right
                   });

            var tts = await queryList.OrderByDescending(t => t.Order.CreationTime).PageBy(dto).ToListAsync();
            var pageList = new List<OrderListDto>();
            foreach (var tt in tts)
            {
                pageList.Add(new OrderListDto()
                {
                    Id = tt.Order.Id,
                    Amount = tt.Order.Amount,
                    GrouponStatus = tt.Order.GrouponStatus,
                    OrderId = tt.Order.Id,
                    ProductName = tt.OrderProduct.FirstOrDefault()?.Name,
                    Status = tt.Order.Status,
                    Type = tt.Order.Type,
                    Thumbnail = tt.OrderProduct.FirstOrDefault()?.Thumbnail,
                    OrderNo = tt.Order.OrderNo,
                    Address = tt.Order.Address,
                    Contact = tt.Order.Contact,
                    Mobile = tt.Order.Mobile,
                    CreationTime = tt.Order.CreationTime,
                    MemberName = tt.Member.FirstOrDefault()?.Name,
                    RefundStatus=tt.Order.RefundStatus
                });
            }

            return new PageListResultDto<OrderListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }

        public async Task<OrderItemDto> GetOrder(long id)
        {
            var orderInfo = await _orderRepository.FirstOrDefaultAsync(t => t.Id == id);
            if (orderInfo == null)
                throw new UserFriendlyException("订单不存在!");

            OrderItemDto orderDetail = new OrderItemDto();
            orderDetail.Id = orderInfo.Id;
            orderDetail.Type = orderInfo.Type;
            orderDetail.Address = orderInfo.Address;
            orderDetail.Amount = orderInfo.Amount;
            orderDetail.Contact = orderInfo.Contact;
            orderDetail.CreationTime = orderInfo.CreationTime;
            orderDetail.GrouponId = orderInfo.GrouponId;
            orderDetail.GrouponOrderId = orderInfo.JoinGrouponId;
            orderDetail.GrouponStatus = orderInfo.GrouponStatus;
            orderDetail.Mobile = orderInfo.Mobile;
            orderDetail.OrderNo = orderInfo.OrderNo;
            orderDetail.Status = orderInfo.Status;
            orderDetail.ClientRemark = orderInfo.ClientRemark;
            orderDetail.RefundStatus = orderInfo.RefundStatus;
            if(orderDetail.RefundStatus!=Core.Mall.RefundOrders.RefundStatus.None)
            {
                var refundOrderInfo =await _refundOrderRepository.FirstOrDefaultAsync(t => t.OrderId == orderDetail.Id);
                if (refundOrderInfo != null)
                    orderDetail.RefundOrderId = refundOrderInfo.Id;
            }


            var payinfo = await _orderPayRepository.FirstOrDefaultAsync(t => t.OrderId == orderInfo.Id);
            if (payinfo != null)
            {
                orderDetail.PayChannel = payinfo.Channel;
                orderDetail.PayTradeNo = payinfo.TradeNo;
                orderDetail.RefundsPayTime = payinfo.RefundsPayTime;
                orderDetail.RefundsTradeNo = payinfo.RefundsTradeNo;
            }

            var memberInfo = await _memberRepository.FirstOrDefaultAsync(t => t.Id == orderInfo.MemberId);
            if (memberInfo != null)
            {
                orderDetail.MemberName = memberInfo.Name;
            }

            var productInfo = await _orderProductRepository.FirstOrDefaultAsync(t => t.OrderId == orderInfo.Id);
            if (productInfo != null)
            {
                orderDetail.ProductName = productInfo.Name;
                orderDetail.Num = productInfo.Num;
                orderDetail.Thumbnail = productInfo.Thumbnail;
            }

            return orderDetail;
        }

        public async Task Ship(OrderShipDto dto)
        {
            var shipInfo = await _orderShipRepository.FirstOrDefaultAsync(t => t.OrderId == dto.OrderId);
            Order orderInfo = await _orderRepository.FirstOrDefaultAsync(t=>t.Id==dto.OrderId);
            if (shipInfo == null && orderInfo!=null && orderInfo.Status==OrderStatus.IsReading)
            {
                OrderShip orderShip = dto.MapTo<OrderShip>();
                await _orderShipRepository.InsertAsync(orderShip);

                
                orderInfo.Status = OrderStatus.HasShip;

                await _orderRepository.UpdateAsync(orderInfo);


                UnitOfWorkManager.Current.Completed += (sender, args) =>
                {
                    new Thread(() => EventBus.Default.Trigger(new OrderShipEventData() { OrderId = dto.OrderId })).Start();
                };
            }


        }

        
    }
}
