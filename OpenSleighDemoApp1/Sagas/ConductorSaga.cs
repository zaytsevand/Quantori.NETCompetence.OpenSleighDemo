using OpenSleigh.Core;
using OpenSleigh.Core.Messaging;
using OpenSleighDemoApp1.Messages.Commands;
using OpenSleighDemoApp1.Messages.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OpenSleighDemoApp1.Sagas
{
    public class ConductorSaga: ConsoleLoggingSaga<ConductorState>
        , IStartedBy<DoSomething>
        , IHandleMessage<CommandExecuted<DoSomeWork>>
        , IHandleMessage<CommandExecuted<DoMoreWork>>
        , IHandleMessage<RollbackExecuted<DoSomeWork>>
        , IHandleMessage<RollbackExecuted<DoMoreWork>>
    {
        public ConductorSaga(ConductorState state) : base(state)
        {
        }

        public Task HandleAsync(IMessageContext<DoSomething> context, CancellationToken cancellationToken = new())
        {
            WriteLine($"Received a message: {context.Message.GetType().Name}. Appending it to the State.");

            Publish(new DoSomeWork(context.Message.CorrelationId, Guid.NewGuid()));

            return Task.Delay(Program.Delay, cancellationToken);
        }

        public Task HandleAsync(IMessageContext<CommandExecuted<DoSomeWork>> context, CancellationToken cancellationToken = new())
        {
            WriteLine($"Received a message: {context.Message.GetType().Name} Appending it to the State.");

            State.Append(context.Message);

            Publish(new DoMoreWork(context.Message.CorrelationId, Guid.NewGuid()));

            return Task.Delay(Program.Delay, cancellationToken);
        }

        public Task HandleAsync(IMessageContext<CommandExecuted<DoMoreWork>> context, CancellationToken cancellationToken = new())
        {
            WriteLine($"Received a message: {context.Message.GetType().Name} Appending it to the State.");

            State.Append(context.Message);
            State.MarkAsCompleted();

            return Task.Delay(Program.Delay, cancellationToken);
        }

        public Task HandleAsync(IMessageContext<RollbackExecuted<DoSomeWork>> context, CancellationToken cancellationToken = new())
        {
            WriteLine($"Received a message: {context.Message.GetType().Name} Executing full rollback.");

            return RollbackTransaction();
        }

        public Task HandleAsync(IMessageContext<RollbackExecuted<DoMoreWork>> context, CancellationToken cancellationToken = new())
        {
            WriteLine($"Received a message: {context.Message.GetType().Name} Executing full rollback.");

            return RollbackTransaction();
        }

        private Task RollbackTransaction()
        {
            while (State.TryPop(out var @event))
            {
                switch (@event)
                {
                    case CommandExecuted<DoSomeWork> cmd:
                        Publish(new Rollback<DoSomeWork>(cmd.Command));
                        break;
                    case CommandExecuted<DoMoreWork> cmd:
                        Publish(new Rollback<DoMoreWork>(cmd.Command));
                        break;
                }
            }

            return Task.Delay(Program.Delay);
        }
    }
}
