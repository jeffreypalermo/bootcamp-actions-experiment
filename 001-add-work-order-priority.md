# Work Order: Add Work Order Priority

**ID:** 001
**Feature:** Work Order Priority
**Status:** Proposed

## Summary

Add a `Priority` field to the Work Order domain model so that facility maintenance tasks can be classified by urgency (Low, Normal, High, Urgent, Critical).

## OpenSpec

The full specification for this feature is located in the OpenSpec directory:

- **Proposal:** [openspec/changes/001-add-work-order-priority/proposal.md](openspec/changes/001-add-work-order-priority/proposal.md)
- **Tasks:** [openspec/changes/001-add-work-order-priority/tasks.md](openspec/changes/001-add-work-order-priority/tasks.md)
- **Feature Spec:** [openspec/changes/001-add-work-order-priority/specs/work-order-priority/spec.md](openspec/changes/001-add-work-order-priority/specs/work-order-priority/spec.md)

## Acceptance Criteria

1. A `WorkOrderPriority` enumeration class exists with levels: Low, Normal, High, Urgent, Critical
2. `WorkOrder.Priority` defaults to `Normal` on new instances
3. Priority is persisted as `CHAR(3)` in the database
4. Priority is editable regardless of work order status
5. Priority serializes to/from JSON using the Key string
6. Priority is displayed in the UI using its FriendlyName
