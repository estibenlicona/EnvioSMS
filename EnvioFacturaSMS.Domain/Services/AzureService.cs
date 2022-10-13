using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using EnvioFacturaSMS.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace EnvioFacturaSMS.Domain.Services
{
    public class AzureService : IAzureService
    {
        private readonly IConfiguration _configuration;
        private readonly BlobContainerClient ContainerClient;

        public AzureService(IConfiguration configuration)
        {
            _configuration = configuration;

            string ConnectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE") ?? _configuration.GetValue<string>("AppConfigurations:AzureStorage:ConnectionString");
            string Container = Environment.GetEnvironmentVariable("AZURE_CONTAINER") ?? _configuration.GetValue<string>("AppConfigurations:AzureStorage:Container");
            ContainerClient = new BlobContainerClient(ConnectionString, Container);
        }

        public string Upload(byte[] BytesDocument, string name)
        {
            MemoryStream FileStram = new MemoryStream(BytesDocument);
            BlobClient blobClient = ContainerClient.GetBlobClient($"/abonos/{name}.pdf");
            BlobHttpHeaders blobHeaders = new BlobHttpHeaders() { ContentType = "application/pdf" };
            blobClient.Upload(FileStram, blobHeaders);
            //FileStram.Dispose();
            return blobClient.Uri.ToString();
        }


    }
}
