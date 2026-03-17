using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Security.Cryptography.Xml;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using UtilityNGPKG.ExternalApiIntegration;
using UtilityNGPKG.FilePost.DTOs.Cloudinary;
using UtilityNGPKG.ResponseDetail;
using ResourceType = CloudinaryDotNet.Actions.ResourceType;

namespace UtilityNGPKG.FilePost
{
    internal class FileService : IFileService
    {
        private readonly ILogger<FileService> logger;
        private readonly IApiIntegrationService integrationService;
        public FileService(ILogger<FileService> logger, IApiIntegrationService integrationService)
        {
            this.logger = logger;
            this.integrationService = integrationService;
        }
        public async Task<bool> Delete_Cloudinary(CloudinaryCredentials credentials, string publicId)
        {
            try
            {
                var deletionParams = new DeletionParams(publicId)
                {
                    ResourceType = ResourceType.Raw,
                };
                var cloudinary = new Cloudinary(new Account(credentials.CloudinaryName, credentials.CloudinaryAPIKey, credentials.CloudinaryAPISecret));
                var result = await cloudinary.DestroyAsync(deletionParams);

                return result.Result == "ok";
            }
            catch (Exception ex)
            {
                logger.LogCritical("An exception {ex} was thrown while deleting from cloudinary", ex);
                return false;
            }
        }

        public async Task<(MemoryStream? fileStream, string contentType, bool success)> Download_Cloudinary(CloudinaryCredentials credentials, string urlOrPublicId)
        {
            try
            {
                logger.LogWarning($"[CloudinaryService] DownloadAsync called with: {urlOrPublicId}");
                var cloudinary = new Cloudinary(new Account(credentials.CloudinaryName, credentials.CloudinaryAPIKey, credentials.CloudinaryAPISecret));

                string resourceUrl;

                if (urlOrPublicId.StartsWith("http://") || urlOrPublicId.StartsWith("https://"))
                {
                    resourceUrl = urlOrPublicId;
                    logger.LogWarning("[CloudinaryService] Using provided full URL.");
                }
                else
                {
                    resourceUrl = cloudinary.Api.Url
                        .ResourceType("raw")
                        .Action("upload")
                        .Transform(new Transformation().Flags("attachment"))
                        .Signed(true)
                        .Secure(true)
                        .BuildUrl(urlOrPublicId);
                    logger.LogWarning($"[CloudinaryService] Generated Signed URL: {resourceUrl}");
                }

                var response = await integrationService.GetRequest(resourceUrl);
                if (!response.IsSuccessStatusCode)
                {
                    logger.LogWarning($"Cloudinary download failed with status {response.StatusCode} for {resourceUrl}. Content: {await response.Content.ReadAsStringAsync()}");
                    return default;
                }
                var stream = await response.Content.ReadAsStreamAsync();
                var contentHeader = new FileExtensionContentTypeProvider();
                if (!contentHeader.TryGetContentType(resourceUrl, out var contentType))
                {
                    contentType = "application/octet-stream";
                }
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                return (memoryStream, contentType, true);
            }
            catch (Exception ex)
            {
                logger.LogCritical("{ex}", ex);
                return default;
            }
        }

        public async Task<(string signedUrl, bool success)> GenerateSignedUrl_Cloudinary(CloudinaryCredentials credentials, string publicId)
        {
            try
            {
                var cloudinary = new Cloudinary(new Account(
                    credentials.CloudinaryName,
                    credentials.CloudinaryAPIKey,
                    credentials.CloudinaryAPISecret));

                if (string.IsNullOrWhiteSpace(publicId))
                    throw new ArgumentException("publicId cannot be null or empty");

                var signedUrl = cloudinary.Api.Url
                    .ResourceType("raw")
                    .Action("upload")
                    .Transform(new Transformation().Flags("attachment"))
                    .Signed(true)
                    .Secure(true)
                    .BuildUrl(publicId);

                return (signedUrl, true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Failed to generate signed URL for publicId: {publicId}");
                return (" ", true); ;
            }
        }

        public async Task<UploadResponse> UploadDocument_Cloudinary(UploadRequestBody requestBody, IFormFile file, CloudinaryCredentials credentials)
        {
            var result = new UploadResponse();
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            result.FileExtension = fileExtension;
            try
            {
                long limit = requestBody.FileLimit * 1024 * 1024;
                var fileSizeExceedLimit = file.Length > limit;
                if (fileSizeExceedLimit)
                {
                    result.Error = $"Document exceeds limit of {limit}";
                    return result;
                }

                var fileName = file.FileName.Trim();

                var fileFormatIsAcceptable = requestBody.AllowedExtensionTypes.Contains(fileExtension) &&
                                             requestBody.AllowedMimeTypes.Contains(file.ContentType);
                if (!fileFormatIsAcceptable)
                {
                    result.Error = $"File format {fileExtension} or mime type {file.ContentType} is not supported. " +
                                                                    $"Allowed Extensions are: {string.Join(", ", requestBody.AllowedExtensionTypes)}..." +
                                                                    $"\n Allowed mime types are: {string.Join(", ", requestBody.AllowedMimeTypes)}";
                    return result;
                }
                var cloudinary = new Cloudinary(new Account(credentials.CloudinaryName, credentials.CloudinaryAPIKey, credentials.CloudinaryAPISecret));

                var documentFolder = $"{requestBody.CloudinaryBaseFolder}/{requestBody.UniqueIdentifier}/{requestBody.DirectoryName}";

                var uploadParams = new RawUploadParams
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream()),
                    Folder = documentFolder
                };

                var uploadResult = await cloudinary.UploadAsync(uploadParams);
                result.Success = uploadResult.StatusCode == HttpStatusCode.OK;
                result.Url = uploadResult.Url.ToString();
                result.PublicId = uploadResult.PublicId;
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError("{ex}", ex);
                return result;
            }
        }
    }
}
