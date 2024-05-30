public interface IUserRepository
{
    Task<List<User>> GetUsers();
    Task<User> GetUser(string id);
    Task CreateUser(User user);
    Task UpdateUser(User user);
    Task DeleteUser(string id);
}