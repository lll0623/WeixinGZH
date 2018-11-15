using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebSockets;
using System.Windows.Forms;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Castle.Components.DictionaryAdapter;
using LesoftWuye2.Application.Frontpages.Session;
using LesoftWuye2.Application.Groupons;
using LesoftWuye2.Application.Groupons.Dto;
using LesoftWuye2.Application.Malls.Dto;
using LesoftWuye2.Core.Categories;
using LesoftWuye2.Core.Groupons;
using LesoftWuye2.Core.MallSlideImages;
using LesoftWuye2.Core.Products;
using Obs.Dto;
using Obs.Filter;
using LesoftWuye2.Application.MallSlideImages.Dto;
using LesoftWuye2.Application.WuyeApis;
using LesoftWuye2.Core.Areas;
using LesoftWuye2.Core.Mall.Orders;
using LesoftWuye2.Core.MemberAddresss;
using LesoftWuye2.Core.Orders;
using LesoftWuye2.Core.SqlExecuters;
using LesoftWuye2.Core.Wuyebase.Members;
using Obs;

namespace LesoftWuye2.Application.Malls
{
    public class MallService : LesoftWuye2AppServiceBase, IMallService
    {
        private readonly IFrontSession _frontSession;

        private readonly IRepository<Groupon, long> _grouponRepository;
        private readonly IRepository<Product, long> _productRepository;
        private readonly IRepository<Category, long> _categoryRepository;
        private readonly IRepository<ProductSlideImage, long> _productSlideimageRepository;
        private readonly IRepository<ProductComment, long> _productCommentRepository;

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

        private readonly ISqlExecuter _sqlExecuter;
        private readonly IAppFolders _appFolders;

        public MallService(
            IFrontSession frontSession,
            IRepository<Groupon, long> grouponRepository,
            IRepository<Product, long> productRepository,
            IRepository<Category, long> categoryRepository,
            IRepository<ProductSlideImage, long> productSlideimageRepository,
             IRepository<ProductComment, long> productCommentRepository,
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
              ISqlExecuter sqlExecuter,
              IAppFolders appFolders
             )
        {
            _frontSession = frontSession;
            _grouponRepository = grouponRepository;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _productSlideimageRepository = productSlideimageRepository;
            _productCommentRepository = productCommentRepository;

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
            _orderShipRepository = orderShipRepository;

            _sqlExecuter = sqlExecuter;
            _appFolders = appFolders;
        }



        public async Task<List<ComboboxItemDto>> GetCategories()
        {
            var items = await _categoryRepository.GetAll().OrderBy(t => t.Sort).ToListAsync();
            var list = items.Select(result =>
            {
                var item = new ComboboxItemDto();
                item.DisplayText = result.Name;
                item.Value = result.Id.ToString();
                return item;
            }).ToList();

            list.Insert(0, new ComboboxItemDto("0", "全部"));

            return list;
        }

        public async Task<PageListResultDto<GrouponItem>> GetGrouponItems(int cid, GetPageListRequstDto dto)
        {
            var query = _grouponRepository.GetAll();
            var where = FilterExpression.FindByGroup<Groupon>(dto.Filter);



            var queryCount = query.Where(where)
                 .GroupJoin(_productRepository.GetAll(), left => left.ProductId, right => right.Id,
                   (left, right) => new
                   {
                       Groupon = left,
                       Product = right
                   });
            if (cid != 0)
            {
                queryCount = queryCount.Where(t => t.Product.FirstOrDefault().CategoryId == cid);
            }


            queryCount = queryCount.Where(t => t.Groupon.IsSale && t.Product.FirstOrDefault().IsSale);

            int count = await queryCount.CountAsync();

            var queryList = query.Where(where)
               .GroupJoin(_productRepository.GetAll(), left => left.ProductId, right => right.Id,
                   (left, right) => new
                   {
                       Groupon = left,
                       Product = right
                   });

            if (cid != 0)
            {
                queryList = queryList.Where(t => t.Product.FirstOrDefault().CategoryId == cid);
            }

            queryList = queryList.Where(t => t.Groupon.IsSale && t.Product.FirstOrDefault().IsSale);

            var tts = await queryList.Select(t => new GrouponAndProduct()
            {
                Groupon = t.Groupon,
                Product = t.Product.FirstOrDefault()
            }).OrderByDescending(t => t.Groupon.CreationTime).PageBy(dto).ToListAsync();

            var grouponListDtos = tts.Select(
                result =>
                {
                    var grouponInfo = result.Groupon.MapTo<GrouponItem>();
                    grouponInfo.Name = result.Product?.Name;
                    grouponInfo.Unit = result.Product?.Unit;
                    grouponInfo.Thumbnail = result.Product?.Thumbnail;
                    if (result.Product?.SellCount != null)
                        grouponInfo.SellCount = (result.Product?.SellCount).Value;
                    return grouponInfo;
                }).ToList();

            var pageList = grouponListDtos.MapTo<List<GrouponItem>>();
            return new PageListResultDto<GrouponItem>(count, pageList, dto.CurrentPage, dto.PageSize);
        }

