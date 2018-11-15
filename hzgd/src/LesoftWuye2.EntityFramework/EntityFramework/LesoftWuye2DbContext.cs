using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using LesoftWuye2.Core.ActivityProjects;
using LesoftWuye2.Core.Activitys;
using LesoftWuye2.Core.ApiLogs;
using LesoftWuye2.Core.Areas;
using LesoftWuye2.Core.CurrentRooms;
using LesoftWuye2.Core.Details;
using LesoftWuye2.Core.Estateinfos;
using LesoftWuye2.Core.EstateinfoTypes;
using LesoftWuye2.Core.LifeInfoProjects;
using LesoftWuye2.Core.LifeInfos;
using LesoftWuye2.Core.LifeInfoTypes;
using LesoftWuye2.Core.NewsProjects;
using LesoftWuye2.Core.Newss;
using LesoftWuye2.Core.NoticeProjects;
using LesoftWuye2.Core.Notices;
using LesoftWuye2.Core.PayNotifys;
using LesoftWuye2.Core.Projects;
using LesoftWuye2.Core.PropertyCitys;
using LesoftWuye2.Core.Propertys;
using LesoftWuye2.Core.PropertyTypes;
using LesoftWuye2.Core.Rentsaleinfos;
using LesoftWuye2.Core.RentsaleinfoTypes;
using LesoftWuye2.Core.ServiceTels;
using LesoftWuye2.Core.SlideImages;
using LesoftWuye2.Core.Substations;
using Obs.EntityFramework;
using LesoftWuye2.Core.Categories;
using LesoftWuye2.Core.FeeServiceProjects;
using LesoftWuye2.Core.FeeServices;
using LesoftWuye2.Core.ForumComments;
using LesoftWuye2.Core.ForumImages;
using LesoftWuye2.Core.ForumPosts;
using LesoftWuye2.Core.Suppliers;
using LesoftWuye2.Core.Groupons;
using LesoftWuye2.Core.Mall.Orders;
using LesoftWuye2.Core.Mall.RefundOrders;
using LesoftWuye2.Core.MallSlideImages;
using LesoftWuye2.Core.MemberAddresss;
using LesoftWuye2.Core.Orders;
using LesoftWuye2.Core.Plates;
using LesoftWuye2.Core.Products;
using LesoftWuye2.Core.RefundOrders;
using LesoftWuye2.Core.TemplateKeys;
using LesoftWuye2.Core.WeixinSubscribes;
using LesoftWuye2.Core.Wuyebase.Members;

namespace LesoftWuye2.EntityFramework.EntityFramework
{
    public class LesoftWuye2DbContext : ObsDbContext
    {
        //TODO: Define an IDbSet for your Entities...
        public virtual IDbSet<Project> Projects { get; set; }
        public virtual IDbSet<ApiLog> ApiLogs { get; set; }
        public virtual IDbSet<Notice> Notices { get; set; }
        public virtual IDbSet<NoticeProject> NoticeProjects { get; set; }
        public virtual IDbSet<Detail> Details { get; set; }

        public virtual IDbSet<News> Newss { get; set; }
        public virtual IDbSet<NewsProject> NewsProjects { get; set; }

        public virtual IDbSet<LifeInfoType> LifeInfoTypes { get; set; }

        public virtual IDbSet<PropertyCity> PropertyCitys { get; set; }

        public virtual IDbSet<PropertyType> PropertyTypes { get; set; }

        public virtual IDbSet<Property> Propertys { get; set; }

        public virtual IDbSet<LifeInfo> LifeInfos { get; set; }
        public virtual IDbSet<LifeInfoProject> LifeInfoProjects { get; set; }

        public virtual IDbSet<Activity> Activitys { get; set; }

        public virtual IDbSet<ActivityPerson> ActivityPersons { get; set; }

        public virtual IDbSet<ActivityProject> ActivityProjects { get; set; }

        public virtual IDbSet<SlideImage> SlideImages { get; set; }

        public virtual IDbSet<ServiceTel> ServiceTels { get; set; }

        public virtual IDbSet<Substation> Substations { get; set; }

        public virtual IDbSet<Estateinfo> Estateinfos { get; set; }

        public virtual IDbSet<EstateinfoComment> EstateinfoComments { get; set; }

        public virtual IDbSet<EstateinfoImage> EstateinfoImages { get; set; }

        public virtual IDbSet<EstateinfoType> EstateinfoTypes { get; set; }

        public virtual IDbSet<Rentsaleinfo> Rentsaleinfos { get; set; }

        public virtual IDbSet<RentsaleinfoImage> RentsaleinfoImages { get; set; }

        public virtual IDbSet<RentsaleinfoType> RentsaleinfoTypes { get; set; }

        public virtual IDbSet<PayNotify> PayNotifys { get; set; }

        public virtual IDbSet<CurrentRoom> CurrentRooms { get; set; }





        //Mall
        public virtual IDbSet<MallSlideImage> MallSlideImages { get; set; }
        public virtual IDbSet<Member> Members { get; set; }
        public virtual IDbSet<Category> Categories { get; set; }
        public virtual IDbSet<Supplier> Suppliers { get; set; }
        public virtual IDbSet<Product> Products { get; set; }
        public virtual IDbSet<ProductSlideImage> ProductSlideImages { get; set; }

        public virtual IDbSet<ProductComment> ProductComments { get; set; }

        public virtual IDbSet<ProductLike> ProductLikes { get; set; }

        public virtual IDbSet<Groupon> Groupons { get; set; }
        public virtual IDbSet<Area> Areas { get; set; }
        public virtual IDbSet<MemberAddress> MemberAddresss { get; set; }

        public virtual IDbSet<Order> Orders { get; set; }
        public virtual IDbSet<OrderPay> OrderPays { get; set; }

        public virtual IDbSet<OrderShip> OrderShips { get; set; }

        public virtual IDbSet<OrderProduct> OrderProducts { get; set; }
        public virtual IDbSet<GrouponOrder> GrouponOrders { get; set; }
        public virtual IDbSet<GrouponMember> GrouponMembers { get; set; }

        public virtual IDbSet<Plate> Plates { get; set; }
        public virtual IDbSet<ForumComment> ForumComments { get; set; }
        public virtual IDbSet<ForumImage> ForumImages { get; set; }
        public virtual IDbSet<ForumPost> ForumPosts { get; set; }

        public virtual IDbSet<WeixinSubscribe> WeixinSubscribes { get; set; }

        public virtual IDbSet<FeeService> FeeServices { get; set; }
        public virtual IDbSet<FeeServiceProject> FeeServiceProjects { get; set; }

        public virtual IDbSet<TemplateKey> TemplateKeys { get; set; }

        public virtual IDbSet<RefundOrder> RefundOrders { get; set; }

        public virtual IDbSet<RefundOrderImage> RefundOrderImages { get; set; }

        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */
        public LesoftWuye2DbContext()
            : base("Default")
        {

        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in ObsDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of ObsDbContext since ABP automatically handles it.
         */
        public LesoftWuye2DbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        //This constructor is used in tests
        public LesoftWuye2DbContext(DbConnection connection)
            : base(connection)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Member>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Area>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
