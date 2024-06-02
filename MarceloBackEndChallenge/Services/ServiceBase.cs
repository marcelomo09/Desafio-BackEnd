public class ServiceBase
{
    protected readonly MongoDBContext _dbContext;

    public ServiceBase(MongoDBContext mongoDBContext)
    {
        _dbContext = mongoDBContext;
    }
}