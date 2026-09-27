namespace Payment.Application.Abstractions;

public interface IPaymentOutboxStore
{
    void Add<TMessage>(TMessage message) where TMessage : class;
}