using MongoDB.Driver;

public class UserRepository : IUserRepository
{
    private readonly MongoDbContext _context;

    public UserRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetUsers() => await _context.Users.Find(x => true).ToListAsync();

    public async Task<User> GetUser(string id) => await _context.Users.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateUser(User user) => await _context.Users.InsertOneAsync(user);

    public async Task UpdateUser(User user) => await _context.Users.ReplaceOneAsync(x => x.Id == user.Id, user);

    public async Task DeleteUser(string id) => await _context.Users.DeleteOneAsync(x => x.Id == id);
}