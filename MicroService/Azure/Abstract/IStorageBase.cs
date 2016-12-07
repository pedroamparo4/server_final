using Microsoft.WindowsAzure.Storage;

namespace MicroService.Azure.Abstract
{
    public interface IStorageBase
    {
        CloudStorageAccount CloudStorageAccount { get; }
    }
}
