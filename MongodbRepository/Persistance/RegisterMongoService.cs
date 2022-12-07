namespace MongodbRepository.Persistance
{
    public static class RegisterMongoService
    {

        public static IServiceCollection AddMongoContext(this IServiceCollection services,IConfiguration configuration)
        {
          

            services.Configure<MongoDbSetting>( configuration.GetSection("MongoDbSetting"));

            services.AddSingleton<MongoContext>();
            services.AddScoped(typeof(IMongoRepository<>),typeof(MongoRepository<>));
            return services;
        }
    }
}
