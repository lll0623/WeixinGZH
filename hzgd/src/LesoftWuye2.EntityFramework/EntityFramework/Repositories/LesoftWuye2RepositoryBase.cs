using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;


namespace LesoftWuye2.EntityFramework.EntityFramework.Repositories
{
    public abstract class LesoftWuye2RepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<LesoftWuye2DbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected LesoftWuye2RepositoryBase(IDbContextProvider<LesoftWuye2DbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class LesoftWuye2RepositoryBase<TEntity> : LesoftWuye2RepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected LesoftWuye2RepositoryBase(IDbContextProvider<LesoftWuye2DbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
