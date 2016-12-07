using Microsoft.WindowsAzure.Storage;

namespace MicroService.Azure.Abstract
{
    public interface IQueueBase
    {
        CloudStorageAccount CloudStorageAccount { get; }
    }
}
