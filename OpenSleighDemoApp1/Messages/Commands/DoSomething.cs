using OpenSleigh.Core.Messaging;
using System;

namespace OpenSleighDemoApp1.Messages.Commands
{
    public record DoSomething(Guid CorrelationId, Guid Id) : ICommand;
}
