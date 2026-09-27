namespace Inventory.Application.Abstractions;

public interface IInventoryOutboxStore
{
    void Add<TMessage>(TMessage message) where TMessage : class;
}