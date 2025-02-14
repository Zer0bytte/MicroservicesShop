
namespace Catalog.API.Products.CreateProduct;

public class ProductCreatedEvent : INotification
{
}
public class ProductCreatedEventHandler : INotificationHandler<ProductCreatedEvent>
{
    public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        await Task.Delay(1);
    }
}