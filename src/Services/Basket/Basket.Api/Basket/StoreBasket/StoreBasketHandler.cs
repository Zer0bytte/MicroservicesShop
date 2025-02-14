
namespace Basket.Api.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
public record StoreBasketResult(string Username);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(s => s.Cart).NotNull().WithMessage("Cart can't be null");
        RuleFor(s => s.Cart.UserName).NotEmpty().WithMessage("Username is required");
    }
}

public class StoreBasketCommandHandler(IBasketRepository repositroy)
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        ShoppingCart cart = command.Cart;
        await repositroy.StoreBasket(cart, cancellationToken);

        return new StoreBasketResult(command.Cart.UserName);
    }
}
