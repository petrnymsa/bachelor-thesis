using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BachelorThesis.Business;
using BachelorThesis.Business.DataModels;
using BachelorThesis.Business.Parsers;
using BachelorThesis.Business.Simulation;
using Microsoft.AspNetCore.SignalR;

namespace BachelorThesis.Server
{
    public class SimulationTimer
    {
        private Timer timer;
        private ProcessSimulation simulation;
        private ProcessKind processKind;

        public static SimulationTimer Instance = new SimulationTimer();

        public IHubContext<SimulationHub,ISimulationHubClient> Hub { get; set; }

        private SimulationTimer() { }

        public async Task Start()
        {
            if (timer == null)
                timer = new Timer(NotifySimulationNextStep);

           // var caseParser = new SimulationCaseParser();
            var parser = new ProcessKindXmlParser();

            var xml = await SimulationCases.LoadXmlAsync(SimulationCases.ModelDefinition);
            processKind = parser.ParseDefinition(xml);

            var caseXml = await SimulationCases.LoadXmlAsync(SimulationCases.Case01);
            simulation = new RentalContractSimulationFromXml(caseXml);
            simulation.Prepare();

            //send model definition
            await Hub.Clients.All.NotifyStart(processKind);
            timer.Change(TimeSpan.FromSeconds(0), TimeSpan.FromMilliseconds(500));
        }

        private async void NotifySimulationNextStep(object state)
        {
            if (!simulation.CanContinue)
            {
                await Hub.Clients.All.NotifySimulationEnd();
                Stop();
                return;
            }

            var events = simulation.SimulateNextChunk();

            foreach (var transactionEvent in events)
            {
                await Hub.Clients.All.NotifyEvent((CompletionChangedTransactionEvent)transactionEvent);
            }
        }

        public void Stop()
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite);
        }
      
    }
}
