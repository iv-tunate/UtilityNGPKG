> **Version:** v1.0.0

# CloudinaryCredentials

Authentication details for connecting to Cloudinary. Pass this into every `IFileService` method call.

> **Keep these out of source code.** Store in environment variables or a secrets vault — the `ApiSecret` in particular must never be exposed publicly.

## Properties

| Property    | Type     | Required | Notes                                                                         |
| ----------- | -------- | -------- | ----------------------------------------------------------------------------- |
| `CloudName` | `string` | Yes      | Found on your Cloudinary dashboard home page                                  |
| `ApiKey`    | `string` | Yes      | Found on your Cloudinary dashboard — safe to include in backend config        |
| `ApiSecret` | `string` | Yes      | Found on your Cloudinary dashboard — **keep secret**, never expose to clients |
