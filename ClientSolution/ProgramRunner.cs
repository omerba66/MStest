using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace ClientSolution
{
    public class ProgramRunner
    {
        private string AppSetting { get; }
        private  int NumberOfClients { get; }
        public List<ClientRequestExecutor> RequestExecutors { get; set; } = new List<ClientRequestExecutor>();
        public ProgramRunner(int numberOfClients)
        {
            NumberOfClients = numberOfClients;
            AppSetting = ConfigurationManager.AppSettings["Uri"];
        }

        public async Task Run()
        {

            PopulateExcecutors();
            Task.WaitAll(RequestExecutors.Select(x => x.Execute()).ToArray());
        }

        private void PopulateExcecutors()
        {
            for (var i = 0; i < NumberOfClients - 1; i++)
            {
                RequestExecutors.Add(new ClientRequestExecutor(NumberOfClients, AppSetting));
            }
        }
    }
}