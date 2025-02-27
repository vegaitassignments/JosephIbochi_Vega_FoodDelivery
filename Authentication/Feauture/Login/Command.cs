namespace FoodDelivery.Authentication.Feauture.Login;

public class Command : IRequest<BaseResponse>
{
    public LoginDTO requestData { get; set; }
}

public class CommandHandler : IRequestHandler<Command, BaseResponse>
{
    public Task<BaseResponse> Handle(Command request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}