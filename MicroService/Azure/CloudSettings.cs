namespace MicroService.Azure
{
    public class CloudSettings
    {
        #region Properties
        /// <summary>
        /// Blob storage account name
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// Blob storage account key
        /// </summary>
        public string AccountKey { get; set; }
        #endregion
    }
}
