using MediatR;

namespace BuildingBlocks.Application.CQRS;

public interface ICommand : IRequest
{
    
}

public interface ICommand<out TResponse> : IRequest<TResponse>
{
    
}