        public async Task<GrouponDetail> GetGrouponDetail(long id)
        {
            var grouponInfo = await _grouponRepository.GetAsync(id);
            if (grouponInfo == null)
                throw new UserFriendlyException("拼团信息不存在!");
            var productInfo = await _productRepository.GetAsync(grouponInfo.ProductId);


            GrouponDetail detail = new GrouponDetail();
            detail.Id = grouponInfo.Id;
            detail.Name = productInfo.Name;
            detail.CommentCount = 0;
            detail.Comments = new List<GrouponDetail.CommentItem>();
            detail.Content = productInfo.Content;
            detail.IsSale = grouponInfo.IsSale;
            detail.Price = grouponInfo.Price;
            detail.RequireCount = grouponInfo.RequireCount;
            detail.SalePrice = productInfo.SalePrice;
            detail.SellCount = productInfo.SellCount;
            detail.SlideImages = new List<string>();
            detail.Summary = grouponInfo.Summary;
            detail.Tags = new List<string>(productInfo.ServiceTags.Split(','));
            detail.Unit = productInfo.Unit;
            detail.Thumbnail = productInfo.Thumbnail;
            detail.ProductId = productInfo.Id;
            detail.MemberPrice = productInfo.MemberPrice;
            detail.Comments = new List<GrouponDetail.CommentItem>();


            var images = await _productSlideimageRepository.GetAll()
                .OrderBy(t => t.Sort)
                .Where(t => t.ProductId == productInfo.Id)
                .ToListAsync();
            foreach (var image in images)
            {
                detail.SlideImages.Add(image.Thumbnail);
            }

            //查询已经开团的数据(只显示最近的2条)
            var grouponings = await _groupOrderRepository.GetAll()
                .GroupJoin(_memberRepository.GetAll(), left => left.HeaderId, right => right.Id, (left, right) => new
                {
                    GrouponOrder = left,
                    Member = right
                })
                .Where(t => t.GrouponOrder.GrouponId == grouponInfo.Id && t.GrouponOrder.GrouponStatus == GrouponStatus.Grouponing).OrderByDescending(t => t.GrouponOrder.CreationTime)
                .Take(5)
                .ToListAsync();
            detail.GrouponingItems = new List<GrouponDetail.GrouponingItem>();
            foreach (var grouponing in grouponings)
            {
                detail.GrouponingItems.Add(new GrouponDetail.GrouponingItem()
                {
                    Id = grouponing.GrouponOrder.Id,
                    ExpireTime = grouponing.GrouponOrder.ExpireTime,
                    Name = grouponing.Member.FirstOrDefault()?.Name,
                    Thumbnail = grouponing.Member.FirstOrDefault()?.Thumbnail,
                    RequireCount = grouponing.GrouponOrder.RequireCount - grouponing.GrouponOrder.JoinCount,
                    BindRooms = grouponing.Member.FirstOrDefault()?.BindRooms
                });
            }

            detail.CommentCount = await _productCommentRepository.CountAsync(t => t.ProductId == detail.ProductId);
            var comments = await _productCommentRepository.GetAll()
                .GroupJoin(_memberRepository.GetAll(), left => left.MemberId, right => right.Id, (left, right) => new
                {
                    ProductComment = left,
                    Member = right
                })
                .Where(t => t.ProductComment.ProductId == grouponInfo.ProductId).OrderByDescending(t => t.ProductComment.CreationTime)
                .Take(2)
                .ToListAsync();

            foreach (var comment in comments)
            {
                detail.Comments.Add(new GrouponDetail.CommentItem()
                {
                    Name = comment.Member.FirstOrDefault()?.Name,
                    Thumbnail = comment.Member.FirstOrDefault()?.Thumbnail,
                    Content = comment.ProductComment.Content,
                    CreationTime = comment.ProductComment.CreationTime
                });
            }

            return detail;
        }

        public async Task<MallHomeData> GetHomeData()
        {
            MallHomeData data = new MallHomeData();

            var images = await _mallslideimageRepository.GetAll().OrderBy(t => t.Sort).ToListAsync();
            data.SlideImages = images.MapTo<List<MallSlideImageListDto>>();


            var query = _grouponRepository.GetAll();
            var where = FilterExpression.FindByGroup<Groupon>(new List<FilterCondition>());


            var queryList = query.Where(where)
               .GroupJoin(_productRepository.GetAll(), left => left.ProductId, right => right.Id,
                   (left, right) => new
                   {
                       Groupon = left,
                       Product = right
                   });

            queryList = queryList.Where(t => t.Groupon.IsSale && t.Product.FirstOrDefault().IsSale);

            var tts = await queryList.Select(t => new GrouponAndProduct()
            {
                Groupon = t.Groupon,
                Product = t.Product.FirstOrDefault()
            }).OrderByDescending(t => t.Groupon.CreationTime).ToListAsync();

            var grouponListDtos = tts.Select(
                result =>
                {
                    var grouponInfo = result.Groupon.MapTo<GrouponItem>();
                    grouponInfo.Name = result.Product?.Name;
                    grouponInfo.Unit = result.Product?.Unit;
                    grouponInfo.Thumbnail = result.Product?.Thumbnail;
                    if (result.Product?.SellCount != null)
                        grouponInfo.SellCount = (result.Product?.SellCount).Value;
                    return grouponInfo;
                }).ToList();

            data.Groupons = grouponListDtos.MapTo<List<GrouponItem>>();

            return data;
        }

        public async Task<List<ComboboxItemDto>> GetProvinces()
        {
            var items = await _areaRepository.GetAll().Where(t => t.Level == 1).OrderBy(t => t.Sort).ToListAsync();
            var list = items.Select(result =>
            {
                var item = new ComboboxItemDto();
                item.DisplayText = result.Short_name;
                item.Value = result.Id.ToString();
                return item;
            }).ToList();
            return list;
        }

