﻿namespace milescarrental.Application.Configuration.DomainEvents
{
    public interface IDomainEventNotification<out TEventType>
    {
        TEventType DomainEvent { get; }
    }
}