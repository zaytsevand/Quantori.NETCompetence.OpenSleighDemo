using OpenSleigh.Core;
using OpenSleigh.Core.Messaging;
using OpenSleighDemoApp1.Messages.Commands;
using OpenSleighDemoApp1.Messages.Events;
using System.Threading;
using System.Threading.Tasks;

namespace OpenSleighDemoApp1.Sagas
{
    public class WorkerSaga
        : ConsoleLoggingSaga<WorkerState>
            , IStartedBy<DoSomeWork>
            , IHandleMessage<Rollback<DoSomeWork>>
    {
        public WorkerSaga(WorkerState state) : base(state)
        {
        }

        public Task HandleAsync(IMessageContext<DoSomeWork> context, CancellationToken cancellationToken = new())
        {
            WriteLine($"Received a message: {context.Message.GetType().Name} Executing normally...");

            Publish(new CommandExecuted<DoSomeWork>(context.Message, null));

            return Task.Delay(Program.Delay, cancellationToken);
        }


        public Task HandleAsync(IMessageContext<Rollback<DoSomeWork>> context, CancellationToken cancellationToken = new())
        {
            WriteLine($"Received a message: {context.Message.GetType().Name} Executing rollback...");

            Publish(new CommandExecuted<Rollback<DoSomeWork>>(context.Message, null));

            return Task.Delay(Program.Delay, cancellationToken);
        }
    }
}
