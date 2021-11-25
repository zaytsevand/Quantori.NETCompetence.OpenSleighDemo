using OpenSleigh.Core;
using OpenSleighDemoApp1.Messages.Events;
using System;
using System.Collections.Generic;

namespace OpenSleighDemoApp1.Sagas
{
    public class ConductorState : SagaState
    {
        private readonly Stack<EventBase> _executionHistory = new();
        private readonly object _lock = new {};

        public ConductorState(Guid id) : base(id)
        {
        }

        public void Append(EventBase stepResult)
        {
            lock (_lock)
            {
                _executionHistory.Push(stepResult);
            }
        }

        public bool TryPop(out EventBase message)
        {
            lock (_lock)
            {
                return _executionHistory.TryPop(out message);
            }
        }
    }
}
