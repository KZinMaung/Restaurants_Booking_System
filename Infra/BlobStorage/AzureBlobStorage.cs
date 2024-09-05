
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.BlobStorage
{
    public class AzureBlobStorage : IAzureBlobStorage
    {
        //resource path = caremechat
        public async Task<string> UploadFileToBlob(string stringInBase64, string extension = "", string root = "caremechat")
        {
            List<string> bs64List = stringInBase64.Split(',').ToList();
            var contenttype = stringInBase64.Split(",")[0].Split(":")[1].Split(";")[0];
            if (bs64List.Count() > 0)
            {
                stringInBase64 = bs64List[1];
            }
            string guid = Guid.NewGuid().ToString();
            byte[] file = System.Convert.FromBase64String(stringInBase64);  // ByteArrayToImage(stringInBase64);
            try

            {
                //  CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                // CloudConfigurationManager.GetSetting("StorageConnectionString"));
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                "DefaultEndpointsProtocol=https;AccountName=caremestorage;AccountKey=40TpzqMVq3sym8CNxDN/EIQR/H0rVjoJysg2iWr2awcOtUwa315esbR/e3te0RG7d3gj4oB0if8akDcaXWOBrw==;EndpointSuffix=core.windows.net");

                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("caremerss");
                BlobRequestOptions opt = new BlobRequestOptions
                {
                    SingleBlobUploadThresholdInBytes = 1048576,
                    ParallelOperationThreadCount = 4
                };
                blobClient.DefaultRequestOptions = opt;
                BlobContainerPermissions permissions = new BlobContainerPermissions()
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                };
                await container.SetPermissionsAsync(permissions);

                CloudBlockBlob blockBlob = container.GetBlockBlobReference(root + "/" + guid +"."+ extension);

                blockBlob.Properties.ContentType = contenttype;
                blockBlob.Properties.CacheControl = "max-age=86400";

                await blockBlob.UploadFromByteArrayAsync(file, 0, file.Length);

                return guid+"." + extension;
            }
            catch
            {
                return null;
            }
        }
    }
}
