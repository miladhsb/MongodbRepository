using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongodbRepository.Entities;
using System.Linq.Expressions;

namespace MongodbRepository.Persistance
{
    public interface IMongoRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<bool> DeleteOneAsync(string id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<List<TEntity>> GetAsync(FilterDefinition<TEntity> FilterBuilder, SortDefinition<TEntity> sortBuilder, int PageSize, int PageNumber);
        Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IMongoQueryable<TEntity>, IOrderedMongoQueryable<TEntity>> orderBy = null, int PageSize = 10, int PageNumber = 1);
        Task<TEntity> GetByIdAsync(string id);
        Task<bool> UpdateOneAsync(TEntity entity);
    }
}
