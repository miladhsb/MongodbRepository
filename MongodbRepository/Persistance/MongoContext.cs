using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using MongodbRepository.Entities;

namespace MongodbRepository.Persistance
{
    public class MongoContext
    {
        private readonly IMongoDatabase _mongoDatabase;

        public MongoContext(IOptions<MongoDbSetting> options)
        {
            //var connectionString = "mongodb://user1:password1@localhost/test";

            IMongoClient mongoClient = new MongoClient(new MongoClientSettings()
            {
                Server = new MongoServerAddress(options.Value.ServerIP,27017),
                //Credential=MongoCredential.CreateCredential(options.Value.Database, options.Value.UserName, options.Value.Password),
                ConnectTimeout=new TimeSpan(0,0,5),
                
                ClusterConfigurator = (p) => {
                    p.Subscribe<CommandStartedEvent>(s =>
                    {
                        Console.WriteLine($"Log mongodb CommandName : {s.CommandName} CommandDetail :{s.Command.ToJson()}");

                    });
                
                }
                

            });
            _mongoDatabase = mongoClient.GetDatabase(options.Value.Database);

           
        }


        public IMongoCollection<TEntity> GetCollection<TEntity>()
        {
            return _mongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name);
         
        }



        //#region Collection
        //public IMongoCollection<Customer> Customers { get => _mongoDatabase.GetCollection<Customer>("Customers"); }

        //#endregion



    }
}
