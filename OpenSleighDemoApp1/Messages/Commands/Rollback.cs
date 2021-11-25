using OpenSleigh.Core.Messaging;
using System;

namespace OpenSleighDemoApp1.Messages.Commands
{
    public record Rollback<TCommand> : ICommand where TCommand: ICommand
    {
        public Rollback(TCommand command)
        {
            CorrelationId = command.CorrelationId;
            Command = command;
            Id = Guid.NewGuid();
        }

        public Guid CorrelationId { get; init; }
        public TCommand Command { get; init; }
        public Guid Id { get; init; }
    }
}
