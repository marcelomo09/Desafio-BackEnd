using Microsoft.EntityFrameworkCore;

public class UserService : ServiceBase
{
    public UserService(MongoDBContext mongoDBContext) : base(mongoDBContext)
    {

    }
    
    /// <summary>
    /// Busca todos os usuários cadastrados
    /// </summary>
    /// <returns>Retorna a lista dos usuários cadastrados</returns>
    public async Task<List<User>> GetAll()
    {
        var users = await _dbContext.Users.ToListAsync();

        return users;
    }

    /// <summary>
    /// Cria um usuário desde que não tenho como UserName admin
    /// </summary>
    /// <param name="request">Dados do usuário para cadastro</param>
    /// <returns>Retorna uma resposta do processo de cadastro do usuário</returns>
    public async Task<Response> Create(CreateUserRequest request)
    {
        try
        {
            if (await _dbContext.Users.AnyAsync(u => u.UserName == request.UserName)) return new Response(true, "Usuário já cadastrado.", ResponseTypeResults.BadRequest);

            await _dbContext.Users.AddAsync(new User(request));

            await _dbContext.SaveChangesAsync();

            return new Response(false, "Usuário cadastrado com sucesso!");
        }
        catch (Exception ex)
        {
            return new Response(true, $"Ocorreu uma exeção no processo de criação do usuário: {ex.Message}", ResponseTypeResults.BadRequest);
        }
    }
    
    /// <summary>
    /// Excluí um usuário desde que não seja o admin
    /// </summary>
    /// <param name="userName">Nome do usuário a ser excluído</param>
    /// <returns>Retorna uma resposta do processo de exclusão do usuário</returns>
    public async Task<Response> Delete(string userName)
    {
        try
        {
            var user = await _dbContext.Users.Where(u => u.UserName == userName).FirstOrDefaultAsync();

            if (user == null) return new Response(false, "Usuário não encontrado.", ResponseTypeResults.NotFound);

            if (user.UserName == "admin") return new Response(true, "Usuário admin não pode ser excluído.", ResponseTypeResults.BadRequest);            

            _dbContext.Users.Remove(user);

            await _dbContext.SaveChangesAsync();

            return new Response(false, "Usuário excluído com sucesso!");
        }
        catch (Exception ex)
        {
            return new Response(true, $"Ocorreu uma exeção no processo de exclusão do usuário: {ex.Message}", ResponseTypeResults.BadRequest);
        }
    }
}