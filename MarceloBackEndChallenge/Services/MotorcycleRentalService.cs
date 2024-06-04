using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Entities;

public class MotorcycleRentalService : ServiceBase
{
    private readonly IOptions<MongoSettings> _settings;

    public MotorcycleRentalService(MongoDBContext mongoDBContext, IOptions<MongoSettings> settings) : base(mongoDBContext)
    {
        _settings = settings;
    }

    #region Private Methods

    /// <summary>
    /// Calcula quanto custará ao entregador a locação
    /// </summary>
    /// <param name="plan">Plano selecionado pelo entregador</param>
    /// <param name="start">Data de inicio da locação</param>
    /// <param name="end">Data que o entregador pretendde devolver a moto</param>
    /// <returns>Retorna o valor total que será pago pela locação</returns>
    private float CalcMotorcycleRental(RentalPriceTableSettingsValue plan, DateTime startDate, string end)
    {
        DateTime expectedDate = startDate.AddDays(plan.Days);
        DateTime endDate      = DateTime.Parse(end);

        float price = plan.Price;
        float total = price;

        if (endDate < expectedDate)
        {
            TimeSpan difference = endDate.Subtract(startDate);

            int diffDays = difference.Days;

            float valueAddForDay = price * plan.AssessmentPercent / 100;

            total = (plan.Days - diffDays) * valueAddForDay;

            total += price;
        }
        else if (endDate > expectedDate)
        {
            TimeSpan difference = endDate.Subtract(expectedDate);
            
            int diffDays = difference.Days;

            total = 50 * diffDays;

            total += price;
        }

        return total;
    }

    #endregion Private Methods

    #region  Public Methods

    /// <summary>
    /// Busca todas as locações de motos realizadas
    /// </summary>
    /// <returns></returns>
    public async Task<List<MotorcycleRentalResponse>> GetAll()
    {   
        var motorcycleRentals = await _dbContext.MotorcycleRentals.ToListAsync();

        var response = new List<MotorcycleRentalResponse>();

        motorcycleRentals.ForEach(x => response.Add(new MotorcycleRentalResponse(x)));

        return response;
    }

    /// <summary>
    /// Realiza a locação da moto pelo entregador
    /// </summary>
    /// <param name="request">Dados para requisição da locação</param>
    /// <returns>Retorna uma resposta do processo de locação da moto</returns>
    public async Task<Response> Create(CreateMotorcycleRentalRequest request)
    {
        try
        {
            // Verifica se a moto selecionada está disponivel
            var motorcycle = await _dbContext.Motorcycles.FirstOrDefaultAsync(x => x.Plate == request.Plate);

            if (motorcycle == null) return new Response(true, "Moto não encontrada pela placa informada.", ResponseTypeResults.NotFound);

            bool motorcycleRentalInUse = await _dbContext.MotorcycleRentals.AnyAsync(x => x.IdMotocycle == motorcycle.IdMotorcycle && x.Active == 1);

            if (motorcycleRentalInUse) return new Response(true, "Moto selecionada já encontra-se alugada.", ResponseTypeResults.BadRequest);

            // Valida se o entregador pode alugar a moto
            var deliveryman = await _dbContext.DeliveryDrivers.FirstOrDefaultAsync(x => x.CNH == request.CNH);

            if (deliveryman == null) return new Response(true, "Entregador não encontrado.", ResponseTypeResults.NotFound);

            if (deliveryman.TypeCNH.ToUpper() != "A" && deliveryman.TypeCNH.ToUpper() != "A+B") return new Response(true, "Entregador não possuí uma carteira de motorista valida para a locação da moto.", ResponseTypeResults.BadRequest);

            // Verifica qual plano foi selecionado e calcula o valor total ao final
            var rentalsPrices = _settings.Value.Collections.RentalsPricesTable.Prices;

            var plan = rentalsPrices.FirstOrDefault(x => x.Days == request.PlanOfLocation);

            if (plan == null) return new Response(true, $"Os planos disponiveis em dias são {string.Join(", ", rentalsPrices.Select(s => s.Days))}, informe qualquer um desses dias, por favor.");

            DateTime startDate    = DateTime.Now.AddDays(1);
            DateTime expectedDate = startDate.AddDays(plan.Days);

            float total = plan != null ? CalcMotorcycleRental(plan, startDate, request.EndDate) : 0;

            // Realiza a locação da moto
            await _dbContext.MotorcycleRentals.AddAsync(new MotorcycleRental(deliveryman.IdDeliveryman, motorcycle.IdMotorcycle, new Date(startDate), new Date(expectedDate), total, request));

            await _dbContext.SaveChangesAsync();

            return new Response(false, $"Locação da Moto {motorcycle.Model} realizada com sucesso!");
        }
        catch (Exception ex)
        {
            return new Response(true, $"Ocorreu uma exceção no metodo de criação dos registros de aluguel da moto: {ex.Message}", ResponseTypeResults.BadRequest);
        }
    }

