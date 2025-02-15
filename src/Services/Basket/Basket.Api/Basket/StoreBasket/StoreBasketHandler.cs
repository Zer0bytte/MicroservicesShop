
using Discount.Grpc;

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

public class StoreBasketCommandHandler(IBasketRepository repositroy,
    DiscountProtoService.DiscountProtoServiceClient discountProto)
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        await ProcessDiscounts(command.Cart, cancellationToken);
        await repositroy.StoreBasket(command.Cart, cancellationToken);

        return new StoreBasketResult(command.Cart.UserName);


    }
    private async Task ProcessDiscounts(ShoppingCart cart, CancellationToken cancellationToken)
    {
        foreach (var item in cart.Items)
        {
            var coupon = await discountProto.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName });
            item.Price -= coupon.Amount;
        }
    }
}
