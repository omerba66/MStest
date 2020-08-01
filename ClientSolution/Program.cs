using System;
using System.Collections.Concurrent;
using System.Linq;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;

namespace ClientSolution
{
    class Program
    {
        private static ConcurrentBag<ClientRequestExecutor> Bag;
        static volatile bool exit = false;

        static async Task Main(string[] args)
        {
            Thread.Sleep(5000);

            Console.WriteLine("How many clients do you want ? \n");
            var userInput = Console.ReadLine();

            int.TryParse(userInput, out var numberOfClients);

            Bag = new ConcurrentBag<ClientRequestExecutor>();

            for (var i = 0; i < numberOfClients - 1; i++)
            {
                Bag.Add(new ClientRequestExecutor(i + 1));
            }
            
            // Console.WriteLine("Sending requests, press q to stop the program");
            //
            // await Task.Factory.StartNew(() =>
            // {
            //     while (Console.ReadKey().Key != ConsoleKey.Q) ;
            //     exit = true;
            // });
            //
            // while (!exit)
            // {
            //      await Task.WhenAll(Bag.Select(x => x.Execute()).ToArray());
            // }

            Task.WaitAll(Bag.Select(x => x.Execute()).ToArray());

            Console.WriteLine("DONE!!!!");
            Console.ReadKey();
        }
        public async Task PeriodicFooAsync(TimeSpan interval, CancellationToken cancellationToken)
        {
            while (true)
            {
                // await FooAsync();
                await Task.Delay(interval, cancellationToken);
            }
        }

    }
}
