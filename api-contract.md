# API Contract

Base URL: `http://localhost:5000/api` (adjust per your setup)

---

## Users (read-only for Core)

### GET /users
**Purpose:** List seeded users for assignee dropdowns

**Response 200:**
```json
[
  { "id": 1, "name": "Alice Admin", "email": "alice@example.com", "role": "Admin" }
]
```

---

## Tickets

### GET /tickets
**Purpose:** List tickets with optional search and filter

**Query params:**
| Param | Type | Description |
|-------|------|-------------|
| search | string | Keyword search in title/description |
| status | string | Filter by status (Open, InProgress, Resolved, Closed, Cancelled) |

**Response 200:**
```json
[
  {
    "id": 1,
    "title": "Login issue",
    "description": "Cannot log in",
    "priority": "High",
    "status": "Open",
    "assignedTo": 2,
    "assignedToName": "Bob Agent",
    "createdBy": 1,
    "createdByName": "Alice Admin",
    "createdAt": "2026-07-17T10:00:00Z",
    "updatedAt": "2026-07-17T10:00:00Z"
  }
]
```

### GET /tickets/{id}
**Purpose:** Get ticket detail with comments

**Response 200:** Ticket object + `comments[]`  
**Response 404:** `{ "error": "Ticket not found" }`

### POST /tickets
**Purpose:** Create a new ticket

**Request:**
```json
{
  "title": "Login issue",
  "description": "Cannot log in after password reset",
  "priority": "High",
  "assignedTo": 2,
  "createdBy": 1
}
```

**Validation Rules:**
- `title` — required, max 200 chars
- `description` — optional, max 2000 chars
- `priority` — required, enum: Low | Medium | High
- `createdBy` — required, must exist in Users
- `assignedTo` — optional, must exist in Users if provided
- `status` — defaults to `Open` (not settable on create)

**Response 201:** Created ticket  
**Response 400:** Validation errors

### PUT /tickets/{id}
**Purpose:** Update ticket fields (not status)

**Request:**
```json
{
  "title": "Updated title",
  "description": "Updated description",
  "priority": "Medium",
  "assignedTo": 3
}
```

**Response 200:** Updated ticket  
**Response 400 / 404**

### PATCH /tickets/{id}/status
**Purpose:** Change ticket status via state machine

**Request:**
```json
{ "status": "InProgress" }
```

**Validation Rules:**
- Transition must be valid per state machine
- Invalid → 400 with `{ "error": "...", "code": "INVALID_TRANSITION" }`

**Valid transitions:**
- Open → InProgress, Cancelled
- InProgress → Resolved, Cancelled
- Resolved → Closed

### Error Responses (common)

| Code | Body |
|------|------|
| 400 | `{ "error": "Title is required" }` |
| 400 | `{ "error": "Cannot transition from Open to Closed", "code": "INVALID_TRANSITION" }` |
| 404 | `{ "error": "Ticket not found" }` |

---

## Comments

### POST /tickets/{ticketId}/comments
**Purpose:** Add a comment to a ticket

**Request:**
```json
{
  "message": "Investigating the issue",
  "createdBy": 2
}
```

**Validation Rules:**
- `message` — required, max 1000 chars
- `createdBy` — required, must exist
- `ticketId` — must exist

**Response 201:** Created comment  
**Response 400 / 404**
