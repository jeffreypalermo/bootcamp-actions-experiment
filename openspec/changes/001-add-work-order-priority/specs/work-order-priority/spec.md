# Feature: Work Order Priority

## Status

PROPOSED

## Description

The system SHALL support a priority classification on each work order, enabling users to indicate the urgency of facility maintenance tasks. Priority is an intrinsic property of a work order, set at creation and modifiable throughout the work order lifecycle.

---

## Requirements

### REQ-PRIORITY-001: Priority Enumeration

The system SHALL define the following priority levels as a closed enumeration:

| Code | Key       | FriendlyName | SortBy | Description                              |
|------|-----------|--------------|--------|------------------------------------------|
| LOW  | Low       | Low          | 1      | Routine tasks with no time pressure      |
| NRM  | Normal    | Normal       | 2      | Standard tasks with typical scheduling   |
| HGH  | High      | High         | 3      | Important tasks needing prompt attention  |
| URG  | Urgent    | Urgent       | 4      | Time-sensitive tasks requiring rapid action |
| CRT  | Critical  | Critical     | 5      | Emergency tasks requiring immediate action |

**Scenario: Enumerate all priority levels**

```
GIVEN the WorkOrderPriority type
WHEN GetAllItems is called
THEN it SHALL return exactly five items: Low, Normal, High, Urgent, Critical
AND they SHALL be ordered by SortBy ascending
```

**Scenario: Resolve priority from code**

```
GIVEN a valid priority code "HGH"
WHEN FromCode is called with "HGH"
THEN it SHALL return the High priority instance
```

**Scenario: Resolve priority from key**

```
GIVEN a valid priority key "Urgent"
WHEN FromKey is called with "Urgent"
THEN it SHALL return the Urgent priority instance
```

**Scenario: Invalid key**

```
GIVEN an invalid priority key "Unknown"
WHEN FromKey is called with "Unknown"
THEN it SHALL throw an ArgumentOutOfRangeException
```

---

### REQ-PRIORITY-002: Default Priority

A newly created work order SHALL have its Priority set to `Normal` (Code: `NRM`).

**Scenario: New work order default priority**

```
GIVEN a new WorkOrder is instantiated
WHEN no priority is explicitly set
THEN the Priority property SHALL equal WorkOrderPriority.Normal
```

---

### REQ-PRIORITY-003: Priority Mutability

The priority of a work order SHALL be modifiable regardless of the work order's current status.

**Scenario: Change priority on a draft work order**

```
GIVEN a work order in Draft status
WHEN the priority is changed to Critical
THEN the Priority property SHALL equal WorkOrderPriority.Critical
```

**Scenario: Change priority on an in-progress work order**

```
GIVEN a work order in InProgress status
WHEN the priority is changed to Low
THEN the Priority property SHALL equal WorkOrderPriority.Low
```

---

### REQ-PRIORITY-004: Priority Persistence

The system SHALL persist the priority of a work order as a `CHAR(3)` column in the `WorkOrder` database table, storing the priority code value.

**Scenario: Persist and retrieve priority**

```
GIVEN a work order with Priority set to High
WHEN the work order is saved and subsequently retrieved from the database
THEN the retrieved work order Priority SHALL equal WorkOrderPriority.High
```

**Scenario: Database default value**

```
GIVEN a new row is inserted into the WorkOrder table without specifying Priority
WHEN the row is read back
THEN the Priority column SHALL contain 'NRM'
```

---

### REQ-PRIORITY-005: Priority Serialization

The system SHALL serialize `WorkOrderPriority` to and from JSON using the Key string value.

**Scenario: Serialize priority to JSON**

```
GIVEN a WorkOrderPriority of Urgent
WHEN serialized to JSON
THEN the JSON value SHALL be the string "Urgent"
```

**Scenario: Deserialize priority from JSON**

```
GIVEN a JSON string "Critical"
WHEN deserialized to WorkOrderPriority
THEN the result SHALL equal WorkOrderPriority.Critical
```

---

### REQ-PRIORITY-006: Priority Display

The system SHALL display the `FriendlyName` of the priority when rendering a work order in the user interface.

**Scenario: Display priority in work order detail**

```
GIVEN a work order with Priority set to High
WHEN the work order detail view is rendered
THEN the displayed priority text SHALL be "High"
```

---

## Data Model Changes

### WorkOrder Table (Modified)

```sql
ALTER TABLE [dbo].[WorkOrder]
    ADD [Priority] CHAR(3) NOT NULL DEFAULT 'NRM';
```

### WorkOrderPriority Class (New)

```
Namespace: ClearMeasure.Bootcamp.Core.Model
Pattern:   Enumeration (same as WorkOrderStatus)
```

### WorkOrder Class (Modified)

```
+ Priority : WorkOrderPriority = WorkOrderPriority.Normal
```

---

## Affected Components

| Layer      | Component                  | Change Type |
|------------|----------------------------|-------------|
| Core       | WorkOrderPriority.cs       | ADDED       |
| Core       | WorkOrder.cs               | MODIFIED    |
| DataAccess | WorkOrder mapping          | MODIFIED    |
| Database   | 022_AddPriorityToWorkOrder | ADDED       |
| UI.Client  | Work order forms           | MODIFIED    |
| UI.Shared  | Shared models              | MODIFIED    |
