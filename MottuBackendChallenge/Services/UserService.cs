public class UserService: IUserRepository
{
    private readonly UserRepository _userRepository;

    public UserService(UserRepository repos)
    {
        _userRepository = repos;
    }

    public async Task CreateUser(User user)
    {
        await _userRepository.CreateUser(user);
    }

    public async Task DeleteUser(string id)
    {
        await _userRepository.DeleteUser(id);
    }

    public async Task<User> GetUser(string id)
    {
        return await _userRepository.GetUser(id);
    }

    public async Task<List<User>> GetUsers()
    {
        return await _userRepository.GetUsers();
    }

    public async Task UpdateUser(User user)
    {
        await _userRepository.UpdateUser(user);
    }
}