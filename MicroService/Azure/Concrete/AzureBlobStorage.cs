using Microsoft.WindowsAzure.Storage.Blob;

namespace MicroService.Azure.Concrete
{
    public class AzureBlobStorage : AzureStorageBase
    {
        private readonly CloudBlobClient _cloudBlobClient;

        public AzureBlobStorage(string connectionString)
            : base(connectionString)
        {
            _cloudBlobClient = CloudStorageAccount.CreateCloudBlobClient();
        }

        public CloudBlobContainer GetContainer(string containerName)
        {
            // Retrieve a reference to a container. 
            //
            var container = _cloudBlobClient.GetContainerReference(containerName);

            // Create the container if it doesn't already exist.
            //
            container.CreateIfNotExists();

            // Setting the contents of the container public.
            // TODO: RR: Pass this as a param
            container.SetPermissions(
                new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });

            return container;
        }
    }
}