    /// <summary>
    /// Simula o valor total a ser pego pelo entregador antes fazer o aluguel
    /// </summary>
    /// <param name="request">Dados para execução da simulação</param>
    /// <returns>Retorna uma resposta do processo de simulação do aluguel de moto</returns>
    public Response SimulationOfMotorcycleRentalValues(SimulationOfMotorcycleRentalValuesRequest request)
    {
        try
        {
            // Verifica qual plano foi selecionado e calcula o valor total ao final
            var rentalsPrices = _settings.Value.Collections.RentalsPricesTable.Prices;

            var plan = rentalsPrices.FirstOrDefault(x => x.Days == request.PlanOfLocation);
        
            if (plan == null) return new Response(true, $"Os planos disponiveis em dias são {string.Join(", ", rentalsPrices.Select(s => s.Days))}, informe qualquer um desses dias, por favor.");

            // Calcula o valor de aluguel no final da locação
            float total = CalcMotorcycleRental(plan, DateTime.Parse(request.StartDate), request.EndDate);

            return new Response(false, $"O total a ser pago é {total.ToString("C", CultureInfo.GetCultureInfo("pt-BR"))}");
        }
        catch (Exception ex)
        {
            return new Response(true, $"Ocorreu uma exceção no metodo de simulação dos valores de aluguel da moto: {ex.Message}", ResponseTypeResults.BadRequest);
        }
    }

    /// <summary>
    /// Finaliza a locação da moto
    /// </summary>
    /// <param name="cnh">CNH do entregador</param>
    /// <param name="plate">Placa da moto</param>
    /// <returns>Retorna uma resposta do processo de finalizar o pedido de locação</returns>
    public async Task<Response> FinalizeRental(string cnh, string plate)
    {
        try
        {
            // Busca o entregador
            var deliveryman = await _dbContext.DeliveryDrivers.FirstOrDefaultAsync(x => x.CNH == cnh);

            if (deliveryman == null) return new Response(true, "Entregador não encontrado.", ResponseTypeResults.NotFound);

            // Busca a moto
            var motorcycle = await _dbContext.Motorcycles.FirstOrDefaultAsync(x => x.Plate == plate);

            if (motorcycle == null) return new Response(true, "Moto não encontrada.", ResponseTypeResults.NotFound);

            // Busca o pedido de locação da moto
            var rental = await _dbContext.MotorcycleRentals.FirstOrDefaultAsync(x => x.IdDeliveryman == deliveryman.IdDeliveryman && x.IdMotocycle == motorcycle.IdMotorcycle && x.Active == 1);

            if (rental == null) return new Response(true, "Não foi localizado nenhum pedido de locação para o entregador e moto informados.", ResponseTypeResults.NotFound);

            // Faz o calculo total e finaliza o pedido
            var plan = _settings.Value.Collections.RentalsPricesTable.Prices.FirstOrDefault(x => x.Days == rental.PlanOfLocation);

            if (plan == null) return new Response(true, "O Plano utilizado para fazer o pedido não foi encontrado, entre em contato com o administrador do sistema.", ResponseTypeResults.BadRequest);

            Date endDate = new Date();

            float total = plan != null ? CalcMotorcycleRental(plan, DateTime.Parse(rental.StartDate.ToString()), endDate.ToString()) : 0;

            rental.EndDate    = endDate;
            rental.TotalValue = total;
            rental.Active     = 0;

            await _dbContext.SaveChangesAsync();

            return new Response(true, "Pedido finalizado com sucesso!");
        }
        catch (Exception ex)
        {
            return new Response(true, $"Ocorreu uma exceção no metodo finalizar aluguel da moto: {ex.Message}", ResponseTypeResults.BadRequest);
        }
    }

    #endregion Public Methods
}