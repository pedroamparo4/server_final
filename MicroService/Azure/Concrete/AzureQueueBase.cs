﻿using Microsoft.WindowsAzure.Storage;
using MicroService.Azure.Abstract;

namespace MicroService.Azure.Concrete
{
    public class AzureQueueBase : IQueueBase
    {
        public AzureQueueBase(string connectionString)
        {
            CloudStorageAccount = CloudStorageAccount.Parse(connectionString);
        }

        #region Properties
        public CloudStorageAccount CloudStorageAccount { get; }
        #endregion
    }
}
