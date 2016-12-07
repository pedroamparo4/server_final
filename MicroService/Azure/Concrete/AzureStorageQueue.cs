using Microsoft.WindowsAzure.Storage.Queue;

namespace MicroService.Azure.Concrete
{
    public class AzureStorageQueue : AzureQueueBase
    {
        private readonly CloudQueueClient _queueClient;
        public AzureStorageQueue(string connectionString)
            : base(connectionString)
        {
            _queueClient = CloudStorageAccount.CreateCloudQueueClient();
        }

        public CloudQueue GetQueue(string queueName)
        {
            // Retrieve a reference to a container. 
            //
            CloudQueue queue = _queueClient.GetQueueReference(queueName);
            
            //
            queue.CreateIfNotExists();

            return queue;
        }
    }
}
