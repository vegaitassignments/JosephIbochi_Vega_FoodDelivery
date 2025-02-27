namespace FoodDelivery.Authentication.Feauture.ForgotPassword;

public class Command : IRequest<BaseResponse>
{
    public UpdatePasswordDTO requestData { get; set; }
}

public class CommandHandler : IRequestHandler<Command, BaseResponse>
{
    public Task<BaseResponse> Handle(Command request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}