        public async Task<List<ComboboxItemDto>> GetCities(long provinceId)
        {
            var items = await _areaRepository.GetAll().Where(t => t.Level == 2 && t.Parent_id == provinceId).OrderBy(t => t.Sort).ToListAsync();
            var list = items.Select(result =>
            {
                var item = new ComboboxItemDto();
                item.DisplayText = result.Short_name;
                item.Value = result.Id.ToString();
                return item;
            }).ToList();
            return list;
        }

        public async Task<List<ComboboxItemDto>> GetDistricts(long cityId)
        {
            var items = await _areaRepository.GetAll().Where(t => t.Level == 3 && t.Parent_id == cityId).OrderBy(t => t.Sort).ToListAsync();
            var list = items.Select(result =>
            {
                var item = new ComboboxItemDto();
                item.DisplayText = result.Short_name;
                item.Value = result.Id.ToString();
                return item;
            }).ToList();
            return list;
        }

        public async Task AddAddress(MemberAddress address)
        {
            //如果地址设置为默认，先将该用户的其他地址设置为非默认
            if (address.IsDefault)
            {
                var defaults = await _memberAddressRepository.GetAll().Where(t => t.IsDefault && t.MemberId == address.MemberId).ToListAsync();
                foreach (var @default in defaults)
                {
                    @default.IsDefault = false;
                    await _memberAddressRepository.UpdateAsync(@default);
                }
            }

            await _memberAddressRepository.InsertAsync(address);
        }

        public async Task EditAddress(MemberAddress address)
        {
            var oldAddress = await _memberAddressRepository.GetAsync(address.Id);

            oldAddress.Contact = address.Contact;
            oldAddress.Mobile = address.Mobile;
            oldAddress.Address = address.Address;
            oldAddress.IsDefault = address.IsDefault;
            oldAddress.ProvinceId = address.ProvinceId;
            oldAddress.CityId = address.CityId;
            oldAddress.DistrictId = address.DistrictId;

            await _memberAddressRepository.UpdateAsync(oldAddress);
        }

        public async Task<List<MemberAddressListItem>> GetAddresss(long memberId)
        {
            var query = _memberAddressRepository.GetAll();
            var where = FilterExpression.FindByGroup<MemberAddress>(new List<FilterCondition>());


            var queryList = query.Where(where)
                .GroupJoin(_areaRepository.GetAll(), left => left.ProvinceId, province => province.Id,
                    (left, province) => new
                    {
                        MemberAddress = left,
                        Province = province
                    })
                .GroupJoin(_areaRepository.GetAll(), left => left.MemberAddress.CityId, city => city.Id,
                    (left, city) => new
                    {
                        MemberAddress = left.MemberAddress,
                        Province = left.Province,
                        City = city
                    })
               .GroupJoin(_areaRepository.GetAll(), left => left.MemberAddress.DistrictId, cistrict => cistrict.Id,
                    (left, cistrict) => new
                    {
                        MemberAddress = left.MemberAddress,
                        Province = left.Province,
                        City = left.City,
                        District = cistrict
                    });

            queryList = queryList.Where(t => t.MemberAddress.MemberId == memberId);

            var tts = await queryList.Select(t => new MemberAddressAndAera()
            {
                MemberAddress = t.MemberAddress,
                Province = t.Province.FirstOrDefault(),
                City = t.City.FirstOrDefault(),
                District = t.District.FirstOrDefault()
            }).OrderByDescending(t => t.MemberAddress.CreationTime).ToListAsync();

            var addresss = tts.Select(
                result =>
                {
                    var info = result.MemberAddress.MapTo<MemberAddressListItem>();
                    info.ProvinceName = result.Province?.Short_name;
                    info.CityName = result.City?.Short_name;
                    info.DistrictName = result.District?.Short_name;
                    return info;
                }).ToList();
            return addresss;
        }

        public async Task DeleteAddress(long id)
        {
            await _memberAddressRepository.DeleteAsync(id);
        }

        public async Task SetDefaultAddress(long id)
        {
            var addresInfo = await _memberAddressRepository.GetAsync(id);

            var defaults = await _memberAddressRepository.GetAll().Where(t => t.IsDefault && t.MemberId == addresInfo.MemberId).ToListAsync();
            foreach (var @default in defaults)
            {
                @default.IsDefault = false;
                await _memberAddressRepository.UpdateAsync(@default);
            }
            addresInfo.IsDefault = true;
            await _memberAddressRepository.UpdateAsync(addresInfo);
        }

        public async Task<MemberAddress> GetAddress(long id)
        {
            var addresInfo = await _memberAddressRepository.GetAsync(id);
            return addresInfo;
        }

        public async Task<bool> CheckMemberIsOwner(string id)
        {
            var memberInfo = await _memberRepository.FirstOrDefaultAsync(Convert.ToInt64(id));
            if (string.IsNullOrEmpty(memberInfo?.BindRooms))
                return false;
            return true;
        }

