# 001 – Add Work Order Priority

## Summary

Add a Priority field to the Work Order domain model, enabling users to classify work orders by urgency. This supports better triage, scheduling, and reporting of facility maintenance tasks.

## Motivation

The current Work Order model tracks status (Draft → Assigned → In Progress → Complete/Cancelled) but provides no mechanism for indicating urgency or importance. Facility managers need to distinguish between routine maintenance and urgent repairs so that work can be prioritized appropriately.

## Proposed Change

Introduce a `WorkOrderPriority` enumeration following the same pattern as `WorkOrderStatus`, and add a `Priority` property to the `WorkOrder` entity. The priority SHALL default to `Normal` for new work orders and SHALL be editable at any point in the work order lifecycle.

## Priority Levels

| Code | Key       | FriendlyName | SortBy |
|------|-----------|--------------|--------|
| LOW  | Low       | Low          | 1      |
| NRM  | Normal    | Normal       | 2      |
| HGH  | High      | High         | 3      |
| URG  | Urgent    | Urgent       | 4      |
| CRT  | Critical  | Critical     | 5      |

## Scope

### In Scope

- New `WorkOrderPriority` enumeration class (Core layer)
- `Priority` property on `WorkOrder` entity
- Database migration to add `Priority` column to `WorkOrder` table
- EF Core mapping for the new column
- UI display and editing of priority on work order forms
- Unit tests for the new domain behavior

### Out of Scope

- Priority-based notifications or SLA enforcement
- Priority-based sorting/filtering in list views (future enhancement)
- Historical tracking of priority changes (audit trail)

## Impact

- **Core**: New `WorkOrderPriority` class, modified `WorkOrder` class
- **DataAccess**: Updated EF mapping, new database migration script
- **UI**: Updated work order create/edit forms and detail views
- **Database**: New migration script (`022_AddPriorityToWorkOrder.sql`)
