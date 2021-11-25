using OpenSleigh.Core;
using OpenSleigh.Core.Messaging;
using OpenSleighDemoApp1.Messages.Commands;
using OpenSleighDemoApp1.Messages.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OpenSleighDemoApp1.Sagas
{
    public class FailingWorkerSaga
        : ConsoleLoggingSaga<WorkerState>
            , IStartedBy<DoMoreWork>
            , ICompensateMessage<DoMoreWork>
            , IHandleMessage<Rollback<DoMoreWork>>
    {
        public FailingWorkerSaga(WorkerState state) : base(state)
        {
        }

        public Task HandleAsync(IMessageContext<DoMoreWork> context, CancellationToken cancellationToken = new())
        {
            WriteLine($"Received a message: {context.Message.GetType().Name} Preparing to fail!");

            return Task.FromException(new Exception("It was my destiny to fail. Blame the creator."));
        }

        public Task HandleAsync(IMessageContext<Rollback<DoMoreWork>> context, CancellationToken cancellationToken = new())
        {
            WriteLine($"Received a message: {context.Message.GetType().Name} Executing rollback...");

            Publish(new RollbackExecuted<Rollback<DoMoreWork>>(context.Message, null));

            return Task.Delay(Program.Delay, cancellationToken);
        }

        public Task CompensateAsync(ICompensationContext<DoMoreWork> context, CancellationToken cancellationToken = new())
        {
            WriteLine($"Received a message: {context.MessageContext.Message}. Executing compensation...");

            Publish(new RollbackExecuted<DoMoreWork>(context.MessageContext.Message, new {}, context.Exception));

            return Task.Delay(Program.Delay, cancellationToken);
        }
    }
}