        public async Task<MemberAddressListItem> GetAddressForSubmit(long memberId, long selectAddressId)
        {
            var query = _memberAddressRepository.GetAll();
            var where = FilterExpression.FindByGroup<MemberAddress>(new List<FilterCondition>());


            var queryList = query.Where(where)
                .GroupJoin(_areaRepository.GetAll(), left => left.ProvinceId, province => province.Id,
                    (left, province) => new
                    {
                        MemberAddress = left,
                        Province = province
                    })
                .GroupJoin(_areaRepository.GetAll(), left => left.MemberAddress.CityId, city => city.Id,
                    (left, city) => new
                    {
                        MemberAddress = left.MemberAddress,
                        Province = left.Province,
                        City = city
                    })
               .GroupJoin(_areaRepository.GetAll(), left => left.MemberAddress.DistrictId, cistrict => cistrict.Id,
                    (left, cistrict) => new
                    {
                        MemberAddress = left.MemberAddress,
                        Province = left.Province,
                        City = left.City,
                        District = cistrict
                    });

            queryList = queryList.Where(t => t.MemberAddress.MemberId == memberId);
            if (selectAddressId != 0)
            {
                if (
                    await
                        _memberAddressRepository.FirstOrDefaultAsync(
                            t => t.MemberId == memberId && t.Id == selectAddressId) != null)
                {
                    queryList = queryList.Where(t => t.MemberAddress.Id == selectAddressId);
                }
            }

            var tts = await queryList.Select(t => new MemberAddressAndAera()
            {
                MemberAddress = t.MemberAddress,
                Province = t.Province.FirstOrDefault(),
                City = t.City.FirstOrDefault(),
                District = t.District.FirstOrDefault()
            }).OrderByDescending(t => t.MemberAddress.IsDefault).FirstOrDefaultAsync();

            if (tts != null)
            {
                MemberAddressListItem item = tts.MemberAddress.MapTo<MemberAddressListItem>();
                item.ProvinceName = tts.Province?.Short_name;
                item.CityName = tts.City?.Short_name;
                item.DistrictName = tts.District?.Short_name;
                return item;
            }

            return null;
        }

        public async Task<OrderSubmitResult> SubmitOrder(OrderSubmitInfo info)
        {
            //1.先检查参数有效性
            if (info.Type != 0 && info.Type != 1)
            {
                throw new UserFriendlyException("无效的订单.");
            }

            var productInfo = await _productRepository.FirstOrDefaultAsync(info.ProductId);
            if (productInfo == null || !productInfo.IsSale)
                throw new UserFriendlyException("商品信息不存在或已下架!");

            var grouponInfo = await _grouponRepository.FirstOrDefaultAsync(info.GrouponId);
            if (info.Type == 0)
            {
                if (grouponInfo == null || !grouponInfo.IsSale)
                    throw new UserFriendlyException("拼团信息不存在或已下架!");
                if(grouponInfo.ExpireTime<DateTime.Now)
                    throw new UserFriendlyException("拼团已结束!");
            }

            var memberInfo = await _memberRepository.FirstOrDefaultAsync(info.MemberId);
            if (memberInfo == null)
                throw new UserFriendlyException("用户信息不存在!");

            var query = _memberAddressRepository.GetAll();
            var where = FilterExpression.FindByGroup<MemberAddress>(new List<FilterCondition>());


            var queryList = query.Where(where)
                .GroupJoin(_areaRepository.GetAll(), left => left.ProvinceId, province => province.Id,
                    (left, province) => new
                    {
                        MemberAddress = left,
                        Province = province
                    })
                .GroupJoin(_areaRepository.GetAll(), left => left.MemberAddress.CityId, city => city.Id,
                    (left, city) => new
                    {
                        MemberAddress = left.MemberAddress,
                        Province = left.Province,
                        City = city
                    })
               .GroupJoin(_areaRepository.GetAll(), left => left.MemberAddress.DistrictId, cistrict => cistrict.Id,
                    (left, cistrict) => new
                    {
                        MemberAddress = left.MemberAddress,
                        Province = left.Province,
                        City = left.City,
                        District = cistrict
                    });

            queryList = queryList.Where(t => t.MemberAddress.MemberId == memberInfo.Id && t.MemberAddress.Id == info.AddressId);
            var addressInfo = await queryList.FirstOrDefaultAsync();
            if (addressInfo == null)
            {
                throw new UserFriendlyException("用户地址不存在!");
            }

            //检查如果是参团，一个团不允许参与2次
            if (info.Type == (int)OrderType.Groupon && info.GrouponOrderId != 0)
            {
                if (await _groupMemberRepository.CountAsync(t => t.MemberId == memberInfo.Id && t.GrouponOrderId == info.GrouponOrderId) > 0)
                    throw new UserFriendlyException("一个团只能参与一次.");
            }


            //2.先插入订单
            Order order = new Order();
            order.OrderNo = "D" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            order.MemberId = memberInfo.Id;
            if (info.Type == 0)
            {
                order.Amount = grouponInfo.Price * info.Num;
                order.GrouponId = grouponInfo.Id;
            }
            if (info.Type == 1)
            {
                if (string.IsNullOrEmpty(memberInfo.BindRooms))
                    order.Amount = productInfo.SalePrice * info.Num;
                else
                {
                    order.Amount = productInfo.MemberPrice * info.Num;
                }
                order.GrouponId = grouponInfo.Id;
            }
            order.Type = (OrderType)info.Type;
            order.Status = OrderStatus.WaitingPay;
            order.GrouponStatus = GrouponStatus.None;
            order.Address = addressInfo.Province.FirstOrDefault()?.Short_name +
                            addressInfo.City.FirstOrDefault()?.Short_name +
                            addressInfo.District.FirstOrDefault()?.Short_name +
                            addressInfo.MemberAddress.Address;
            order.ProvinceId = addressInfo.MemberAddress.ProvinceId;
            order.CityId = addressInfo.MemberAddress.CityId;
            order.DistrictId = addressInfo.MemberAddress.DistrictId;
            order.Contact = addressInfo.MemberAddress.Contact;
            order.Mobile = addressInfo.MemberAddress.Mobile;
            order.JoinGrouponId = info.GrouponOrderId;

            var orderId = await _orderRepository.InsertAndGetIdAsync(order);

            //3.插入订单商品

            OrderProduct orderProduct = new OrderProduct();
            orderProduct.OrderId = order.Id;
            orderProduct.ProductId = productInfo.Id;
            if (order.Type == OrderType.Groupon)
                orderProduct.GrouponPrice = grouponInfo.Price;
            orderProduct.Name = productInfo.Name;
            orderProduct.Specification = productInfo.Specification;
            orderProduct.Unit = productInfo.Unit;
            orderProduct.Price = productInfo.Price;
            orderProduct.SalePrice = productInfo.SalePrice;
            orderProduct.MemberPrice = productInfo.MemberPrice;
            orderProduct.Thumbnail = productInfo.Thumbnail;
            orderProduct.Summary = productInfo.Summary;
            orderProduct.Num = info.Num;


            await _orderProductRepository.InsertAsync(orderProduct);

            OrderSubmitResult result = new OrderSubmitResult();
            result.OrderNo = order.OrderNo;
            result.Amount = order.Amount;
            result.OrderId = orderId;
            return result;
        }

