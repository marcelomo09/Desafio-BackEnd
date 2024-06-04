using Microsoft.EntityFrameworkCore;
using MongoDB.Entities;

public class DeliverymanService: ServiceBase
{

    private readonly IConfiguration _configuration;

    public DeliverymanService(IConfiguration configuration, MongoDBContext mongoDBContext) : base(mongoDBContext)
    {
        _configuration = configuration;
    }


    #region Private Methods

    /// <summary>
    /// Salva a foto enviada no cadastro do entregador na pasta UploadsCNH que fica na raiz do projeto
    /// </summary>
    /// <param name="file">Foto da CNH do entregador</param>
    /// <returns>Retorna uma resposta do processo de busca a foto a ser salva</returns>
    private async Task<Response> GetPathImageCNHPathAndSaveOrReplace(IFormFile? file)
    {
        try
        {
            if (file == null || file.Length == 0) return new Response(true, "Foto da CNH deve ser enviada para o cadastro, enviar em PNG ou BMP.", ResponseTypeResults.BadRequest);

            if (file.ContentType != "image/png" && file.ContentType != "image/bmp") return new Response(true, "Foto da CNH deve ser em PNG ou BMP!", ResponseTypeResults.BadRequest);

            string filepath = Path.Combine(Directory.GetCurrentDirectory(), _configuration.GetSection("UploadsCNHFolder").Value ?? "UploadsCNH", file.FileName);

            if (File.Exists(filepath))
            {
                File.Delete(filepath);    
            }

            using (var stream = new FileStream(filepath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return new Response(false, filepath);
        }
        catch (Exception ex)
        {
            return new Response(true, $"Ocorreu uma exeção durante a busca do caminho onde a foto será salva: {ex.Message}", ResponseTypeResults.BadRequest);
        }
    }

    #endregion Private Methods

    #region  Public Methods

    /// <summary>
    /// Busca uma lista de todos os entregadores cadastrados
    /// </summary>
    /// <returns>Retorno da lista de todos entregadores cadastrados</returns>
    public async Task<List<DeliverymanResponse>> GetAll()
    {
        try
        {
            var delioveryDrivers =  await _dbContext.DeliveryDrivers.ToListAsync();

            var response = new List<DeliverymanResponse>();

            delioveryDrivers.ForEach(x => response.Add(new DeliverymanResponse(x)));

            return response;
        }
        catch (Exception ex)
        {
            throw new Exception($"Ocorreu uma exceção na GetAll da DeliverymanService: {ex.Message}");
        }
    }

    /// <summary>
    /// Realiza o cadastro do entregador
    /// </summary>
    /// <param name="request">Dados para cadastro do entregador</param>
    /// <returns>Retorna uma resposta do processo de cadastro do entregador</returns>
    public async Task<Response> Create(CreateDeliverymanRequest request)
    {
        try
        {
            // Verifica se o CNPJ já não está cadastrado
            var deliverymanCNPJExists = await _dbContext.DeliveryDrivers.FirstOrDefaultAsync(x => x.CNPJ == request.CNPJ);

            if (deliverymanCNPJExists != null) return new Response(true, $"CNPJ já cadastrado para o entregador {deliverymanCNPJExists.CNPJ}", ResponseTypeResults.BadRequest);

            // Verifica se a CNH já não está cadastrada
            var deliverymanCNHExists = await _dbContext.DeliveryDrivers.FirstOrDefaultAsync(x => x.CNH == request.CNH);

            if (deliverymanCNHExists != null) return new Response(true, $"CNH já cadastrado para o entregador {deliverymanCNHExists.CNH}", ResponseTypeResults.BadRequest);

            // Valida e salva a foto da CNH
            var responseImageCNH = await GetPathImageCNHPathAndSaveOrReplace(request.ImageCNH);

            if (responseImageCNH.Error) return responseImageCNH;

            // Cadastra o entregador
            await _dbContext.DeliveryDrivers.AddAsync(new Deliveryman(responseImageCNH.Message, request));

            await _dbContext.SaveChangesAsync();

            return new Response(false, "Entregador cadastrado com sucesso!");
        }
        catch (Exception ex)
        {
            return new Response(true, $"Ocorreu uma exceção no processo de inclusão do entregador: {ex.Message}");
        }
    }

    /// <summary>
    /// Excluí o cadastro do entregador
    /// </summary>
    /// <param name="cnh">CNH do entregador</param>
    /// <returns>Retorna uma resposta do processo de exclusão do entregador</returns>
    public async Task<Response> Delete(string cnh)
    {
        try
        {
            var deliveryman = await _dbContext.DeliveryDrivers.FirstOrDefaultAsync(x => x.CNH == cnh);

            if (deliveryman == null) return new Response(true, "Entregador não encontrado.", ResponseTypeResults.BadRequest);

            _dbContext.DeliveryDrivers.Remove(deliveryman);

            await _dbContext.SaveChangesAsync();

            return new Response(false, "Entregador excluído com sucesso!");
        }
        catch (Exception ex)
        {
            return new Response(true, $"Ocorreu uma exceção no processo de exclusão do entregador: {ex.Message}");
        }
    }

    /// <summary>
    /// Atualiza a foto da CNH do entregador
    /// </summary>
    /// <param name="cnh">CNH do entregador</param>
    /// <param name="imageCNH">Foto da CNH do entregador</param>
    /// <returns>Retorna uma resposta do processo de atualização da foto de CNH do entregador</returns>
    public async Task<Response> UpdateImageCNH(string cnh, IFormFile imageCNH)
    {
        try
        {
            // Busca o entregador ao qual terá a foto da cnh alterada
            var deliveryman = await _dbContext.DeliveryDrivers.FirstOrDefaultAsync(x => x.CNH == cnh);

            if (deliveryman == null) return new Response(true, "Entregador não encontrado.", ResponseTypeResults.NotFound);

            // Valida e salva a foto da CNH
            var responseImageCNH = await GetPathImageCNHPathAndSaveOrReplace(imageCNH);

            if (responseImageCNH.Error) return responseImageCNH;

            // Excluí o arquivo anterior caso o novo tenha um nome diferente
            if (deliveryman.ImageCNHPath != responseImageCNH.Message && File.Exists(deliveryman.ImageCNHPath)) File.Delete(deliveryman.ImageCNHPath);

            // Atualiza o dado da foto da CNH
            deliveryman.ImageCNHPath = responseImageCNH.Message;

            await _dbContext.SaveChangesAsync();

            return new Response(false, "Foto da CNH do entregador atualizada com sucesso!");
        }
        catch (Exception ex)
        {
            return new Response(true, $"Ocorreu uma exceção no processo de atualização da foto do entregador: {ex.Message}");
        }
    }

    #endregion Public Methods
}