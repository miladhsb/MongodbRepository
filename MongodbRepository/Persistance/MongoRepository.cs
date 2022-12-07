using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongodbRepository.Entities;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MongodbRepository.Persistance
{
    public class MongoRepository<TEntity> : IMongoRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly IMongoCollection<TEntity> DbSet;
 

        public MongoRepository(MongoContext mongoContext)
        {
            this.DbSet = mongoContext.GetCollection<TEntity>();
        }

        public virtual async Task<TEntity> AddAsync(TEntity  entity)
        {
            try
            {
                entity.Id = BsonObjectId.GenerateNewId().AsObjectId.ToString();
                await DbSet.InsertOneAsync(entity);
                return entity;
            }
            catch (Exception ex)
            {
               
                throw new MongoClientException(ex.Message);
            }

        }
        public virtual async Task<List<TEntity>> GetAsync(FilterDefinition<TEntity> FilterBuilder,SortDefinition<TEntity> sortBuilder, int PageSize, int PageNumber)
        {

           var data= await DbSet.FindAsync(FilterBuilder,options:new FindOptions<TEntity>() {Sort=sortBuilder ,Limit= PageSize ,Skip= (PageNumber - 1) * PageSize });
           return data.ToList();
        }

        public virtual async Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IMongoQueryable<TEntity>, IOrderedMongoQueryable<TEntity>> orderBy = null, int PageSize = 10, int PageNumber = 1)
        {

           var query = DbSet.AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return await query.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToListAsync();
        }

       

        public virtual async Task<TEntity> GetByIdAsync(string id)
        {
            // var FilterBuilder = Builders<TEntity>.Filter.Eq("_id", id);
            //or
            // var data = await DbSet.FindAsync<TEntity>(p=>p.Id==id);
            //or
            //var data =  DbSet.AsQueryable().Where(p=>p.Id==id).FirstOrDefault();
            //or
           // var data = await DbSet.AsQueryable().FirstOrDefaultAsync(p => p.Id == id);

            var FilterBuilder = Builders<TEntity>.Filter.Eq(p=>p.Id, id);
            var data = await DbSet.FindAsync(FilterBuilder);
            return data.SingleOrDefault();
        }



        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {

            // var all = await DbSet.AsQueryable().ToListAsync();
          

            //sorting
            //sortBuilder= Builders<TEntity>.Sort.Ascending(p => p.Id);
            //var findOptions = new FindOptions<TEntity>() { Sort = sortBuilder };
            //var all = await DbSet.FindAsync(Builders<TEntity>.Filter.Empty, findOptions);
            //or
            //var all = await DbSet.AsQueryable().OrderBy(p=>p.Id).ToListAsync();


            var all = await DbSet.FindAsync(Builders<TEntity>.Filter.Empty);
            return all.ToList();
        }

        public virtual async Task<bool> UpdateOneAsync(TEntity entity)
        {
            //var FilterBuilder = Builders<TEntity>.Filter.Eq(p => p.Id, entity.Id);
            //var UpdateBuilder =  Builders<TEntity>.Update.Set<TEntity>(p => entity, entity);
            //DbSet.UpdateOne(FilterBuilder, UpdateBuilder);

          

            try
            {
                var Result = await DbSet.ReplaceOneAsync<TEntity>(p => p.Id == entity.Id, entity);
                return Result.IsAcknowledged;
            }
            catch (Exception ex)
            {

                throw new MongoClientException(ex.Message);
            }


        }

        public virtual async Task<bool> DeleteOneAsync(string id)
        {

            try
            {
                var Result = await DbSet.DeleteOneAsync<TEntity>(p => p.Id == id);
                return Result.IsAcknowledged;
            }
            catch (Exception ex)
            {

                throw new MongoClientException(ex.Message);
            }


        }




    }
}
