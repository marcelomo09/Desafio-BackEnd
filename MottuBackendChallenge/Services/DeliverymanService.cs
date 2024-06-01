
public class DeliverymanService
{
    private readonly DeliverymanRepository _deliverymanRepository;

    public DeliverymanService(DeliverymanRepository repos)
    {
        _deliverymanRepository = repos;
    }

    #region Private Methods

    /// <summary>
    /// Verifica se algum entregador possui o CNPJ informado e que sejá diferente da identificação inserida
    /// </summary>
    /// <param name="cnpj">CNPJ do entregador</param>
    /// <param name="id">Número de identificação do entregador</param>
    /// <returns>Retorna um objeto com propriedades que identificam erros ou não</returns>
    private async Task<Response> CNPJExists(string cnpj, string? id)
    {
        bool cnpjExists = await _deliverymanRepository.CNPJExists(id, cnpj);

        if (cnpjExists) return new Response(true, "CNPJ já encontra-se cadastrado.", ResponseTypeResults.BadRequest);

        return new Response(false, "");
    }

    /// <summary>
    /// Verifica se o entregador existe
    /// </summary>
    /// <param name="id">Número de identificação do entregador</param>
    /// <returns>Retorna um objeto com propriedades que identificam erros ou não</returns>
    private async Task<Response> DeliverymanNotExists(string id)
    {
        var exists = await _deliverymanRepository.GetDeliveryman(id);

        if (exists == null) return new Response(true, "Entregador não encontrado!", ResponseTypeResults.NotFound);

        return new Response(false, "");
    }

    /// <summary>
    /// Verifica se algum entregador possui a CNH informada e que sejá diferente da identificação inserida
    /// </summary>
    /// <param name="cnh">Número da CNH do entregador</param>
    /// <param name="id">Número de identificação do entregador</param>
    /// <returns>Retorna um objeto com propriedades que identificam erros ou não</returns>
    private async Task<Response> CNHExists(string cnh, string? id)
    {
        bool cnhExists = await _deliverymanRepository.CNHExists(id, cnh);

        if (cnhExists) return new Response(true, "CNH já encontra-se cadastrada.", ResponseTypeResults.BadRequest); 

        return new Response(false, "");
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Cria na base de dados o registro do Entregador
    /// </summary>
    /// <param name="deliveryman">Objeto contendo todos os dados necessários para o cadastro do entregador</param>
    /// <returns>Retorna um objeto com propriedades que identificam erros ou não</returns>
    public async Task<Response> CreateDeliveryman(Deliveryman deliveryman)
    {
        Response response = await CNPJExists(deliveryman.CNPJ, deliveryman.Id);

        if (response.Error) return response;

        response = await CNHExists(deliveryman.CNH, deliveryman.Id);

        if (response.Error) return response;

        await _deliverymanRepository.CreateDeliveryman(deliveryman);

        return new Response(false, "Entregador cadastrado com sucesso!");
    }

    /// <summary>
    /// Excluí o registro do entregador
    /// </summary>
    /// <param name="id">Número de identificação do entregador</param>
    /// <returns>Retorna um objeto com propriedades que identificam erros ou não</returns>
    public async Task<Response> DeleteDeliveryman(string id)
    {
        Response response = await DeliverymanNotExists(id);

        if (response.Error) return response;

        response.Message = "Entregador excluído com sucesso!";

        await _deliverymanRepository.DeleteDeliveryman(id);

        return response;
    }

    /// <summary>
    /// Retorna a lista de todos os entregadores cadastrados
    /// </summary>
    /// <returns>Lista dos entregadores cadastrados</returns>
    public async Task<List<Deliveryman>> GetDeliveryDrivers()
    {
        return await _deliverymanRepository.GetDeliveryDrivers();
    }

    /// <summary>
    /// Busca um entregador especifico pela sua identificação
    /// </summary>
    /// <param name="id">Número de identificação do entregador</param>
    /// <returns>Retorna o objeto com todos os dados do entregador encontrado</returns>
    public async Task<Deliveryman> GetDeliveryman(string id)
    {
        return await _deliverymanRepository.GetDeliveryman(id);
    }

    /// <summary>
    /// Realiza a atualização de dados do entregador
    /// </summary>
    /// <param name="deliveryman">Dados de atualização do entregador</param>
    /// <returns>Retorna um objeto com propriedades que identificam erros ou não</returns>
    public async Task<Response> UpdateDeliveryman(Deliveryman deliveryman)
    {
        Response response = await CNPJExists(deliveryman.CNPJ, deliveryman.Id);

        if (response.Error) return response;

        response = await DeliverymanNotExists(deliveryman.Id ?? string.Empty);

        if (response.Error) return response;

        response = await CNHExists(deliveryman.CNH, deliveryman.Id);

        if (response.Error) return response;

        // Excluí o arquivo anterior caso o novo tenha um nome diferente
        if (!string.IsNullOrEmpty(deliveryman.ImageCNHPath))
        {
            var deliverymanNow = await _deliverymanRepository.GetDeliveryman(deliveryman.Id ?? string.Empty);

            if (deliverymanNow.ImageCNHPath != deliveryman.ImageCNHPath && File.Exists(deliveryman.ImageCNHPath)) File.Delete(deliveryman.ImageCNHPath);
        }
        
        await _deliverymanRepository.UpdateDeliveryman(deliveryman);

        response.Message = "Dados do entregador atualizados com sucesso!";

        return response;
    }

    /// <summary>
    /// Realiza a atualização da foto de CNH do entregador
    /// </summary>
    /// <param name="id">Número de identificação do entregador</param>
    /// <param name="imagecnhpath">Caminho ao qual a foto foi armazenada</param>
    /// <returns>Retorna um objeto com propriedades que identificam erros ou não</returns>
    public async Task<Response> UpdateImageCNHPath(string id, string imagecnhpath)
    {
        var deliveryman = await _deliverymanRepository.GetDeliveryman(id);

        if (deliveryman == null) return new Response(true, "Entragador não identificado.", ResponseTypeResults.NotFound);

        // Excluí o arquivo anterior caso o novo tenha um nome diferente
        if (deliveryman.ImageCNHPath != imagecnhpath && File.Exists(deliveryman.ImageCNHPath)) File.Delete(deliveryman.ImageCNHPath);

        deliveryman.ImageCNHPath = imagecnhpath;

        await _deliverymanRepository.UpdateDeliveryman(deliveryman);

        return new Response(true, "Imagem da CNH atualizada com sucesso.");
    }

    #endregion
}