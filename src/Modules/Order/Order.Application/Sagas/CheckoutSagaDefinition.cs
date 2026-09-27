using MassTransit;

namespace Order.Application.Sagas;

public class CheckoutSagaDefinition : SagaDefinition<CheckoutSagaState>
{
    public CheckoutSagaDefinition()
    {
        EndpointName = "checkout-saga-state";
    }

    protected override void ConfigureSaga(
        IReceiveEndpointConfigurator endpointConfigurator,
        ISagaConfigurator<CheckoutSagaState> sagaConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(r => 
            r.Interval(5, TimeSpan.FromMilliseconds(200)));

        var partitioner = endpointConfigurator.CreatePartitioner(32);

        sagaConfigurator.Message<OrderCreatedIntegrationEvent>(m => 
            m.UsePartitioner(partitioner, p => p.Message.OrderId));
        sagaConfigurator.Message<InventoryReservedIntegrationEvent>(m => 
            m.UsePartitioner(partitioner, p => p.Message.OrderId));
        sagaConfigurator.Message<InventoryReservationFailedIntegrationEvent>(m => 
            m.UsePartitioner(partitioner, p => p.Message.OrderId));
        sagaConfigurator.Message<InventoryReleasedIntegrationEvent>(m => 
            m.UsePartitioner(partitioner, p => p.Message.OrderId));
        sagaConfigurator.Message<PaymentCreatedIntegrationEvent>(m => 
            m.UsePartitioner(partitioner, p => p.Message.OrderId));
        sagaConfigurator.Message<PaymentCreationFailedIntegrationEvent>(m => 
            m.UsePartitioner(partitioner, p => p.Message.OrderId));
        sagaConfigurator.Message<PaymentSucceededIntegrationEvent>(m => 
            m.UsePartitioner(partitioner, p => p.Message.OrderId));
        sagaConfigurator.Message<PaymentFailedIntegrationEvent>(m => 
            m.UsePartitioner(partitioner, p => p.Message.OrderId));
    }
}