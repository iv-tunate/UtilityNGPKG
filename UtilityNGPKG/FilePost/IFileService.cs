using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityNGPKG.FilePost.DTOs.Cloudinary;
using UtilityNGPKG.ResponseDetail;

namespace UtilityNGPKG.FilePost
{
    /// <summary>
    /// Defines methods for uploading, downloading, generating signed URLs, and deleting files from storage providers such as cloudinary.
    /// </summary>
    /// <remarks>Implementations of this interface provide file management operations that interact with
    /// the storage provider. Methods are asynchronous and return results indicating the outcome of each operation. Credentials
    /// for the storage provider must be supplied for each method call.</remarks>
    public interface IFileService
    {
        /// <summary>
        /// Uploads a document to Cloudinary using the specified file, request details, and credentials.
        /// </summary>
        /// <param name="requestBody">The request details for the document upload, including any metadata or configuration options. Cannot be
        /// null.</param>
        /// <param name="file">The file to upload to Cloudinary. Must be a valid, non-null file representing the document to be uploaded.</param>
        /// <param name="credentials">The Cloudinary credentials used to authenticate the upload request. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an UploadResponse with
        /// information about the uploaded document.</returns>
        Task<UploadResponse> UploadDocument_Cloudinary(UploadRequestBody requestBody, IFormFile file, CloudinaryCredentials credentials);

        /// <summary>
        /// Generates a signed URL for accessing a Cloudinary resource using the specified credentials and public
        /// identifier.
        /// </summary>
        /// <param name="credentials">The Cloudinary credentials used to authenticate and sign the URL. Cannot be null.</param>
        /// <param name="publicId">The public identifier of the Cloudinary resource for which to generate the signed URL. Cannot be null or
        /// empty.</param>
        /// <returns>A tuple containing the signed URL as a string and a Boolean value indicating whether the operation was
        /// successful. The signed URL is valid only if success is <see langword="true"/>.</returns>
        Task<(string signedUrl, bool success)> GenerateSignedUrl_Cloudinary(CloudinaryCredentials credentials, string publicId);
        
        /// <summary>
        /// Downloads a file from Cloudinary using the specified credentials and resource identifier.
        /// </summary>
        /// <param name="credentials">The Cloudinary credentials used to authenticate the download request. Cannot be null.</param>
        /// <param name="urlOrPublicId">The URL or public ID of the Cloudinary resource to download. Must not be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The result contains a tuple with the file stream, the
        /// content type of the file, and a value indicating whether the download was successful. If the download fails,
        /// the file stream may be null and success will be <see langword="false"/>.</returns>
        Task<(MemoryStream? fileStream, string contentType, bool success)> Download_Cloudinary( CloudinaryCredentials credentials, string urlOrPublicId);
        
        /// <summary>
        /// Deletes a resource from Cloudinary using the specified credentials and public identifier.
        /// </summary>
        /// <param name="credentials">The Cloudinary account credentials used to authenticate the deletion request. Cannot be null.</param>
        /// <param name="publicId">The public identifier of the Cloudinary resource to delete. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the resource
        /// was successfully deleted; otherwise, <see langword="false"/>.</returns>
        Task<bool> Delete_Cloudinary(CloudinaryCredentials credentials, string publicId);
    }
}
