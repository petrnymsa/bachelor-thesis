using System;
using System.Threading;
using System.Threading.Tasks;
using BachelorThesis.Business.DataModels;
using Microsoft.AspNetCore.SignalR.Client;

namespace BachelorThesis.Server.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {

            var cancellationTokenSource = new CancellationTokenSource();

            Task.Run(() => MainAsync(cancellationTokenSource.Token).GetAwaiter().GetResult(), cancellationTokenSource.Token);

            ConsoleKeyInfo key;
            while ((key = Console.ReadKey(true)).Key != ConsoleKey.Escape)
            {

            }


            cancellationTokenSource.Cancel();
        }

        private static async Task MainAsync(CancellationToken cancellationToken)
        {
            var hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:60105/simulation")
                .Build();

            hubConnection.On<ProcessKind>("SendEvent", (processKind) =>
            {
                Console.WriteLine(processKind);

            });
            await hubConnection.StartAsync();

            //Console.WriteLine("Press key to continue");
            //Console.ReadKey();

            await hubConnection.SendAsync("NotifyStart");

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(250, cancellationToken);
            }

            await hubConnection.DisposeAsync();
        }
    }
}
