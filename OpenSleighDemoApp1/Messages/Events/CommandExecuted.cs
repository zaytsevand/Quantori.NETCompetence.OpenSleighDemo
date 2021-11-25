using OpenSleigh.Core.Messaging;
using System;

namespace OpenSleighDemoApp1.Messages.Events
{
    public record CommandExecuted<TCommand>(TCommand Command, object State)
        : EventBase(Guid.NewGuid(), Command.CorrelationId, Command, State) where TCommand : ICommand;
}