        public void PayOrder(OrderPayInfo payInfo)
        {
            //1.查出订单信息
            //2.保存支付信息，更新订单状态
            //3.如果是拼团，开团/或者参团
            //4.参团检查是否拼团成功

            var orderInfo = _orderRepository.FirstOrDefault(t => t.OrderNo == payInfo.OrderNo);
            if (orderInfo == null)
                throw new UserFriendlyException("订单不存在!");

            var groupInfo = _grouponRepository.FirstOrDefault(t => t.Id == orderInfo.GrouponId);
            if (groupInfo == null && orderInfo.Type == OrderType.Groupon)
                throw new UserFriendlyException("拼团信息不存在!");

            var oldpayInfo = _orderPayRepository.FirstOrDefault(t => t.OrderId == orderInfo.Id);
            if (oldpayInfo != null)
            {
                Logger.Info($"订单:{payInfo.OrderNo}已经支付");
                return;
            }




            var orderPay = new OrderPay();
            orderPay.OrderId = orderInfo.Id;
            orderPay.Channel = payInfo.Channel;
            orderPay.Money = payInfo.Money;
            orderPay.TradeNo = payInfo.TradeNo;
            orderPay.PayTime = DateTime.Now;

            _orderPayRepository.Insert(orderPay);

            orderInfo.IsPay = true;
            switch (orderInfo.Type)
            {
                case OrderType.Normal:
                    orderInfo.Status = OrderStatus.IsReading;
                    _orderRepository.Update(orderInfo);
                    break;
                case OrderType.Groupon:

                    //判断是开团还是参团
                    if (orderInfo.JoinGrouponId == 0)
                    {
                        GrouponOrder grouponOrder = new GrouponOrder();
                        grouponOrder.GrouponId = orderInfo.GrouponId;
                        grouponOrder.ExpireTime = DateTime.Now.AddDays(groupInfo.ValidDay);
                        grouponOrder.GrouponStatus = GrouponStatus.Grouponing;
                        grouponOrder.HeaderId = orderInfo.MemberId;
                        grouponOrder.RequireCount = groupInfo.RequireCount;
                        grouponOrder.JoinCount = 1;
                        var grouponOrderId = _groupOrderRepository.InsertAndGetId(grouponOrder);

                        GrouponMember grouponMember = new GrouponMember();
                        grouponMember.IsHeader = true;
                        grouponMember.MemberId = orderInfo.MemberId;
                        grouponMember.GrouponOrderId = grouponOrder.Id;
                        _groupMemberRepository.Insert(grouponMember);

                        //开团
                        orderInfo.Status = OrderStatus.Grouponing;
                        orderInfo.GrouponStatus = GrouponStatus.Grouponing;
                        orderInfo.JoinGrouponId = grouponOrderId;
                        _orderRepository.Update(orderInfo);

                    }
                    else
                    {
                        //参团
                        var grouponOrder = _groupOrderRepository.FirstOrDefault(t => t.Id == orderInfo.JoinGrouponId);
                        if (grouponOrder == null)
                            throw new UserFriendlyException("拼团订单不存在");

                        GrouponMember grouponMember = new GrouponMember();
                        grouponMember.MemberId = orderInfo.MemberId;
                        grouponMember.GrouponOrderId = grouponOrder.Id;
                        _groupMemberRepository.Insert(grouponMember);

                        grouponOrder.JoinCount++;
                        if (grouponOrder.JoinCount >= grouponOrder.RequireCount)
                        {
                            grouponOrder.GrouponSuccessTime = DateTime.Now;
                            grouponOrder.GrouponStatus = GrouponStatus.GrouponSuccess;

                            orderInfo.GrouponStatus = GrouponStatus.GrouponSuccess;
                            orderInfo.Status = OrderStatus.IsReading;

                            //找出对应该拼团的所有订单，修改状态

                            var ret = _sqlExecuter.Execute("update orders set Status=" + (int)OrderStatus.IsReading +
                                                  ",GrouponStatus=" + (int)GrouponStatus.GrouponSuccess +
                                                  " where JoinGrouponId=" + orderInfo.JoinGrouponId);

                        }

                        _groupOrderRepository.Update(grouponOrder);
                        _orderRepository.Update(orderInfo);
                    }
                    break;
            }

            var productInfo = _productRepository.FirstOrDefault(t => t.Id == groupInfo.ProductId);
            if (productInfo != null)
                _sqlExecuter.Execute("update Products set SellCount=SellCount+1 where id=" + productInfo.Id);

        }

