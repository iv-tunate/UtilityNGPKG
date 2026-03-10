# Contributing to UtilityNGPKG

First off, thank you for considering contributing to UtilityNGPKG! It's people like you that make this open source package such a great utility for the .NET community.

## How Can I Contribute?

### Reporting Bugs 🐛

If you find a bug, please check the existing issues to see if it has already been reported. If not, open a new issue following the [Issues Guide](issues.md).

### Suggesting Enhancements 💡

We are always open to new features and ideas. Whether it's adding a new payment gateway provider or enhancing regex support, your suggestions are welcome. Please open an issue to start a discussion.

### Submitting Pull Requests 🛠️

To make a code contribution:

1. Fork the repository and create your branch from `main`.
2. Ensure you have tested your code. Our GitHub Actions CI will also run the test suite.
3. Update the documentation inside `docs/` if you add new features.
4. Issue that pull request! Follow the [Pull Requests Guide](pull-requests.md) for detailed steps.

## Developer Environment Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/your-repo/UtilityNGPKG.git
   ```
2. Open `UtilityNGPKG.slnx` or `UtilityNGPKG.csproj` in Visual Studio or VS Code.
3. Build the project:
   ```bash
   dotnet build
   ```
4. Run tests:
   ```bash
   dotnet test
   ```

## Documentation

We use **DocFX** for generating static documentation. If you modify any public API, please ensure you update or add markdown files under the `docs/` matching folder.

Welcome aboard!
