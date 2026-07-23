# AI Prompts — Debugging Phase

> **How to use:** Open a new chat when you hit a bug. Paste the error details into the prompt.

---

## Prompt 1 — Debug Invalid Status Transition Behavior

**When:** Status transition returns wrong status code or wrong error message.

**@ references:** `requirements-analysis.md`, relevant source files

### Copy into chat:

```
I have a bug in the ticket status state machine.

Problem:
[PASTE: what you expected vs what happened]

Error / response:
[PASTE: HTTP status, response body, or stack trace]

Relevant code:
[PASTE or @-reference the controller/service file]

Expected behavior per @requirements-analysis.md:
- Valid transitions succeed with 200
- Invalid transitions return 400 with clear error message and code INVALID_TRANSITION

Help me:
1. Identify the root cause
2. Propose a minimal fix (do not refactor unrelated code)
3. Suggest a test to prevent regression

After fixing, I will log this in debugging-notes.md.
```

### Response Log

| Field | Your notes |
|-------|------------|
| **Date** | |
| **Problem** | |
| **AI response summary** | |
| **Fix applied** | |
| **Verified how** | |

---

## Prompt 2 — Debug API / Frontend Integration Issue

**When:** UI not showing data, wrong query params, CORS error, etc.

### Copy into chat:

```
I have an integration bug between React frontend and ASP.NET API.

Problem:
[PASTE: describe the UI behavior]

Browser network tab:
[PASTE: request URL, method, status code, response body]

Frontend code:
[@-reference or paste the fetch/axios call]

Backend endpoint:
[@-reference or paste the controller action]

Help me trace the issue step by step and provide a minimal fix.
Do not change unrelated files.
```

### Response Log

| Field | Your notes |
|-------|------------|
| **Date** | |
| **Problem** | |
| **AI response summary** | |
| **Fix applied** | |
| **Verified how** | |

---

## Prompt 3 — Debug Database / Migration Issue

**When:** Migration fails, seed data missing, FK constraint errors.

### Copy into chat:

```
I have a database setup issue.

Error:
[PASTE: full error message]

Setup steps I followed:
[PASTE: from database/setup-notes.md]

Environment:
- DB: PostgreSQL / SQLite
- OS: Windows

Files:
[@-reference DbContext, migration, seed files]

Help me fix the issue and update setup-notes.md if steps need correction.
```

### Response Log

| Field | Your notes |
|-------|------------|
| **Date** | |
| **Problem** | |
| **AI response summary** | |
| **Fix applied** | |
| **Verified how** | |
