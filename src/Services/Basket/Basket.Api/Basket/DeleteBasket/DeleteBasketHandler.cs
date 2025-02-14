
namespace Basket.Api.Basket.DeleteBasket;

public record DeleteBasketCommand(string Username) : ICommand<DeleteBasketResult>;

public record DeleteBasketResult(bool IsSuccess);

public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(b => b.Username).NotEmpty().WithMessage("Username is required");
    }
}
public class DeleteBasketCommandHandler(IBasketRepository repository)
    : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        await repository.DeleteBasket(command.Username, cancellationToken);

        return new DeleteBasketResult(true);
    }
}
