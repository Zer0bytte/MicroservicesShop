namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductRequest(Guid Id, string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price);
public record UpdateProductResponse(bool IsSuccess);
public class UpdateProductEndpoint() : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products", async (UpdateProductRequest request, ISender sender) =>
        {
            var result = await sender.Send(request.Adapt<UpdateProductCommand>());
            var response = result.Adapt<UpdateProductResponse>();

            return Results.Ok(response);

        })
        .WithName("UpdateProduct")
        .Produces<UpdateProductEndpoint>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Update Product")
        .WithDescription("Update Product");
    }
}
