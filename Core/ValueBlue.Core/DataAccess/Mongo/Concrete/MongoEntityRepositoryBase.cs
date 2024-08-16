using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.Entities.Abstract;
using ValueBlue.Core.Entities.Concrete;
using ValueBlue.Core.Utilities.Concrete;

namespace ValueBlue.Core.DataAccess.Mongo.Concrete
{
    public abstract class MongoEntityRepositoryBase<TEntity> : IEntityRepository<TEntity> where TEntity : class, IDocumentEntity, new()
    {
        protected readonly IMongoCollection<TEntity> collection;

        protected MongoEntityRepositoryBase(IMongoDatabase database)
        {
            collection = database.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await collection.InsertOneAsync(entity);
        }

        public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            var data = await collection.FindAsync(filter);
            return data.SingleOrDefault();
        }

        public virtual async Task<List<TEntity>> GetAllAsync(Pagination? pagination = null)
        {
            FindOptions<TEntity>? findOptions = GetPaginationOptions(pagination);

            var all = await collection.FindAsync(Builders<TEntity>.Filter.Empty, findOptions);
            
            
            return await all.ToListAsync();
        }

        public virtual async Task DeleteAsync(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, objectId);
            await collection.DeleteOneAsync(filter);
        }

         public virtual async Task UpdateAsync(TEntity entity, string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, objectId);
            await collection.ReplaceOneAsync(filter, entity);
        }

        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter, Pagination? pagination = null)
        {
            FindOptions<TEntity>? findOptions = GetPaginationOptions(pagination);

            var data = await collection.FindAsync(filter, findOptions);
            return await data.ToListAsync();
        }

        public async Task<List<TEntity>> GetPaginatedResults(int pageNumber, int pageSize)
        {
            var skip = (pageNumber - 1) * pageSize;
            var all = await collection.Find(Builders<TEntity>.Filter.Empty)
                                      .Skip(skip)
                                      .Limit(pageSize)
                                      .ToListAsync();

            return all;
        }

        public FindOptions<TEntity>? GetPaginationOptions(Pagination? pagination)
        {
            FindOptions<TEntity>? findOptions = null;
            if (PaginationService.ShouldUsePagination(pagination))
            {
                var skip = (pagination.Page - 1) * pagination.PageSize;

                findOptions = new FindOptions<TEntity>
                {
                    Skip = skip,
                    Limit = pagination.PageSize
                };
            }

            return findOptions;
        }
    }
}
