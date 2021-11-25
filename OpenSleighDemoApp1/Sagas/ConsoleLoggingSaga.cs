using OpenSleigh.Core;
using System;

namespace OpenSleighDemoApp1.Sagas
{
    public abstract class ConsoleLoggingSaga<TState>: Saga<TState> where TState : SagaState
    {
        protected ConsoleLoggingSaga(TState state) : base(state)
        {
        }

        protected void WriteLine(string message) => Console.WriteLine($"{DateTime.Now.TimeOfDay:hh\\:mm\\:ss}: {GetType().Name}: {message}");
    }
}