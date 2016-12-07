using Microsoft.WindowsAzure.Storage.Queue;

namespace MicroService.Azure.Concrete
{
    public class AzureStorageManager
    {
        public AzureStorageManager(CloudSettings cloudInfo) :
            this(cloudInfo.AccountName, cloudInfo.AccountKey)
        {
        }

        public AzureStorageManager(string accountName, string accountKey)
        {
            AccountName = accountName;
            AccountKey = accountKey;

            // Build conn string
            //
#if DEBUG
            /* 
             * TODO: RR: When using the Azure SDK's storage emulator
             * If not then remove the whole clause and leave Release setting.
             */
            // ConnectionString = "UseDevelopmentStorage=true";
            ConnectionString = 
                $"DefaultEndpointsProtocol=https;AccountName={AccountName};AccountKey={AccountKey}";
#else
            ConnectionString = 
                $"DefaultEndpointsProtocol=https;AccountName={AccountName};AccountKey={AccountKey}";
#endif
            var azureStorageQueue = new AzureStorageQueue(ConnectionString);

            // Get the PhotoAnalyzerQueue queue
            PhotoAnalyzerQueue = azureStorageQueue.GetQueue(
                                ResourceName.PhotoAnalyzerQueue);
        }

        #region Properties
        public string AccountName { get; private set; }
        public string AccountKey { get; private set; }
        public string ConnectionString { get; private set; }
        public CloudQueue PhotoAnalyzerQueue { get; }
        #endregion
    }
}
