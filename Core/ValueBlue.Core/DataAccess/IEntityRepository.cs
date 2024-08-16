using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.Entities.Concrete;

namespace ValueBlue.Core.DataAccess
{
    public interface IEntityRepository<TEntity>
    {
        Task AddAsync(TEntity entity);

        Task UpdateAsync(TEntity entity, string id);

        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter);

        Task<List<TEntity>> GetAllAsync(Pagination? pagination = null);

        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter, Pagination? pagination = null);

        Task DeleteAsync(string id);
    }
}
