
namespace FoodDelivery.Features.Food.RateFood;

public class Command : IRequest<BaseResponse>
{
    
}

public class CommandHandler : IRequestHandler<Command, BaseResponse>
{
    public Task<BaseResponse> Handle(Command request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
