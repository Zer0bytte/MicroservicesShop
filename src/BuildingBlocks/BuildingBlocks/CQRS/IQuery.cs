namespace BuildingBlocks.CQRS;
public interface IQuery<out TResponse> : ICommand<TResponse> 
    where TResponse : notnull
{
}
