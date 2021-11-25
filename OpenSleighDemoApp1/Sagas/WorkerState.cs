using OpenSleigh.Core;
using System;

namespace OpenSleighDemoApp1.Sagas
{
    public class WorkerState : SagaState
    {
        public WorkerState(Guid id) : base(id)
        {
        }
    }
}