using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using LesoftWuye2.Application.Weixin.Events.EventDatas;
using LesoftWuye2.Core.Mall.Orders;
using LesoftWuye2.Core.Orders;

namespace LesoftWuye2.Application.Jobs
{
    public class GrouponCancelWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {

        private readonly IRepository<Order, long> _orderRepository;
        private readonly IRepository<GrouponOrder, long> _groupOrderRepository;
        private readonly IRepository<OrderProduct, long> _orderProductRepository;
        public GrouponCancelWorker(AbpTimer timer,
            IRepository<Order, long> orderRepository,
            IRepository<GrouponOrder, long> groupOrderRepository,
            IRepository<OrderProduct, long> orderProductRepository

            ) : base(timer)
        {
            _orderRepository = orderRepository;
            _groupOrderRepository = groupOrderRepository;
            _orderProductRepository = orderProductRepository;
            Timer.Period = 1000 * 60 * 10; //每10分钟执行一次
        }

        [UnitOfWork]
        protected override void DoWork()
        {
            //将已经过期的团购订单设置为失败,并退款
            //_orderRepository.GetAll().Where(t=>t.Type==OrderType.Groupon && t.GrouponStatus==GrouponStatus.Grouponing)

            var groupOrdrs = _groupOrderRepository.GetAll()
                 .Where(t => t.GrouponStatus == GrouponStatus.Grouponing && t.ExpireTime <= DateTime.Now)
                 .ToList();

            Logger.Info("GrouponCancelWorker 过期拼团数量:" + groupOrdrs.Count);

            GrouponFailEventData eventData = new GrouponFailEventData();

            foreach (var grouponOrder in groupOrdrs)
            {
                //设置对应订单的状态
                var orderInfos = _orderRepository.GetAll().Where(t => t.JoinGrouponId == grouponOrder.Id).ToList();
                foreach (var orderInfo in orderInfos)
                {
                    if (orderInfo != null && orderInfo.Type == OrderType.Groupon && orderInfo.GrouponStatus == GrouponStatus.Grouponing)
                    {
                        var orderProduct = _orderProductRepository.FirstOrDefault(t => t.OrderId == orderInfo.Id);
                        orderInfo.Status = OrderStatus.Cancel;
                        orderInfo.GrouponStatus = GrouponStatus.GrouponFail;
                        _orderRepository.Update(orderInfo);

                        eventData.Orders.Add(new GrouponFailEventData.CanceOrder()
                        {
                            OrderId = orderInfo.Id,
                            MemberId = orderInfo.MemberId,
                            Price = orderInfo.Amount,
                            ProductName = orderProduct.Name
                        });
                    }
                }
               
         

                //修改团购状态 
                grouponOrder.GrouponStatus = GrouponStatus.GrouponFail;
                _groupOrderRepository.Update(grouponOrder);
            }

            //
            UnitOfWorkManager.Current.Completed += (sender, args) =>
            {
                new Thread(() => EventBus.Default.Trigger(eventData)).Start();
            };
        }
    }
}
