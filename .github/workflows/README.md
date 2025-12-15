# GitHub Actions Workflows

This directory contains comprehensive GitHub Actions workflows that automate the entire software development lifecycle for the Patient Timeline application.

## Overview

The workflows are organized into the following categories:

### 🔄 Continuous Integration & Deployment
- **[ci-cd.yml](ci-cd.yml)** - Main CI/CD pipeline with build, test, security scan, and Docker image creation
- **[release.yml](release.yml)** - Automated release creation and Docker image publishing
- **[deploy.yml](deploy.yml)** - Azure deployment to staging/production environments

### 🧪 Quality Assurance
- **[quality-gates.yml](quality-gates.yml)** - Comprehensive testing, linting, and code quality checks
- **[e2e-tests.yml](e2e-tests.yml)** - End-to-end testing with Playwright, API tests, and load testing
- **[performance.yml](performance.yml)** - Performance monitoring, benchmarking, and Lighthouse audits

### 🔒 Security & Compliance
- **[security.yml](security.yml)** - Security vulnerability scanning, license checks, and secrets detection
- **[codeql-analysis.yml](codeql-analysis.yml)** - Automated CodeQL security analysis

### 🔄 Maintenance & Automation
- **[dependency-updates.yml](dependency-updates.yml)** - Automated dependency updates and security patches
- **[documentation.yml](documentation.yml)** - Automated documentation generation and publishing
- **[issue-management.yml](issue-management.yml)** - Automated issue triage, stale management, and project tracking
- **[backup-recovery.yml](backup-recovery.yml)** - Database backup, disaster recovery testing, and compliance audits
- **[environment-sync.yml](environment-sync.yml)** - Configuration validation, drift detection, and environment synchronization

## Prerequisites

### Required Secrets
Set these secrets in your GitHub repository settings:

```bash
# Azure Credentials
AZURE_CREDENTIALS
AZURE_SUBSCRIPTION_ID
AZURE_STORAGE_ACCOUNT
AZURE_STORAGE_KEY
AZURE_STATIC_WEB_APPS_API_TOKEN

# Security Scanning
SNYK_TOKEN
GITLEAKS_LICENSE
FOSSA_API_KEY

# Container Registry
GITHUB_TOKEN (automatically provided)
```

### Required Software Versions
- .NET 9.0.x
- Node.js 20.x
- Docker Buildx
- PostgreSQL 15
- Terraform 1.5.0
- kubectl v1.28.0
- Helm v3.12.0

## Workflow Details

### CI/CD Pipeline (`ci-cd.yml`)
**Triggers:** Push to main/develop, Pull Requests
**Purpose:** Complete build, test, and package pipeline

**Jobs:**
1. **build-api** - Build .NET API with caching
2. **build-web** - Build React frontend with caching
3. **test-api** - Run .NET unit tests with coverage
4. **test-web** - Run React unit tests with coverage
5. **security-scan** - Trivy vulnerability scanning
6. **docker-build** - Multi-stage Docker build
7. **publish-artifacts** - Upload build artifacts

### Quality Gates (`quality-gates.yml`)
**Triggers:** Push/PR to main/develop
**Purpose:** Ensure code quality and prevent regressions

**Jobs:**
1. **test** - Unit tests, integration tests, coverage
2. **security-scan** - Trivy filesystem and config scanning
3. **lint** - ESLint, Prettier, .NET code analysis

### Security Scanning (`security.yml`)
**Triggers:** Push/PR, Daily schedule
**Purpose:** Comprehensive security assessment

**Jobs:**
1. **security-scan** - Trivy, Snyk, GitLeaks scanning
2. **compliance-check** - License compliance, dependency auditing
3. **secrets-scan** - Secrets detection in code
4. **container-security** - Docker image vulnerability scanning

### Deployment (`deploy.yml`)
**Triggers:** Push to main, Manual dispatch
**Purpose:** Deploy to Azure environments

**Jobs:**
1. **deploy** - Deploy API to Web App, Web to Static Web App

### Release Management (`release.yml`)
**Triggers:** Version tags (v*)
**Purpose:** Create releases and publish Docker images

