# UI Flow

## Screen Map

```
Ticket List ──► Ticket Detail ──► (edit fields, change status, add comment)
     │
     └──► Create Ticket
```

## Flow 1: View and Search Tickets

1. User lands on **Ticket List** page
2. Sees all tickets in a table (title, priority, status, assignee, dates)
3. Types keyword in search box → list filters (debounced API call)
4. Selects status from dropdown → list filters by status
5. Clicks a row → navigates to **Ticket Detail**

## Flow 2: Create Ticket

1. User clicks **Create Ticket** on list page
2. Form: title (required), description, priority (dropdown), assignee (dropdown from seeded users), createdBy (dropdown or default)
3. Submit → POST `/api/tickets`
4. On success → redirect to ticket detail or list
5. On validation error → show inline field errors

## Flow 3: View and Update Ticket Detail

1. User opens ticket detail (GET `/api/tickets/{id}`)
2. Sees ticket fields, current status, comment thread
3. Edits title/description/priority/assignee → Save → PUT `/api/tickets/{id}`
4. On error → show alert with API message

## Flow 4: Change Status (State Machine)

1. User sees current status badge
2. Status dropdown shows **only valid next statuses** (computed from current status)
3. User selects new status → PATCH `/api/tickets/{id}/status`
4. On success → UI updates status badge
5. On invalid transition (if user bypasses UI) → show error: _"Cannot transition from X to Y"_

## Flow 5: Add Comment

1. User types comment in text area at bottom of detail page
2. Selects createdBy (or uses default user)
3. Submit → POST `/api/tickets/{id}/comments`
4. On success → comment appears in thread with timestamp and author

## Error States

| Scenario | UI Behavior |
|----------|-------------|
| Network failure | "Unable to connect" banner |
| 400 validation | Inline field errors or toast |
| 400 invalid transition | Toast with server message |
| 404 not found | "Ticket not found" page with link back to list |
| Empty search results | "No tickets match your search" |
