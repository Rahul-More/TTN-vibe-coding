# Data Model

## Entity Relationship

```
Users ──────< Tickets (createdBy, assignedTo)
Users ──────< Comments (createdBy)
Tickets ────< Comments (ticketId)
```

## Users

| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | PK, identity |
| Name | varchar(100) | NOT NULL |
| Email | varchar(200) | NOT NULL, unique |
| Role | varchar(50) | NOT NULL |

**Seed data:** 3–5 users with roles (Admin, Agent, etc.)

## Tickets

| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | PK, identity |
| Title | varchar(200) | NOT NULL |
| Description | varchar(2000) | NULL |
| Priority | varchar(20) | NOT NULL (Low, Medium, High) |
| Status | varchar(20) | NOT NULL, default 'Open' |
| AssignedToId | int | FK → Users, NULL |
| CreatedById | int | FK → Users, NOT NULL |
| CreatedAt | timestamptz | NOT NULL |
| UpdatedAt | timestamptz | NOT NULL |

**Indexes:** `Status`, `Title` (for search)

## Comments

| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | PK, identity |
| TicketId | int | FK → Tickets, NOT NULL |
| Message | varchar(1000) | NOT NULL |
| CreatedById | int | FK → Users, NOT NULL |
| CreatedAt | timestamptz | NOT NULL |

## Status Enum

| Value | Description |
|-------|-------------|
| Open | Initial state |
| InProgress | Being worked on |
| Resolved | Fix applied, pending close |
| Closed | Terminal state |
| Cancelled | Terminal state |

## State Machine (enforced in application layer)

```
Open        → InProgress | Cancelled
InProgress  → Resolved   | Cancelled
Resolved    → Closed
```

All other transitions are rejected.
