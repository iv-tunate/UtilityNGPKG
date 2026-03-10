# Pull Requests Guide

We appreciate your effort in contributing to UtilityNGPKG via Pull Requests! To ensure a smooth review process, please adhere to these guidelines.

## Pull Request Checklist

Before submitting a Pull Request, please verify the following:

- [ ] **Branching**: Your PR should branch off from the latest `main` branch. Use a descriptive branch name (e.g., `feature/add-aws-s3-support`, `bugfix/fix-jwt-exception`).
- [ ] **Tests**: Include unit tests for any new features or bug fixes. The CI pipeline will automatically run `dotnet test`, ensure it passes locally first.
- [ ] **Documentation**: If your PR introduces new classes, models, or methods, update the corresponding markdown documentation in the `docs` folder.
- [ ] **Style Constraints**: Ensure your code follows the standard C# conventions (C# 10/11 standards used in this project). Keep code clean and well-commented.

## PR Review Process

1. After submitting, a maintainer will review your code.
2. If changes are needed, the maintainer will request them via GitHub review comments.
3. Once approved, the maintainer will squash and merge your PR into the `main` branch.

Thank you for improving UtilityNGPKG!
