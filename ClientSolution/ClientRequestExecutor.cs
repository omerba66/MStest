using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ClientSolution
{
    public class ClientRequestExecutor
    {
        public HttpClient HttpClient { get; set; }
        public int Id { get; set; }

        public ClientRequestExecutor(int numberOfClients)
        {
            Id = GenerateIdFromClientsRange(numberOfClients);
            HttpClient = new HttpClient {BaseAddress = new Uri("http://localhost:53696")};
        }

        public async Task<HttpResponseMessage> Execute()
        {
            while (true)
            { 
                var result =await HttpClient.GetAsync($"/api/values/{Id}");
                Console.WriteLine($"Client id : {Id}\n{result.Headers}");
                await Task.Delay(GenerateRandomTimeSpan(), CancellationToken.None);
            }
        }

        private static TimeSpan GenerateRandomTimeSpan()
        {
            return new TimeSpan(0, 0, 0, new Random().Next(1,5));
        }
        private static int GenerateIdFromClientsRange(int numberOfClients)
        {
            return new Random().Next(1,numberOfClients+1);
        }

    }
}