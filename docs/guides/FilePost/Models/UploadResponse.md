> **Version:** v1.0.0

# UploadResponse

The object returned for each successfully uploaded file. Store the `PublicId` — you'll need it for deletion, signed URL generation, and any future file management operations.

## Properties

| Property   | Type     | Notes                                                                                                                                      |
| ---------- | -------- | ------------------------------------------------------------------------------------------------------------------------------------------ |
| `PublicId` | `string` | Cloudinary's unique identifier for the file. **Store this in your database.** Required for `DeleteFileAsync` and `GenerateSignedUrlAsync`. |
| `Url`      | `string` | The full public HTTPS URL to the uploaded file. This is what you'd store and serve if the file is publicly accessible.                     |
| `Format`   | `string` | The file format as detected by Cloudinary (e.g. `"jpg"`, `"pdf"`, `"png"`)                                                                 |

> For private files (e.g. user documents), don't store or expose the `Url` directly. Use `GenerateSignedUrlAsync` with the `PublicId` to produce a time-limited access URL each time it's needed.
