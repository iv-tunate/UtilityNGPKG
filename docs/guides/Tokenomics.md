> **Version:** v1.0.0 &nbsp;|&nbsp; [Changelog](../../CHANGELOG.md)

# Tokenomics

The Tokenomics module is your toolkit for everything authentication and cryptography-related — JWT generation, password hashing, encryption/decryption, random token generation, and signature verification. If your application needs to issue tokens, hash passwords, or verify payload integrity, this is where that logic lives.

## How to Access

Everything is exposed through the `ITokenBuilder` interface. Register it once in your DI setup (or use the package's built-in extension method — see [Getting Started](../../index.md)), and then inject it wherever you need it:

```csharp
// In Program.cs (already done if you called AddUtilityNGPKG)
services.AddSingleton<ITokenBuilder, TokenBuilder>();

// In your service or controller
public class AuthService
{
    private readonly ITokenBuilder _tokenBuilder;
    public AuthService(ITokenBuilder tokenBuilder)
    {
        _tokenBuilder = tokenBuilder;
    }
}
```

---

## Available Methods

### GenerateJWTToken

Generates a signed JSON Web Token for an authenticated user. It builds a set of standard claims (user ID, email, role, verification status) and any custom claims you want to include, then signs the token using HMAC-SHA512.

**Returns:** A tuple — the JWT string itself, and a boolean indicating whether the generation succeeded.

**Key requirements:**

- `SecretKey` must be exactly **64 bytes (512 bits)**. Anything shorter will fail. You can use `GenerateRandomToken(64)` (see below) to produce a suitable key.
- `ExpirationMinutes` defaults to 60 if not set. Adjust based on your session policy.
- `Issuer` and `Audience` should match what you've configured in your JWT middleware validation settings.

**Example:**

```csharp
var jwtSettings = new JwtSettings
{
    UserId = user.Id,
    Email = user.Email,
    Role = user.Role,
    VerificationStatus = "Verified",
    SecretKey = "your-64-byte-secret-key-here...",
    Issuer = "https://yourapp.com",
    ExpirationMinutes = 120
};

var (token, success) = _tokenBuilder.GenerateJWTToken(jwtSettings);
if (success)
{
    // return the token to the client
}
```

See also: [JwtSettings](#jwtsettings-reference)

---

### Hash_Argon2id

Hashes a value (typically a password) using Argon2id — the current recommended standard for password storage. It's resistant to GPU-based attacks and brute-force attempts, significantly more secure than bcrypt or PBKDF2 for this purpose.

**Returns:** A tuple with the encoded hash string and a success flag. Store the hash string in your database — it's self-contained and includes the salt and algorithm parameters.

```csharp
var (hashedPassword, ok) = _tokenBuilder.Hash_Argon2id(plainTextPassword);
```

---

### VerifyArgon2idHash

Verifies a plain text value against a previously hashed Argon2id string. Returns `true` if they match, `false` otherwise.

```csharp
bool isValid = _tokenBuilder.VerifyArgon2idHash(providedPassword, storedHash);
```

---

### GenerateRandomToken

Generates a cryptographically secure random token of a given byte length. The result is Base64-encoded. Use this for things like email verification tokens, password reset links, or generating a JWT secret key.

**`length`** — the number of bytes. For a JWT-compatible secret (512 bits), pass `64`.

```csharp
var (token, success) = _tokenBuilder.GenerateRandomToken(64);
// token is a Base64-encoded 64-byte string
```

---

### GenerateRandomNumber

Generates a random numeric string of a specified digit length. Useful for OTP codes (e.g., 6-digit PIN).

```csharp
var (otp, success) = _tokenBuilder.GenerateRandomNumber(6); // "483920"
```

---

### GenerateHMACSHA256

Creates an HMAC-SHA256 signature from a plain text value and a secret key. The output is Base64-encoded. This is useful for signing webhook payloads, short-lived tokens, or any data that needs integrity protection without full encryption.

```csharp
var (signature, success) = _tokenBuilder.GenerateHMACSHA256(payload, secretKey);
```

---

### VerifyHMACSHA256

Verifies that a piece of data matches an expected HMAC-SHA256 signature. Uses constant-time comparison internally so it's safe against timing attacks.

```csharp
var (error, isValid) = _tokenBuilder.VerifyHMACSHA256(payload, expectedSignature, secretKey);
```

If `isValid` is `false`, check the `error` string for the reason.

---

### EncryptAES / DecryptAES

Encrypts and decrypts data using AES-256-CBC with PKCS7 padding. The encrypted output is Base64-encoded.

**Requirements:**

- `key` must be at least **16 bytes**. For AES-256, use a 32-byte key.
- `iv` must be exactly **16 bytes**.

Both methods return a tuple with the result string, a success flag, and an error message if something went wrong.

```csharp
byte[] key = Encoding.UTF8.GetBytes("your-32-byte-key-here-padded!!!!");
byte[] iv = Encoding.UTF8.GetBytes("your-16-byte-iv!");

var (encrypted, ok, err) = _tokenBuilder.EncryptAES("sensitive data", key, iv);
var (decrypted, ok2, err2) = _tokenBuilder.DecryptAES(encrypted, key, iv);
```

> **Note:** The same key and IV used for encryption must be used for decryption. Never hardcode these in source code — store them in environment variables or a secrets vault.

---

### EncryptRSA / DecryptRSA

Asymmetric encryption using RSA with OAEP-SHA256 padding. Encrypt with a public key; decrypt with the corresponding private key. Useful for encrypting small payloads (like symmetric keys).

`EncryptRSA` returns a byte array. `DecryptRSA` takes the Base64-encoded ciphertext and returns the plain text string.

---

### GenerateRSAParameters

Generates a fresh 2048-bit RSA key pair and returns both the private and public `RSAParameters`. Use this during key generation workflows — not for every request.

```csharp
var (privateKey, publicKey) = _tokenBuilder.GenerateRSAParameters();
```

---

### ConstantTimeComparison

Compares two strings in constant time to prevent timing-based attacks. Returns `true` if the strings are equal. Use this when comparing secrets, tokens, or HMAC outputs — never use `==` for that.

---

## JwtSettings Reference

| Property             | Type          | Required | Default | Notes                            |
| -------------------- | ------------- | -------- | ------- | -------------------------------- |
| `UserId`             | `Guid`        | Yes      | —       | Mapped to the `nameid` claim     |
| `Email`              | `string`      | Yes      | —       | Mapped to the `email` claim      |
| `Role`               | `string`      | No       | `""`    | Mapped to the `role` claim       |
| `VerificationStatus` | `string`      | No       | `""`    | Custom claim                     |
| `SecretKey`          | `string`      | Yes      | —       | Must be 64 bytes (512-bit)       |
| `Issuer`             | `string`      | No       | `""`    | Match your JWT validation config |
| `Audience`           | `string[]`    | No       | `null`  | Match your JWT validation config |
| `ExpirationMinutes`  | `int`         | No       | `60`    | Token lifetime in minutes        |
| `CustomClaims`       | `List<Claim>` | No       | `null`  | Any additional claims to embed   |
