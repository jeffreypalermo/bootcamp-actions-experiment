# Integration Build Workflow Refactoring

This document describes the refactoring of the `integration-build.yml` workflow file.

## Summary

The monolithic `integration-build.yml` file (656 lines) has been refactored into:
- **Main orchestrator**: `integration-build.yml` (75 lines)
- **5 reusable workflow files** that contain the actual job logic

## Structure

### Main Orchestrator: integration-build.yml
This file now serves as a simple orchestrator that:
- Defines the workflow triggers (push, pull_request, workflow_dispatch)
- Sets version environment variables (MAJOR_VERSION, MINOR_VERSION)
- Calls 5 reusable workflows in sequence with proper dependencies

### Reusable Workflow Files

1. **build-linux.yml** (264 lines)
   - Builds the .NET solution
   - Runs unit and integration tests
   - Publishes test results and code coverage
   - Creates NuGet packages
   - Publishes packages to GitHub Packages and Octopus Deploy

2. **docker-build-push.yml** (154 lines)
   - Downloads NuGet packages from build-linux job
   - Extracts ChurchBulletin.UI package
   - Builds Docker image
   - Pushes image to Azure Container Registry

3. **deploy-tdd.yml** (204 lines)
   - Creates Octopus release
   - Deploys to TDD environment
   - Runs acceptance tests against TDD
   - Uploads test results

4. **deploy-uat.yml** (56 lines)
   - Deploys release to UAT environment
   - Waits for deployment completion

5. **deploy-prod.yml** (56 lines)
   - Deploys release to Prod environment
   - Waits for deployment completion

## Job Dependencies

The jobs execute in sequence:
```
build-linux
    ↓
docker-build-image-for-churchbulletin-ui
    ↓
deploy-to-tdd
    ↓
deploy-to-uat
    ↓
deploy-to-prod
```

## Benefits

1. **Maintainability**: Each workflow file focuses on a single responsibility
2. **Readability**: Main workflow is now very short and easy to understand
3. **Reusability**: Individual workflow files can be called from other workflows
4. **Modularity**: Changes to one stage don't require editing a large file
5. **Testing**: Individual workflows can be tested independently

## Input Parameters

All reusable workflows accept:
- `major_version`: Major version number (string)
- `minor_version`: Minor version number (string)
- `run_number`: GitHub run number (string)

Each workflow also accepts the necessary secrets for its operations.
