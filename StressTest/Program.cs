using System;
using System.Threading.Tasks;
using System.Net.Http;

using NBomber.CSharp;
using NBomber.Http.CSharp;

namespace StressTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using var httpClient = new HttpClient();

            var scenario = Scenario.Create("http_scenario", async context =>
            {
                var request =
                    Http.CreateRequest("GET", "http://localhost:5097/Restaurant/A6B7B54F-4019-47D4-A067-4D170DC2DFE8")
                        .WithHeader("Accept", "text/plain");

                var response = await Http.Send(httpClient, request);

                return response;
            })
            .WithoutWarmUp()
            .WithLoadSimulations(
                Simulation.Inject(rate: 1000, 
                                  interval: TimeSpan.FromSeconds(1),
                                  during: TimeSpan.FromSeconds(60))
            );

            NBomberRunner
                .RegisterScenarios(scenario)
                .Run();

            Console.ReadLine();
            }
    }
}
