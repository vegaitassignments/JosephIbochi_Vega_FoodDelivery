namespace FoodDelivery.Middleware.GlobalExceptionMiddleware;

public abstract class FoodDeliveryExceptions : Exception
{
    public FoodDeliveryExceptions(string message): base (message){}
}

public class FoodDeliveryNotFoundException : FoodDeliveryExceptions
{
    public FoodDeliveryNotFoundException(string message) : base(message){}
}

public class FoodDeliveryBadRequestException : FoodDeliveryExceptions
{
    public FoodDeliveryBadRequestException(string message) : base(message){}
}

public class FoodDeliveryUnAuthorizedException : FoodDeliveryExceptions
{
    public FoodDeliveryUnAuthorizedException(string message) : base(message){}
}

public class FoodDeliveryForbiddenException : FoodDeliveryExceptions
{
    public FoodDeliveryForbiddenException(string message) : base(message){}
}

public class FoodDeliveryServiceNotFound : FoodDeliveryExceptions
{
    public FoodDeliveryServiceNotFound(string message) : base(message){}
}
