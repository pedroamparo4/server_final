using Microsoft.WindowsAzure.Storage.Blob;

namespace MicroService.Azure.Abstract
{
    public interface IAzureBlobStorage
    {
        CloudBlobContainer GetContainer(string containerName);
    }
}