        public async Task<GrouponOrderInfo> GetGrouponOrder(long id)
        {
            var info = await _groupOrderRepository.GetAll()
                .GroupJoin(_grouponRepository.GetAll(), left => left.GrouponId, right => right.Id,
                    (left, right) => new
                    {
                        GrouponOrder = left,
                        Groupon = right
                    })
                .GroupJoin(_productRepository.GetAll(), left => left.Groupon.FirstOrDefault().ProductId,
                    right => right.Id,
                    (left, right) => new
                    {
                        GrouponOrder = left.GrouponOrder,
                        Groupon = left.Groupon,
                        Product = right
                    })
                .Where(t => t.GrouponOrder.Id == id)
                .FirstOrDefaultAsync();

            if (info == null)
                throw new UserFriendlyException("拼团订单不存在!");

            GrouponOrderInfo grouponOrderInfo = new GrouponOrderInfo();
            grouponOrderInfo.ExpireTime = info.GrouponOrder.ExpireTime;
            grouponOrderInfo.GrouponStatus = info.GrouponOrder.GrouponStatus;
            grouponOrderInfo.JoinCount = info.GrouponOrder.JoinCount;
            grouponOrderInfo.Price = (info.Groupon.FirstOrDefault()?.Price) ?? 0;
            grouponOrderInfo.ProductName = info.Product.FirstOrDefault()?.Name;
            grouponOrderInfo.RequireCount = info.GrouponOrder.RequireCount;
            grouponOrderInfo.Thumbnail = info.Product.FirstOrDefault()?.Thumbnail;
            grouponOrderInfo.Unit = info.Product.FirstOrDefault()?.Unit;
            grouponOrderInfo.GrouponId = info.GrouponOrder.GrouponId;
            grouponOrderInfo.Id = info.GrouponOrder.Id;


            grouponOrderInfo.JoinMembers = new List<GrouponOrderMemberInfo>();

            var members = await _groupMemberRepository.GetAll()
                .GroupJoin(_memberRepository.GetAll(), left => left.MemberId, right => right.Id,
                    (left, right) => new
                    {
                        GrouponMember = left,
                        Member = right
                    })
                 .Where(t => t.GrouponMember.GrouponOrderId == grouponOrderInfo.Id)
                .ToListAsync();

            foreach (var member in members)
            {
                grouponOrderInfo.JoinMembers.Add(new GrouponOrderMemberInfo()
                {
                    IsHeader = member.GrouponMember.IsHeader,
                    JoinTime = member.GrouponMember.CreationTime,
                    Name = member.Member.FirstOrDefault()?.Name,
                    Thumbnail = member.Member.FirstOrDefault()?.Thumbnail
                });
            }


            return grouponOrderInfo;
        }

