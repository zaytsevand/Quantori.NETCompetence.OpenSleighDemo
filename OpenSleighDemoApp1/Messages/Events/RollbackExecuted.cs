using OpenSleigh.Core.Messaging;
using System;

namespace OpenSleighDemoApp1.Messages.Events
{
    public record RollbackExecuted<TCommand>(TCommand Command, object State, Exception Exception = null)
        : EventBase(Guid.NewGuid(), Command.CorrelationId, Command, State) where TCommand : ICommand;
}
