using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BachelorThesis.Business.DataModels;
using Microsoft.AspNetCore.SignalR;

namespace BachelorThesis.Server
{
    public class SimulationHub : Hub<ISimulationHubClient>
    {

        public async Task SendStart()
        {
           await SimulationTimer.Instance.Start();
        }

        public void SendStop()
        {
            SimulationTimer.Instance.Stop();
        }

        //public async Task SendDefindition(ProcessKind kind)
        //{
        //    await Clients.All.SendEvent("msg");
        //}
    }

    // Client side
    public interface ISimulationHubClient
    {
        Task NotifyEvent(CompletionChangedTransactionEvent transactionEvent);
        Task NotifyStart(ProcessKind processKind);
        Task NotifySimulationEnd();
    }
}
