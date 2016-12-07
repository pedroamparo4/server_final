using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Queue;
using MicroService.Azure;
using MicroService.Azure.Concrete;
using MicroService.Threadpool;
using System.IO;

namespace MicroService
{
    class Program
    {
        private static CancellationTokenSource _cts;
        private static AzureStorageManager _azureStorageManager;
        private static WorkerManager _workerManager;

        static void Initialize()
        {
            var settings = new CloudSettings
            {
                //AccountName = ConfigurationManager.AppSettings["AccountName"],
                //AccountKey = ConfigurationManager.AppSettings["AccountKey"]
                AccountName = "photofind",
                AccountKey = "roeESiicfnfPh9UhTNU9fSGLQZPYSMyVebh28poOjChhqMn+gbnLO+dGpPhDxJfFg2wv7ajwjGBSNlKkM2p2ug=="
            };

            _azureStorageManager = new AzureStorageManager(settings);
            _workerManager = new WorkerManager();
            _cts = new CancellationTokenSource();
        }        

        static void Run()
        {
            // 
            Initialize();
            
            var task = new Task(ProcessQueue, _cts.Token);

            Console.WriteLine("Press Escape to quit...");

            task.Start();

            while (!task.IsCompleted)
            {
                var keyInput = Console.ReadKey(true);

                if (keyInput.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("Escape was pressed, cancelling...");
                    Stop();
                }
            }
        }

        private static void Stop()
        {
            // Stop the working jobs
            _workerManager.Running = false;

            foreach (Worker worker in _workerManager.Workers)
            {
                worker.Thread.Abort();
            }

            //
            _cts.Cancel();
        }

        static async void ProcessQueue()
        {
            while (!_cts.IsCancellationRequested)
            {
                // Async dequeue the message
                CloudQueueMessage retrievedMessage = await _azureStorageManager.PhotoAnalyzerQueue.GetMessageAsync();
                
                if (retrievedMessage != null)
                {
                    // Create a new job
                    _workerManager.Queue.Add(AnalyzerJobFactory.Create(_azureStorageManager, ref retrievedMessage));
                }
            }
        }

        static void Main(string[] args)
        {
            //
            if (File.Exists("apples.jpg"))
            {
                Console.WriteLine("El archivo existe.");
            }
            else { Console.WriteLine("No existe"); }

            Run();

            Stop();
        }

    }
}
