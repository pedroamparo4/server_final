using Microsoft.WindowsAzure.Storage.Queue;
using MicroService.Azure.Concrete;
using Newtonsoft.Json;

namespace MicroService.Threadpool
{
    public class AnalyzerJob
    {
        public int PhotoId { get; set; }
        public string UserId { get; set; }
        public AzureStorageManager StorageManager { get; set; }
        public CloudQueueMessage Message { get; set; }
    }
     
    public class AnalyzerJobFactory
    {
        public static AnalyzerJob Create(AzureStorageManager storageManager, ref CloudQueueMessage message)
        {
            /*
             * TODO: RR: A custom converter might be necessary.
             */
            var job = JsonConvert.DeserializeObject<AnalyzerJob>(message.AsString);

            // Set the storage manager
            job.StorageManager = storageManager;

            // Set the ref to the message so it can be deleted.
            job.Message = message;

            return job;
        }
    }
}
