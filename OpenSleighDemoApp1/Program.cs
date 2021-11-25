using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenSleigh.Core.DependencyInjection;
using OpenSleigh.Core.Messaging;
using OpenSleigh.Persistence.InMemory;
using OpenSleighDemoApp1.Messages.Commands;
using OpenSleighDemoApp1.Sagas;
using System;
using System.Threading.Tasks;

namespace OpenSleighDemoApp1
{
    internal class Program
    {
        internal static int Delay = 2000;

        private static async Task Main(string[] args)
        {
            var builder = CreateHostBuilder(args);
            var host = builder.Build();

            var bus = host.Services.GetRequiredService<IMessageBus>();

            await bus
                .PublishAsync(new DoSomething(Guid.NewGuid(), Guid.NewGuid()))
                .ConfigureAwait(false);

            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseConsoleLifetime()
                .ConfigureServices(collection =>
                    collection
                        .AddLogging(builder => builder.SetMinimumLevel(LogLevel.Error))
                        .AddOpenSleigh(configurator =>
                        {
                            _ = configurator
                                .UseInMemoryPersistence()
                                .UseInMemoryTransport();

                            _ = configurator
                                .AddSaga<ConductorSaga, ConductorState>()
                                .UseStateFactory<DoSomething>(s => new ConductorState(s.CorrelationId))
                                .UseInMemoryTransport();

                            _ = configurator
                                .AddSaga<WorkerSaga, WorkerState>()
                                .UseStateFactory<DoSomeWork>(s => new WorkerState(s.CorrelationId))
                                .UseInMemoryTransport();

                            _ = configurator
                                .AddSaga<FailingWorkerSaga, WorkerState>()
                                .UseStateFactory<DoMoreWork>(s => new WorkerState(s.CorrelationId))
                                .UseInMemoryTransport();
                        }));
        }
    }
}
