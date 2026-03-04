# 001 – Add Work Order Priority: Implementation Tasks

## Tasks

- [ ] **Core: Create `WorkOrderPriority` class**
  - Follow `WorkOrderStatus` enumeration pattern with Code, Key, FriendlyName, SortBy
  - Define levels: Low, Normal, High, Urgent, Critical
  - Include `FromCode()`, `FromKey()`, `Parse()`, `GetAllItems()` methods
  - Add JSON converter (`WorkOrderPriorityJsonConverter`)

- [ ] **Core: Add `Priority` property to `WorkOrder`**
  - Add `Priority` property of type `WorkOrderPriority`
  - Default value: `WorkOrderPriority.Normal`

- [ ] **Database: Create migration script**
  - Script `022_AddPriorityToWorkOrder.sql`
  - Add `Priority` column (`CHAR(3)`, NOT NULL, DEFAULT `'NRM'`) to `WorkOrder` table

- [ ] **DataAccess: Update EF Core mapping**
  - Map `Priority` column in the `WorkOrder` entity mapping
  - Configure value conversion between `WorkOrderPriority` and its `Code` string

- [ ] **UI: Update work order forms**
  - Add priority dropdown to create/edit work order components
  - Display priority on work order detail views and list items

- [ ] **Tests: Unit tests**
  - `WorkOrderPriority` enumeration behavior (FromCode, FromKey, equality)
  - `WorkOrder.Priority` default value
  - JSON serialization/deserialization of `WorkOrderPriority`

- [ ] **Tests: Integration tests**
  - Persist and retrieve a `WorkOrder` with each priority level
  - Verify database column default value
