using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientSolution
{
    public class ProgramRunner
    {
        private  int NumberOfClients { get; set; }
        public List<ClientRequestExecutor> RequestExecutors { get; set; } = new List<ClientRequestExecutor>();
        public ProgramRunner(int numberOfClients)
        {
            NumberOfClients = numberOfClients;
        }

        public async Task Run()
        {

            PopulateExcecutors();

            Task.WaitAll(RequestExecutors.Select(x => x.Execute()).ToArray());

            Console.WriteLine("DONE!!!!");
            Console.ReadKey();

        }

        private void PopulateExcecutors()
        {
            for (var i = 0; i < NumberOfClients - 1; i++)
            {
                RequestExecutors.Add(new ClientRequestExecutor(i + 1));
            }
        }
    }
}