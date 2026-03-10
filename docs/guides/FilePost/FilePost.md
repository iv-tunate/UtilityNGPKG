> **Version:** v1.0.0 &nbsp;|&nbsp; [Changelog](../../CHANGELOG.md)

# File Upload & Management

The FilePost module handles all file operations against Cloud storage providers — uploading files from your users, generating secure access URLs, retrieving files programmatically, and deleting files when they're no longer needed. All operations use `IFileService`.

## How to Access

`IFileService` is registered as a scoped service and is available via `AddUtilityNGPKG()`. Inject it into any service that handles file operations:

```csharp
public class MediaService
{
    private readonly IFileService _fileService;
    public MediaService(IFileService fileService)
    {
        _fileService = fileService;
    }
}
```

You'll also need your Cloudinary credentials. Keep these in your environment variables or secrets vault — never in source code:

```csharp
var credentials = new CloudinaryCredentials
{
    CloudName = _config["Cloudinary:CloudName"],
    ApiKey = _config["Cloudinary:ApiKey"],
    ApiSecret = _config["Cloudinary:ApiSecret"]
};
```

---

## Uploading Files

### UploadFileAsync

Uploads a single `IFormFile` to Cloudinary and returns a response containing the file's public URL and public ID. The public ID is what you store in your database — you'll need it for deletions and signed URL generation.

Before uploading, you configure size limits and allowed file types via an `UploadRequestBody` object:

```csharp
var uploadConfig = new UploadRequestBody
{
    MaxFileSizeInMB = 5,
    AllowedExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".pdf" }
};

var result = await _fileService.UploadFileAsync(file, credentials, uploadConfig);

if (result.IsSuccess)
{
    string publicUrl = result.Data.Url;
    string publicId = result.Data.PublicId;
    // store publicId in your database
}
```

If the file fails validation (wrong extension, too large), you'll get a failed `ResponseDetail` back with an explanation — no exception is thrown.

---

### UploadMultipleFilesAsync

Uploads a list of files in one call. Returns a list of `UploadResponse` objects, one per file. Files that fail validation are skipped with an error logged, so a partial success is possible.

```csharp
var results = await _fileService.UploadMultipleFilesAsync(files, credentials, uploadConfig);
```

---

## Retrieving Files

### DownloadFileAsync

Downloads a file from Cloudinary using its public URL and returns it as a byte array — useful when you need to stream the file to the client or process it server-side.

```csharp
byte[] fileBytes = await _fileService.DownloadFileAsync(publicUrl, credentials);
```

---

### GenerateSignedUrlAsync

Generates a time-limited signed URL for a private Cloudinary file. Use this when files shouldn't be publicly accessible but you need to give a specific user temporary access.

Pass the `publicId` (from upload), and how many seconds the URL should be valid for:

```csharp
string signedUrl = await _fileService.GenerateSignedUrlAsync(publicId, credentials, expiresInSeconds: 3600);
```

---

## Deleting Files

### DeleteFileAsync

Deletes a file from Cloudinary by its public ID. Returns `true` on success.

```csharp
bool deleted = await _fileService.DeleteFileAsync(publicId, credentials);
```

### DeleteMultipleFilesAsync

Deletes a batch of files by their public IDs. Returns `true` if all deletions succeeded.

```csharp
bool allDeleted = await _fileService.DeleteMultipleFilesAsync(publicIds, credentials);
```

---

## Model Reference

### CloudinaryCredentials

| Property    | Type     | Required | Notes                                         |
| ----------- | -------- | -------- | --------------------------------------------- |
| `CloudName` | `string` | Yes      | From your Cloudinary dashboard                |
| `ApiKey`    | `string` | Yes      | From your Cloudinary dashboard                |
| `ApiSecret` | `string` | Yes      | Keep this secret — never expose it to clients |

### UploadRequestBody

| Property            | Type           | Required | Default     | Notes                              |
| ------------------- | -------------- | -------- | ----------- | ---------------------------------- |
| `MaxFileSizeInMB`   | `int`          | No       | `10`        | Max allowed file size in megabytes |
| `AllowedExtensions` | `List<string>` | No       | All allowed | e.g. `[".jpg", ".png"]`            |

### UploadResponse

| Property   | Type     | Notes                                                     |
| ---------- | -------- | --------------------------------------------------------- |
| `PublicId` | `string` | Store this in your DB — used for deletion and signed URLs |
| `Url`      | `string` | The full public Cloudinary URL for the uploaded file      |
| `Format`   | `string` | File format as detected by Cloudinary (e.g. `"jpg"`)      |
