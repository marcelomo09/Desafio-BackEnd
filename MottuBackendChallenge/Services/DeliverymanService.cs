
public class DeliverymanService
{
    private readonly DeliverymanRepository _deliverymanRepository;

    public DeliverymanService(DeliverymanRepository repos)
    {
        _deliverymanRepository = repos;
    }

    #region Private Methods

    private async Task<Response> CNPJExists(Deliveryman deliveryman)
    {
        bool cnpjExists = await _deliverymanRepository.CNPJExists(deliveryman.Id, deliveryman.CNPJ);

        if (cnpjExists) return new Response(true, "CNPJ já encontra-se cadastrado.", ResponseTypeResults.BadRequest);

        return new Response(false, "");
    }

    private async Task<Response> DeliverymanNotExists(string id)
    {
        var exists = await _deliverymanRepository.GetDeliveryman(id);

        if (exists == null) return new Response(true, "Entregador não encontrado!", ResponseTypeResults.NotFound);

        return new Response(false, "");
    }

    private async Task<Response> CNHExists(Deliveryman deliveryman)
    {
        bool cnhExists = await _deliverymanRepository.CNHExists(deliveryman.Id, deliveryman.CNH);

        if (cnhExists) return new Response(true, "CNH já encontra-se cadastrada.", ResponseTypeResults.BadRequest); 

        return new Response(false, "");
    }

    #endregion

    #region Public Methods

    public async Task<Response> CreateDeliveryman(Deliveryman deliveryman)
    {
        Response response = await CNPJExists(deliveryman);

        if (response.Error) return response;

        response = await CNHExists(deliveryman);

        if (response.Error) return response;

        await _deliverymanRepository.CreateDeliveryman(deliveryman);

        return new Response(false, "Entregador cadastrado com sucesso!");
    }

    public async Task<Response> DeleteDeliveryman(string id)
    {
        Response response = await DeliverymanNotExists(id);

        if (response.Error) return response;

        response.Message = "Entregador excluído com sucesso!";

        await _deliverymanRepository.DeleteDeliveryman(id);

        return response;
    }

    public async Task<List<Deliveryman>> GetDeliveryDrivers()
    {
        return await _deliverymanRepository.GetDeliveryDrivers();
    }

    public async Task<Deliveryman> GetDeliveryman(string id)
    {
        return await _deliverymanRepository.GetDeliveryman(id);
    }

    public async Task<Response> UpdateDeliveryman(Deliveryman deliveryman)
    {
        Response response = await CNPJExists(deliveryman);

        if (response.Error) return response;

        response = await DeliverymanNotExists(deliveryman.Id ?? string.Empty);

        if (response.Error) return response;

        response = await CNHExists(deliveryman);

        if (response.Error) return response;

        await _deliverymanRepository.UpdateDeliveryman(deliveryman);

        response.Message = "Dados do entregador atualizados com sucesso!";

        return response;
    }

    #endregion
}