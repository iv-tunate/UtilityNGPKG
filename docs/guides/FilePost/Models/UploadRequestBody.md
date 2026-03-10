> **Version:** v1.0.0

# UploadRequestBody

Configuration options applied to file uploads — used to enforce size limits and control which file types are accepted before anything reaches Cloudinary.

## Properties

| Property            | Type            | Required | Default        | Notes                                                                                                                                   |
| ------------------- | --------------- | -------- | -------------- | --------------------------------------------------------------------------------------------------------------------------------------- |
| `MaxFileSizeInMB`   | `int`           | No       | `10`           | Maximum allowed file size in megabytes. Files larger than this are rejected with a descriptive error before any upload attempt is made. |
| `AllowedExtensions` | `List<string>?` | No       | All extensions | A list of permitted file extensions including the dot, e.g. `[".jpg", ".png", ".pdf"]`. If left null, all file types are accepted.      |

## Example

```csharp
var config = new UploadRequestBody
{
    MaxFileSizeInMB = 5,
    AllowedExtensions = new List<string> { ".jpg", ".jpeg", ".png" }
};
```
