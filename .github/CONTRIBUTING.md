# Contributing to Patient Timeline

Thank you for your interest in contributing to the Patient Timeline project! This document provides guidelines and information for contributors.

## Code of Conduct

This project follows a code of conduct to ensure a welcoming environment for all contributors. Please read our [Code of Conduct](CODE_OF_CONDUCT.md) before participating.

## How to Contribute

### Reporting Issues

- Use the issue templates when creating new issues
- Provide detailed information including steps to reproduce
- Include environment details (OS, .NET version, Node.js version, etc.)
- Check existing issues to avoid duplicates

### Development Setup

1. **Prerequisites:**
   - .NET 9.0 SDK
   - Node.js 18+ and npm
   - Docker Desktop (for local development)
   - Git

2. **Clone and setup:**
   ```bash
   git clone https://github.com/your-org/patient-timeline.git
   cd patient-timeline
   ./build.sh build
   ```

3. **Run locally:**
   ```bash
   ./build.sh docker-run
   ```

### Development Workflow

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/your-feature-name`
3. Make your changes following the coding standards
4. Write tests for new functionality
5. Ensure all tests pass: `./build.sh test`
6. Update documentation if needed
7. Commit your changes: `git commit -m "Add your feature"`
8. Push to your fork: `git push origin feature/your-feature-name`
9. Create a Pull Request

### Coding Standards

#### .NET Code
- Follow C# coding conventions
- Use meaningful variable and method names
- Add XML documentation comments for public APIs
- Use async/await for I/O operations
- Follow SOLID principles

#### React/TypeScript Code
- Use TypeScript for all new code
- Follow React best practices
- Use functional components with hooks
- Add PropTypes or TypeScript interfaces
- Follow Material-UI component guidelines

#### General
- Write clear, concise commit messages
- Keep PRs focused on a single feature or fix
- Add tests for new functionality
- Update documentation for API changes

### Testing

- Write unit tests for business logic
- Write integration tests for API endpoints
- Test UI components with React Testing Library
- Ensure all tests pass before submitting PR

### Documentation

- Update README.md for significant changes
- Add code comments for complex logic
- Update API documentation for endpoint changes
- Include examples for new features

### Commit Messages

Follow conventional commit format:
- `feat:` New features
- `fix:` Bug fixes
- `docs:` Documentation changes
- `style:` Code style changes
- `refactor:` Code refactoring
- `test:` Adding tests
- `chore:` Maintenance tasks

Example: `feat: add patient search functionality`

### Pull Request Process

1. Ensure your PR description clearly describes the changes
2. Reference any related issues
3. Ensure CI checks pass
4. Request review from maintainers
5. Address review feedback
6. Squash commits if requested

### Deployment

- Development: `./build.sh docker-run`
- Production: Use Terraform configurations in `deployments/terraform/`
- Kubernetes: Apply manifests from `deployments/k8s/`

## Getting Help

- Check existing issues and documentation
- Join our community discussions
- Contact maintainers for questions

Thank you for contributing to Patient Timeline! 🎉
