namespace Infra.BlobStorage
{
    public interface IAzureBlobStorage
    {
        Task<string> UploadFileToBlob(string stringInBase64, string extension = "", string root = "Careme");
    }
}