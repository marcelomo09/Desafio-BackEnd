
using System.Globalization;
using MongoDB.Entities;

public class MotorcycleRentalService
{
    private readonly MotorcycleRentalRepository _motorcycleRentalRepository;
    private readonly MotorcycleRepository       _motorcycleRepository;
    private readonly DeliverymanRepository      _deliverymanRepository;
    private readonly RentalPriceTableRepository _rentalPriceTableRepository;

    public MotorcycleRentalService(MotorcycleRentalRepository motorentalrepository, MotorcycleRepository motorcyclerepository, DeliverymanRepository deliverymanrepository, RentalPriceTableRepository rentalpricetablerepository)
    {
        _motorcycleRentalRepository = motorentalrepository;
        _motorcycleRepository       = motorcyclerepository;
        _deliverymanRepository      = deliverymanrepository;
        _rentalPriceTableRepository = rentalpricetablerepository;
    }

    public async Task<Response> CreateMotorcycleRental(CreateMotorcycleRentalRequest request)
    {
        // Validação de dados da Moto verificar se a mesma está disponivel
        var motoDisponible = await _motorcycleRepository.GetMotorcycleByPlate(request.Plate);

        if (motoDisponible == null) return new Response(true, "Moto não encontrada.", ResponseTypeResults.NotFound);

        if (motoDisponible.InUse != 0) return new Response(true, "Moto não disponivel para locação.", ResponseTypeResults.BadRequest);

        // Validação do entregador
        var deliveryman = await _deliverymanRepository.GetDeliverymanByCNH(request.CNH);

        if (deliveryman == null) return new Response(true, "Entredador não encontrado.", ResponseTypeResults.NotFound);

        if (deliveryman.TypeCNH.ToUpper() != "A" && deliveryman.TypeCNH.ToUpper() != "A+B") return new Response(true, "Entregador não possuí uma carteira de motorista valida para a locação da moto.", ResponseTypeResults.BadRequest);

        // Verifica qual plano foi selecionado
        var plan = await _rentalPriceTableRepository.GetRentalPriceTableForDay(request.PlanOfLocation);

        if (plan == null)
        {
            var plans = await _rentalPriceTableRepository.GetRentalPricesTable();

            return new Response(true, $"Os planos disponiveis em dias são {string.Join(", ", plans.Select(s => s.Days))}, informe qualquer um desses dias, por favor.");
        }

        DateTime startDate    = DateTime.Parse(request.StartDate);
        DateTime expectedDate = startDate.AddDays(plan.Days);
        
        float total = CalcMotorcycleRental(plan, request.StartDate, request.EndDate);

        // Realiza a locação da moto a pedido do entregador
        await _motorcycleRentalRepository.CreateMotorcycleRental(new MotorcycleRental(deliveryman.Id ?? string.Empty, motoDisponible.Id ?? string.Empty, new Date(expectedDate), total, request));

        // Coloca a moto com status de uso
        motoDisponible.InUse = 1;

        await _motorcycleRepository.UpdateMotorcycle(motoDisponible);

        return new Response(false, $"Locação da Moto {motoDisponible.Model} realizada com sucesso!");
    }

    public async Task<Response> DeleteMotorcycleRental(string id)
    {
        var rental = await _motorcycleRentalRepository.GetMotorcycleRental(id);

        if (rental == null) return new Response(true, "Não existe nenhuma locação com essa identificação.", ResponseTypeResults.NotFound);

        // Libera a moto para locação
        var moto = await _motorcycleRepository.GetMotorcycle(rental.IdMotocycle);

        moto.InUse = 0;

        await _motorcycleRepository.UpdateMotorcycle(moto);

        await _motorcycleRentalRepository.DeleteMotorcycleRental(id);

        return new Response(false, $"Locação da moto {moto.Model} placa {moto.Plate} foi removida com sucesso.");
    }

    public async Task<MotorcycleRental> GetMotorcycleRental(string id)
    {
        return await _motorcycleRentalRepository.GetMotorcycleRental(id);
    }

    public async Task<List<MotorcycleRental>> GetMotorcycleRentals()
    {
        return await _motorcycleRentalRepository.GetMotorcycleRentals();
    }

    public async Task UpdateMotorcycleRental(MotorcycleRental entity)
    {
        await _motorcycleRentalRepository.UpdateMotorcycleRental(entity);
    }

    public async Task<Response> ConsultValueForMotorcycleRental(ConsultValueForMotorcycleRentalRequest request)
    {
        var plan = await _rentalPriceTableRepository.GetRentalPriceTableForDay(request.PlanOfDays);

        if (plan == null)
        {
            var plans = await _rentalPriceTableRepository.GetRentalPricesTable();

            return new Response(true, $"Os planos disponiveis em dias são {string.Join(", ", plans.Select(s => s.Days))}, informe qualquer um desses dias, por favor.");
        }

        float total = CalcMotorcycleRental(plan, request.StartDate, request.EndDate);

        return new Response(false, $"O total a ser pago é {total.ToString("C", CultureInfo.GetCultureInfo("pt-BR"))}");
    }

    private float CalcMotorcycleRental(RentalPriceTable plan, string start, string end)
    {
        DateTime startDate    = DateTime.Parse(start);
        DateTime expectedDate = startDate.AddDays(plan.Days);
        DateTime endDate      = DateTime.Parse(end);

        float price = plan.Price;
        float total = 0;

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
        else total = price;

        return total;
    }
}