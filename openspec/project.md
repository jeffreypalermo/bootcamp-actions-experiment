# Project: Church Bulletin – Work Order Management System

## Overview

A Work Order management system built with .NET 9.0 implementing Onion Architecture. The system enables employees to create, assign, track, and complete work orders for church facility management.

## Technology Stack

- **.NET 9.0** – Primary framework
- **Blazor** – WebAssembly + Server UI
- **Entity Framework Core 9.0** – Data access (SQL Server)
- **MediatR** – CQRS command/query handling
- **Lamar** – Dependency injection container
- **NUnit / Shouldly / bUnit** – Testing frameworks
- **AliaSQL** – Database migrations

## Architecture

Onion Architecture with strict dependency rules:

- **Core** – Domain models, query objects, service interfaces (no external dependencies)
- **DataAccess** – EF Core implementation, query/command handlers (depends on Core only)
- **UI** – Blazor Server/Client, API endpoints (outer layer)
- **Database** – Migration scripts (AliaSQL numbered format: `###_Description.sql`)

## Domain Model

### WorkOrder

| Property       | Type              | Description                        |
|----------------|-------------------|------------------------------------|
| Id             | Guid              | Unique identifier                  |
| Title          | string?           | Short title                        |
| Description    | string?           | Detailed description (max 4000)    |
| RoomNumber     | string?           | Facility room reference            |
| Status         | WorkOrderStatus   | Current workflow status             |
| Creator        | Employee?         | Employee who created the order     |
| Assignee       | Employee?         | Employee assigned to fulfill       |
| Number         | string?           | Human-readable work order number   |
| AssignedDate   | DateTime?         | Date assigned                      |
| CreatedDate    | DateTime?         | Date created                       |
| CompletedDate  | DateTime?         | Date completed                     |

### WorkOrderStatus (Enumeration Pattern)

| Code | Key        | FriendlyName  |
|------|------------|---------------|
| DFT  | Draft      | Draft         |
| ASD  | Assigned   | Assigned      |
| IPG  | InProgress | In Progress   |
| CMP  | Complete   | Complete      |
| CNL  | Cancelled  | Cancelled     |

### Employee

Properties: UserName, FirstName, LastName, EmailAddress, Roles

### Role

Properties: Name, CanCreateWorkOrder, CanFulfillWorkOrder

## Naming Conventions

- **C#**: PascalCase for classes/methods, camelCase for local variables
- **Database**: PascalCase table/column names
- **Tests**: `[MethodName]_[Scenario]_[ExpectedResult]` naming pattern
- **Test doubles**: "Stub" prefix (not "Mock")
- **SQL scripts**: Tabs for indentation
