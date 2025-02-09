using Marten.Linq.QueryHandlers;
using Microsoft.Extensions.Logging;

namespace Catalog.API.Products.GetProductsByCategory;

public record GetProductsByCategoryQuery(string Category) : IQuery<GetProductsByCategoryResult>;
public record GetProductsByCategoryResult(IEnumerable<Product> Products);

public class GetProductsByCategoryQueryHandler(IDocumentSession session, ILogger<GetProductsByCategoryQueryHandler> logger)
    : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductsByCategoryQuery Called With {@Query} ", query);

        var products = await session.Query<Product>()
            .Where(p => p.Category.Contains(query.Category))
            .ToListAsync();

        return new GetProductsByCategoryResult(products);
    }
}
