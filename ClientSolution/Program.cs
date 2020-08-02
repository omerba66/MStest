using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClientSolution
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Thread.Sleep(5000);

            Console.WriteLine("How many clients do you want ? \n");
            var userInput = Console.ReadLine();

            int.TryParse(userInput, out var numberOfClients);

            if (numberOfClients == 0)
            {
                Console.WriteLine("invalid input");
            }
            else
            {
                await new ProgramRunner(numberOfClients).Run();
            }

        }

    }
}
