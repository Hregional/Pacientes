# DatosPacientes .NET 10 Upgrade Tasks

## Overview

This document tracks the execution of the DatosPacientes solution upgrade from .NET 6.0 to .NET 10.0. All three projects will be upgraded simultaneously in a single atomic operation, followed by comprehensive testing and validation.

**Progress**: 0/4 tasks complete (0%) ![0%](https://progress-bar.xyz/0)

---

## Tasks

### [▶] TASK-001: Verify prerequisites
**References**: Plan §Phase 0

- [ ] (1) Verify .NET 10 SDK installed per Plan §Prerequisites (check `dotnet --list-sdks` includes 10.0.xxx)
- [ ] (2) .NET 10 SDK is available (**Verify**)
- [ ] (3) Check for global.json file conflicts that might pin SDK to version < 10.0
- [ ] (4) No global.json conflicts blocking .NET 10 usage (**Verify**)

---

### [ ] TASK-002: Atomic framework and dependency upgrade with compilation fixes
**References**: Plan §Phase 1, Plan §Detailed Execution Steps, Plan §Package Update Reference, Plan §Breaking Changes Catalog

- [ ] (1) Update TargetFramework to net10.0 in all 3 project files per Plan §Step 1 (DatosPacientes, DatosPacientes.IntegrationTests, DatosPacientes.UnitTests)
- [ ] (2) All project files contain `<TargetFramework>net10.0</TargetFramework>` (**Verify**)
- [ ] (3) Update all package references per Plan §Package Update Reference (5 packages to version 10.x, remove Microsoft.VisualStudio.Azure.Containers.Tools.Targets, keep AutoMapper at 12.0.1)
- [ ] (4) All package references updated to target versions (**Verify**)
- [ ] (5) Restore dependencies for entire solution (`dotnet restore DatosPacientes.sln`)
- [ ] (6) All dependencies restored successfully (**Verify**)
- [ ] (7) Build solution and fix all compilation errors per Plan §Breaking Changes Catalog (focus on BC-001 ServiceCollectionExtensions, BC-002 to BC-007 JWT Bearer configuration, review BC-008 to BC-013 System.Uri usage)
- [ ] (8) Solution builds with 0 errors (**Verify**)

---

### [ ] TASK-003: Run full test suite and validate upgrade
**References**: Plan §Phase 2, Plan §Testing & Validation Strategy, Plan §Breaking Changes Catalog

- [ ] (1) Run tests in both test projects (DatosPacientes.UnitTests and DatosPacientes.IntegrationTests)
- [ ] (2) Fix any test failures (reference Plan §Breaking Changes Catalog for common issues, especially JWT authentication changes and EF Core 10 compatibility)
- [ ] (3) Re-run tests after fixes
- [ ] (4) All tests pass with 0 failures (**Verify**)

---

### [ ] TASK-004: Final commit
**References**: Plan §Source Control Strategy

- [ ] (1) Commit all changes with message: "Upgrade solution from .NET 6 to .NET 10 - All projects upgraded to net10.0, packages updated, breaking changes addressed, all tests passing"

---
