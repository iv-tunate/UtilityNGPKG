> **Version:** v1.0.0 &nbsp;|&nbsp; [Changelog](../CHANGELOG.md)


## RegexValidations

`RegexValidations` is a static class — no injection needed, just call its methods directly. It ships with compiled Regex patterns for common input validation scenarios.

> This class uses source-generated Regex on .NET 7 and above for better startup performance. It falls back to compiled Regex instances on .NET 6.

### IsValidMail

Checks whether a string is a valid email address format. Returns `true` if valid, `false` if not. Returns `false` immediately for null or whitespace input.

The pattern checks for a proper local part, `@` symbol, and a domain with at least one dot and a valid TLD.

```csharp
bool ok = RegexValidations.IsValidMail("user@example.com"); // true
bool bad = RegexValidations.IsValidMail("notanemail");       // false
```

---

### IsValidName

Validates that a name (or multiple names) contains only alphabetic characters. First name is required; last name and middle name are optional — if provided, they must also be alphabetic.

```csharp
bool ok = RegexValidations.IsValidName("John", "Doe");      // true
bool bad = RegexValidations.IsValidName("John2");           // false
```

---

### IsValidPhoneNumber

Validates an international phone number format. Accepts an optional `+` prefix, requires the first digit to be 1–9, and enforces a total length of 7–15 digits (aligned with E.164).

**What it covers:** Nigerian numbers like `+2348123456789`, US numbers like `+14155552671`, and most standard international formats.

**What it does NOT cover:** Numbers with spaces, dashes, or parentheses like `(234) 812-3456`. Strip all formatting before passing to this method.

```csharp
bool ok = RegexValidations.IsValidPhoneNumber("+2348012345678"); // true
bool bad = RegexValidations.IsValidPhoneNumber("0801-234-5678"); // false — contains dashes
```

---

### IsAcceptablePasswordFormat

Validates that a password meets minimum complexity requirements. A password passes if it:

- Is at least **8 characters** long
- Contains at least one **uppercase letter**
- Contains at least one **lowercase letter**
- Contains at least one **digit**
- Contains at least one **special character**

```csharp
bool ok = RegexValidations.IsAcceptablePasswordFormat("Secure@123"); // true
bool bad = RegexValidations.IsAcceptablePasswordFormat("password");  // false — no uppercase or special char
```
