
namespace FoodDelivery.Authentication.Feauture.Register;

public class Command : IRequest<BaseResponse>
{
    public RegisterDTO requestData { get; set; }
}

public class CommandHandler : IRequestHandler<Command, BaseResponse>
{
    public Task<BaseResponse> Handle(Command request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
