
namespace FoodDelivery.Authentication.Feauture.ResetPassword;

public class Command : IRequest<BaseResponse>
{
    public ResetPasswordDTO requestData { get; set; }
}

public class CommandHandler : IRequestHandler<Command, BaseResponse>
{
    public Task<BaseResponse> Handle(Command request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
