using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using MicroService.Helpers;

namespace MicroService.Threadpool
{
    public class WorkerManager
    {
        public WorkerManager()
        {
            Workers = new List<Worker>();
            WorkersHistory = new List<Worker>();
            Queue = new List<AnalyzerJob>();
            Running = true;
            MaxWorkers = 10;

            var timer = new Thread(() =>
            {
                while (Running)
                {
                    CheckForJobs();
                    Thread.Sleep(500);
                }
            });

            timer.Start();
        }

        private void CheckForJobs()
        {
            try
            {
                var availableWorkers = MaxWorkers - Workers.Count;
                var jobs = Queue.Take(availableWorkers).ToList();
                foreach (var job in jobs)
                {
                    lock (Queue)
                    {
                        Queue.Remove(job);
                        CreateWorker(job);
                    }
                }
            }
            catch { }
        }

        private void CreateWorker(AnalyzerJob analyzerJob)
        {
            // Create new worker
            var worker = new Worker
            {
                AnalyzerJob = analyzerJob,
            };

            // Add worker to list
            lock (Workers)
            {
                Workers.Add(worker);
            }

            try
            {
                worker.Thread = new Thread(async () =>
                {
                    // start
                    worker.Start = SystemTime.Now();
                    try
                    {                        
                        // TODO: RR: Retrieve the image from the db
                        string path = @"C:\Users\GeorgeTamate\Desktop\peach.png";
                        string base64string = Convert.ToBase64String(File.ReadAllBytes(path));
                        // Transform data to byte array
                        Byte[] bytes = Convert.FromBase64String(base64string);
                        // Extract data from image using the Vision API
                        // TODO: RR: Add your subscription Id
                        // TODO: RR: Abstract this to its own class/function

                        // KEY1 aaaba126ba6a4737b1ef3aee03cca16d
                        // KEY2 a4d9d861fe6f478b96059bfd424fbe0d
                        // string subscriptionKey = "<YOUR Vision API Id goes here>";
                        string subscriptionKey = "aaaba126ba6a4737b1ef3aee03cca16d";
                        VisionServiceClient visionServiceClient = new VisionServiceClient(subscriptionKey);
                        // TODO: RR: Simplified list of features, please read the docs to decide which features to add.
                        VisualFeature[] visualFeatures =
                        {
                            VisualFeature.Description,
                            VisualFeature.ImageType,
                            VisualFeature.Tags
                        };
                        using (Stream stream = new MemoryStream(bytes))
                        {
                            // Retrieve the data.
                            AnalysisResult analysisResult = 
                                await visionServiceClient.AnalyzeImageAsync(stream, visualFeatures);
                            // TODO: RR: Manipulate & store the retrieved data.
                            var results = analysisResult.Tags.Select(tag => tag.Name).ToList();
                            Console.WriteLine("==== RESULTS ====");
                            Console.WriteLine(string.Join(",", results));
                            Console.WriteLine(Environment.NewLine);
                        }

                        // Finish

                        // Async delete the message
                        await worker.AnalyzerJob
                                    .StorageManager
                                    .PhotoAnalyzerQueue
                                    .DeleteMessageAsync(worker.AnalyzerJob.Message);
                        worker.Status = JobStatus.Success;
                        worker.Finish = SystemTime.Now();
                        ArchiveWorker(worker);
                        Console.WriteLine("checkpoint 10");
                    }
                    catch (ThreadAbortException)
                    {
                        worker.Status = JobStatus.Cancelled;
                        worker.Finish = SystemTime.Now();
                        ArchiveWorker(worker);
                    }
                    catch (Exception e)
                    {
                        worker.Status = JobStatus.Error;
                        worker.ErrorMessage = $"{e}";
                        worker.Finish = SystemTime.Now();
                        ArchiveWorker(worker);
                    }
                });

                // Start the worker
                worker.Thread.Start();
            }
            catch (Exception e)
            {
                worker.Status = JobStatus.Error;
                worker.ErrorMessage = $"{e}";
                ArchiveWorker(worker);
            }
        }
        private void ArchiveWorker(Worker worker)
        {
            worker.Thread = null;

            lock (Workers)
            {
                Workers.Remove(worker);
            }

            lock (WorkersHistory)
            {
                WorkersHistory.Add(worker);
            }
        }

        #region Properties
        public List<AnalyzerJob> Queue { get; }
        public List<Worker> Workers { get; }
        public List<Worker> WorkersHistory { get; }
        public int MaxWorkers { get; set; }
        public bool Running { get; set; }
        #endregion
    }
}