**Jobs:**
1. **release** - Create GitHub release with artifacts
2. **publish-docker** - Push to GitHub Container Registry

### End-to-End Testing (`e2e-tests.yml`)
**Triggers:** Push to main, Weekly schedule
**Purpose:** Full application testing

**Jobs:**
1. **e2e-test** - Playwright UI tests, API integration tests
2. **contract-test** - Pact contract testing
3. **accessibility-test** - axe-core and Lighthouse accessibility
4. **api-load-test** - k6 load testing

### Performance Monitoring (`performance.yml`)
**Triggers:** Push/PR, Daily/Weekly schedule
**Purpose:** Performance regression detection

**Jobs:**
1. **performance-test** - .NET benchmarks, Lighthouse audits
2. **load-test** - k6 load testing

### Dependency Management (`dependency-updates.yml`)
**Triggers:** Weekly schedule
**Purpose:** Keep dependencies secure and up-to-date

**Jobs:**
1. **update-dependencies** - Update NuGet/npm packages, create PR

### Documentation (`documentation.yml`)
**Triggers:** Push to main
**Purpose:** Keep documentation synchronized

**Jobs:**
1. **update-docs** - Generate API docs, update Storybook, deploy to GitHub Pages

### Issue Management (`issue-management.yml`)
**Triggers:** Issues/PRs events, Daily schedule
**Purpose:** Automate project management

**Jobs:**
1. **triage-issues** - Auto-label issues based on content
2. **manage-stale** - Close inactive issues/PRs
3. **update-project** - Sync with GitHub Projects
4. **generate-reports** - Create project metrics reports

### Backup & Recovery (`backup-recovery.yml`)
**Triggers:** Weekly schedule, Manual dispatch
**Purpose:** Data protection and disaster recovery

**Jobs:**
1. **database-backup** - Automated database backups to Azure Storage
2. **application-backup** - Configuration backups
3. **disaster-recovery-test** - Recovery testing and validation
4. **compliance-audit** - Backup retention compliance

### Environment Sync (`environment-sync.yml`)
**Triggers:** Push to main, Daily schedule
**Purpose:** Maintain environment consistency

**Jobs:**
1. **config-validation** - Validate Terraform/K8s/Helm configs
2. **drift-detection** - Detect configuration drift
3. **environment-sync** - Sync staging/production environments
4. **secrets-rotation** - Monitor secret/certificates expiry
5. **compliance-monitoring** - Azure Policy compliance

## Usage Guide

### Running Workflows Manually
1. Go to GitHub repository → Actions tab
2. Select desired workflow
3. Click "Run workflow"
4. Fill in any required inputs

### Monitoring Workflow Runs
- Check the Actions tab for workflow status
- View detailed logs for each job
- Download artifacts from completed runs
- Set up notifications for workflow failures

### Troubleshooting
1. **Workflow fails to start:** Check repository secrets and permissions
2. **Build failures:** Review build logs, check dependency versions
3. **Test failures:** Run tests locally, check test configuration
4. **Deployment failures:** Verify Azure credentials and resource permissions
5. **Security scan failures:** Review vulnerability reports, update dependencies

### Customization
- Modify trigger conditions in workflow files
- Add/remove jobs based on project needs
- Update tool versions in setup steps
- Configure environment-specific variables

## Security Considerations

- All workflows run with minimal required permissions
- Secrets are masked in logs
- Security scans run on all PRs and pushes
- Automated dependency updates help prevent vulnerabilities
- Secrets rotation monitoring prevents credential expiry

## Performance Optimization

- Caching is implemented for dependencies and build artifacts
- Jobs run in parallel where possible
- Scheduled workflows run during off-peak hours
- Resource-intensive jobs are isolated

## Contributing

When adding new workflows:
1. Follow naming conventions (`kebab-case.yml`)
2. Include comprehensive comments
3. Add required secrets to documentation
4. Test workflows in a separate branch first
5. Update this README with new workflow details

## Support

For workflow issues:
1. Check GitHub Actions documentation
2. Review workflow logs for error details
3. Verify all required secrets are set
4. Test locally before pushing workflow changes
5. Create an issue with workflow name and error logs