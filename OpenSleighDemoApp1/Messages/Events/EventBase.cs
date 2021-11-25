using OpenSleigh.Core.Messaging;
using System;

namespace OpenSleighDemoApp1.Messages.Events
{
    public record EventBase(Guid Id, Guid CorrelationId, IMessage ParentMessage, object State) : IEvent;
}