        public async Task<OrderDetail> GetOrderDetail(long id)
        {
            var orderInfo = await _orderRepository.FirstOrDefaultAsync(t => t.Id == id);
            if (orderInfo == null)
                throw new UserFriendlyException("订单不存在!");

            OrderDetail orderDetail = new OrderDetail();
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
            orderDetail.RefundStatus = orderInfo.RefundStatus;

            var payinfo = await _orderPayRepository.FirstOrDefaultAsync(t => t.OrderId == orderInfo.Id);
            if (payinfo != null)
            {
                orderDetail.PayChannel = payinfo.Channel;
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

        public async Task<PageListResultDto<GrouponOrderListItem>> GetGrouponOrders(long memberId, int type, GetPageListRequstDto dto)
        {

            string sql = @"
select OrderProducts.Name as ProductName,OrderProducts.Thumbnail,orders.Amount,GrouponOrders.RequireCount,
Orders.Id as OrderId,GrouponOrders.Id as GrouponOrderId,GrouponOrders.GrouponStatus as Status
from GrouponMembers left join GrouponOrders on GrouponMembers.GrouponOrderId=GrouponOrders.Id left join Orders on Orders.JoinGrouponId=GrouponOrders.Id left join OrderProducts on OrderProducts.OrderId=Orders.Id";

            sql += " where GrouponMembers.MemberId=" + memberId + " and Orders.MemberId=" + memberId;

            if (type != 0)
            {
                if (type == 1)
                    sql += " and GrouponOrders.GrouponStatus = " + (int)GrouponStatus.Grouponing;
                if (type == 2)
                    sql += " and  GrouponOrders.GrouponStatus = " + (int)GrouponStatus.GrouponSuccess;
                if (type == 3)
                    sql += " and  GrouponOrders.GrouponStatus in ('" + (int)GrouponStatus.GrouponFail + "','" +
                           (int)GrouponStatus.GrouponFail + "')";
            }

            sql += " order by GrouponMembers.CreationTime desc";

            var pageList = _sqlExecuter.SqlQuery<GrouponOrderListItem>(sql).ToList();





            //var query = _groupMemberRepository.GetAll();
            //var where = FilterExpression.FindByGroup<GrouponMember>(dto.Filter);



            //var queryCount = query.Where(where)
            //     .GroupJoin(_groupOrderRepository.GetAll(), left => left.GrouponOrderId, right => right.Id,
            //        (left, right) => new
            //        {
            //            GrouponMember = left,
            //            GrouponOrder = right
            //        })
            //    .GroupJoin(_orderRepository.GetAll(), left => left.GrouponOrder.FirstOrDefault().Id, right => right.JoinGrouponId,
            //        (left, right) => new
            //        {
            //            GrouponMember = left.GrouponMember,
            //            GrouponOrder = left.GrouponOrder,
            //            Order = right

            //        })
            //        .GroupJoin(_orderProductRepository.GetAll(), left => left.Order.FirstOrDefault().Id, right => right.OrderId,
            //        (left, right) => new
            //        {
            //            GrouponMember = left.GrouponMember,
            //            GrouponOrder = left.GrouponOrder,
            //            Order = left.Order,
            //            OrderProduct =right
            //        });

            //    queryCount = queryCount.Where(t => t.GrouponMember.MemberId==memberId );

            //if (type != 0)
            //{
            //    if(type==1)
            //        queryCount = queryCount.Where(t => t.GrouponOrder.FirstOrDefault().GrouponStatus==GrouponStatus.Grouponing);
            //    if (type == 2)
            //        queryCount = queryCount.Where(t => t.GrouponOrder.FirstOrDefault().GrouponStatus == GrouponStatus.GrouponSuccess);
            //    if (type == 3)
            //        queryCount = queryCount.Where(t => (t.GrouponOrder.FirstOrDefault().GrouponStatus == GrouponStatus.GrouponFail || t.GrouponOrder.FirstOrDefault().GrouponStatus == GrouponStatus.GrouponFailAndRefunds));
            //}



            //int count = await queryCount.CountAsync();

            //var queryList = query.Where(where)
            //  .GroupJoin(_groupOrderRepository.GetAll(), left => left.GrouponOrderId, right => right.Id,
            //        (left, right) => new
            //        {
            //            GrouponMember = left,
            //            GrouponOrder = right
            //        })
            //    .GroupJoin(_orderRepository.GetAll(), left => left.GrouponOrder.FirstOrDefault().Id, right => right.JoinGrouponId,
            //        (left, right) => new
            //        {
            //            GrouponMember = left.GrouponMember,
            //            GrouponOrder = left.GrouponOrder,
            //            Order = right

            //        })
            //        .GroupJoin(_orderProductRepository.GetAll(), left => left.Order.FirstOrDefault().Id, right => right.OrderId,
            //        (left, right) => new
            //        {
            //            GrouponMember = left.GrouponMember,
            //            GrouponOrder = left.GrouponOrder,
            //            Order = left.Order,
            //            OrderProduct = right
            //        });


            //queryList = queryList.Where(t => t.GrouponMember.MemberId == memberId);

            //if (type != 0)
            //{
            //    if (type == 1)
            //        queryCount = queryCount.Where(t => t.GrouponOrder.FirstOrDefault().GrouponStatus == GrouponStatus.Grouponing);
            //    if (type == 2)
            //        queryCount = queryCount.Where(t => t.GrouponOrder.FirstOrDefault().GrouponStatus == GrouponStatus.GrouponSuccess);
            //    if (type == 3)
            //        queryCount = queryCount.Where(t => (t.GrouponOrder.FirstOrDefault().GrouponStatus == GrouponStatus.GrouponFail || t.GrouponOrder.FirstOrDefault().GrouponStatus == GrouponStatus.GrouponFailAndRefunds));
            //}

            //var tts = await queryList.OrderByDescending(t => t.GrouponMember.CreationTime).PageBy(dto).ToListAsync();

            //var pageList=new List<GrouponOrderListItem>();

            //foreach (var tt in tts)
            //{
            //    pageList.Add(new GrouponOrderListItem()
            //    {
            //        Amount = tt.Order.FirstOrDefault().Amount,
            //        GrouponOrderId = tt.GrouponOrder.FirstOrDefault().Id,
            //        OrderId = tt.Order.FirstOrDefault().Id,
            //        ProductName = tt.OrderProduct.FirstOrDefault().Name,
            //        RequireCount = tt.GrouponOrder.FirstOrDefault().RequireCount,
            //        Thumbnail = tt.OrderProduct.FirstOrDefault().Thumbnail,
            //        Status = tt.GrouponOrder.FirstOrDefault().GrouponStatus
            //    });
            //}

            return new PageListResultDto<GrouponOrderListItem>(10, pageList, dto.CurrentPage, dto.PageSize);
        }

        public async Task<PageListResultDto<OrderListItem>> GetOrders(long memberId, int type, GetPageListRequstDto dto)
        {
            var query = _orderRepository.GetAll();
            var where = FilterExpression.FindByGroup<Order>(dto.Filter);



            var queryCount = query.Where(where)
                 .GroupJoin(_orderProductRepository.GetAll(), left => left.Id, right => right.OrderId,
                   (left, right) => new
                   {
                       Order = left,
                       OrderProduct = right
                   });

            queryCount = queryCount.Where(t => t.Order.MemberId == memberId);

            if (type > 0)
            {
                if (type == 1)
                    queryCount = queryCount.Where(t => t.Order.Status == OrderStatus.WaitingPay);
                if (type == 2)
                    queryCount = queryCount.Where(t => t.Order.Status == OrderStatus.Grouponing);
                if (type == 3)
                    queryCount = queryCount.Where(t => t.Order.Status == OrderStatus.IsReading);
                if (type == 4)
                    queryCount = queryCount.Where(t => t.Order.Status == OrderStatus.HasShip);
                if (type == 5)
                    queryCount = queryCount.Where(t => t.Order.Status == OrderStatus.Received);
            }

            int count = await queryCount.CountAsync();

            var queryList = query.Where(where)
               .GroupJoin(_orderProductRepository.GetAll(), left => left.Id, right => right.OrderId,
                   (left, right) => new
                   {
                       Order = left,
                       OrderProduct = right
                   });


            queryList = queryList.Where(t => t.Order.MemberId == memberId);


            if (type > 0)
            {
                if (type == 1)
                    queryList = queryList.Where(t => t.Order.Status == OrderStatus.WaitingPay);
                if (type == 2)
                    queryList = queryList.Where(t => t.Order.Status == OrderStatus.Grouponing);
                if (type == 3)
                    queryList = queryList.Where(t => t.Order.Status == OrderStatus.IsReading);
                if (type == 4)
                    queryList = queryList.Where(t => t.Order.Status == OrderStatus.HasShip);
                if (type == 5)
                    queryList = queryList.Where(t => t.Order.Status == OrderStatus.Received);
            }



            var tts = await queryList.OrderByDescending(t => t.Order.CreationTime).PageBy(dto).ToListAsync();
            var pageList = new List<OrderListItem>();
            foreach (var tt in tts)
            {
                pageList.Add(new OrderListItem()
                {
                    Amount = tt.Order.Amount,
                    GrouponStatus = tt.Order.GrouponStatus,
                    OrderId = tt.Order.Id,
                    ProductName = tt.OrderProduct.FirstOrDefault().Name,
                    Status = tt.Order.Status,
                    Type = tt.Order.Type,
                    Thumbnail = tt.OrderProduct.FirstOrDefault().Thumbnail,
                    OrderNo = tt.Order.OrderNo,
                    RefundStatus=tt.Order.RefundStatus

                });
            }

            return new PageListResultDto<OrderListItem>(count, pageList, dto.CurrentPage, dto.PageSize);
        }


        public async Task ConfirmReceived(long id)
        {
            var shipInfo = await _orderShipRepository.FirstOrDefaultAsync(t => t.OrderId == id);
            Order orderInfo = await _orderRepository.FirstOrDefaultAsync(t => t.Id == id);
            if (orderInfo != null && orderInfo.Status == OrderStatus.HasShip && shipInfo != null)
            {

                orderInfo.Status = OrderStatus.Received;
                await _orderRepository.UpdateAsync(orderInfo);

                shipInfo.ReceivedTime = DateTime.Now;
                shipInfo.ReceivedType = OrderReceivedType.User;

                await _orderShipRepository.UpdateAsync(shipInfo);
            }
        }

        public async Task OrderComment(OrderCommentInfo info)
        {
            if (info.Content.Length < 5 || info.Content.Length > 100)
            {
                throw new UserFriendlyException("评论内容长度请在5~100之间");
            }
            Order orderInfo = await _orderRepository.FirstOrDefaultAsync(t => t.Id == info.OrderId);
            if (orderInfo == null)
                throw new UserFriendlyException("订单不存在!");

            var productInfo = await _orderProductRepository.FirstOrDefaultAsync(t => t.OrderId == orderInfo.Id);
            if (productInfo == null)
            {
                throw new UserFriendlyException("商品信息不存在!");
            }

            ProductComment productComment = new ProductComment();
            productComment.Content = info.Content;
            productComment.MemberId = _frontSession.MemberId;
            productComment.ProductId = productInfo.ProductId;
            productComment.SourceOrderId = orderInfo.Id;

            await _productCommentRepository.InsertAsync(productComment);
        }

        public async Task<PageListResultDto<GrouponDetail.CommentItem>> GetProductComments(long productId, GetPageListRequstDto dto)
        {

            var query = _productCommentRepository.GetAll();
            var where = FilterExpression.FindByGroup<ProductComment>(dto.Filter);


            var queryCount = query.Where(where)
                .GroupJoin(_memberRepository.GetAll(), left => left.MemberId, right => right.Id, (left, right) => new
                {
                    ProductComment = left,
                    Member = right
                });

            queryCount = queryCount.Where(t => t.ProductComment.ProductId == productId);
            int count = await queryCount.CountAsync();




            var queryList = query.Where(where)
             .GroupJoin(_memberRepository.GetAll(), left => left.MemberId, right => right.Id, (left, right) => new
             {
                 ProductComment = left,
                 Member = right
             });


            queryList = queryList.Where(t => t.ProductComment.ProductId == productId);


            var comments = await queryList.OrderByDescending(t => t.ProductComment.CreationTime).PageBy(dto).ToListAsync();


            List<GrouponDetail.CommentItem> Comments = comments.Select(comment => new GrouponDetail.CommentItem()
            {
                Name = comment.Member.FirstOrDefault()?.Name,
                Thumbnail = comment.Member.FirstOrDefault()?.Thumbnail,
                Content = comment.ProductComment.Content,
                CreationTime = comment.ProductComment.CreationTime
            }).ToList();

            return new PageListResultDto<GrouponDetail.CommentItem>(count, Comments, dto.CurrentPage, dto.PageSize);
        }
    }
}

