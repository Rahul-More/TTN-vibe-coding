# Assignment understanding document
_Exported on 7/23/2026 at 09:27:48 GMT+5:30 from Cursor (3.12.30)_

---

**User**

read the doc and understand the assignment 

---

**Cursor**

I'll look through the workspace for assignment or project documentation.


The workspace looks empty — I'll check prior chat transcripts for assignment context.


Found the assignment PDF — reading it now.


I read `c:\TTN\Net_AI_Assessment_ProjectGuide.docx.pdf`. Here is a clear breakdown of the assignment.

## What This Assignment Is

This is the **.NET AI Capability Exercise** — a **hands-on, self-paced exercise** (about **one week**) for all developers in the .NET competency (SE to TL). It is **not a graded test**. You get a **feedback report** and a **growth path** based on how you work with AI across the software lifecycle.

**What matters most:** not just whether the app works, but how you used AI for requirements, planning, coding, testing, debugging, review, documentation, and reflection. **Make your thinking visible.**

---

## Three Parts (Effort Split)

| Part | Focus | Effort |
|------|--------|--------|
| **Part A** | AI Workflow Foundation | ~20% |
| **Part B** | Full-Stack Mini Project (Core + optional Stretch) | ~60% |
| **Part C** | Submission & Reflection | ~20% |

- **Core app:** ~8–12 focused hours  
- **Lifecycle artifacts** (prompts, analysis, reflection, etc.) are equally important — **do not skip artifacts to build more app features**

---

## Part A: `tool-workflow.md`

Document how you use AI:

1. Primary AI tool
2. How you provide project context
3. How you use AI across the lifecycle (requirements → review)
4. What you avoid sharing with AI
5. How you’d reuse this workflow on a real project

---

## Part B: Choose One Project Option

You pick **one** of three options (the PDF lists all three in the summary; **only Option 1 is fully detailed** in your copy):

1. **Backend-Heavy** — Support Ticket Management System  
2. **Frontend-Heavy** — AI Learning Dashboard / Project Tracker  
3. **.NET Full-Stack** — SME ERP (Inventory Management)

Each has a **mandatory Core** and an **optional Stretch** tier.

### Common Technical Requirements (all options)

Every submission must include:

- Frontend + Backend API + **database persistence**
- DB setup/migration scripts + seed data
- Input validation + error handling
- At least one **search/filter** (Core)
- At least one **meaningful test tier** (Core)
- README with setup instructions
- **Full prompt history**
- All lifecycle artifacts in the repo structure

**Database:** mandatory (PostgreSQL, MySQL, MongoDB, SQLite, H2, etc.)  
**Auth:** optional (counts as Stretch if done well)

---

## Option 1 Details (Fully Specified in Your Doc)

**Support Ticket Management System**

### Entities
- **User** (seeded only — no user-management UI): `id`, `name`, `email`, `role`
- **Ticket**: `id`, `title`, `description`, `priority`, `status`, `assignedTo`, `createdBy`, `createdAt`, `updatedAt`
- **Comment**: `id`, `ticketId`, `message`, `createdBy`, `createdAt`

### Core Features
- CRUD-style ticket operations (create, list, detail, update fields, reassign)
- Add comments
- **Status state machine** (key judgment piece):
  - `Open` → `In Progress`
  - `In Progress` → `Resolved`
  - `Resolved` → `Closed`
  - `Open` → `Cancelled`
  - `In Progress` → `Cancelled`
  - Invalid transitions **rejected by backend** and shown clearly in UI
- Keyword search + filter by status
- Data persists across restarts
- Backend validation + meaningful UI error states

### Mandatory Tests
**Integration tests** proving state-machine rules (valid transitions succeed, invalid ones fail).

### Stretch (Optional)
User CRUD, auth, richer filters/sorting/pagination, more tests, Swagger, Docker/CI, reusable prompt templates, etc.

---

## Required Repository Structure

```
ai-practical-assessment/
├── README.md, candidate-info.md, tool-workflow.md
├── requirements-analysis.md, acceptance-criteria.md
├── implementation-plan.md, design-notes.md
├── api-contract.md, data-model.md, ui-flow.md
├── test-strategy.md, test-results.md
├── debugging-notes.md, code-review-notes.md
├── review-fixes.md, pr-description.md
├── reflection.md, final-ai-usage-summary.md
├── src/, tests/
├── database/ (schema, seed-data, setup-notes.md)
├── ai-prompts/ (planning, design, implementation, testing, etc.)
└── tool-specific/ (cursor-workflow/ if using Cursor)
```

For **Cursor**, also include in `tool-specific/cursor-workflow/`:
- `project-context.md`, `spec.md`, `tasks.md`, `cursor-rules-or-instructions.md`

---

## Part C: Submission

1. Submit via the **online form** (no deployment required)
2. Provide **Git repo link**, chosen option, and short answers about your decisions
3. Keep **organized commit history** (reviewers may ask you to point to specific fixes)

---

## What “Complete” Means

Working frontend + backend + DB + migrations + seed data + README + basic tests + prompt history + requirement analysis + design notes + reflection + PR description.

---

## Key Mindset

> **Strong work** = clear understanding before coding, documented AI iterations, clean architecture, tests, honest reflection  
> **Weak work** = blind copy-paste, shallow prompts, broken setup, generic docs

---

## Note on Your PDF

Your PDF’s table of contents references **Options 2 and 3** on later pages, but the file you have appears to include **full specs only for Option 1**. If you’re considering Options 2 or 3, you may need the complete guide from your competency owner.

---

If you want to proceed, tell me which project option you’re leaning toward and your stack (e.g. .NET + React + PostgreSQL), and I can help you plan the implementation and artifact checklist step by step. (I’m in **Ask mode** right now — I can guide and plan, but switch to **Agent mode** if you want me to scaffold the repo and start building.)

---

**User**

create md files in which we i shoul have promprt to enter in the chat seperately and also satisfied the document condition

---

**Cursor**

Creating the full assessment folder structure with artifact templates and copy-paste chat prompts for each lifecycle phase (Option 1: Support Ticket Management).


Now creating the `ai-prompts/` files with separate copy-paste chat prompts.


Created the full assessment scaffold at `c:\TTN\ai-practical-assessment\` — it matches the participant guide and includes **25 copy-paste chat prompts** across **7 separate prompt files**.

## Start here

Open **`PROMPT-ORDER.md`** — it lists every prompt in order (Steps 1–23).

## How to use the prompts

1. Open the prompt file for your current phase (e.g. `ai-prompts/planning.md`)
2. Start a **new Cursor chat** for each prompt
3. `@`-reference the files listed in the prompt
4. Copy the entire prompt block into chat
5. After the response, fill in the **Response Log** table in that same file

## What was created

### Lifecycle artifacts (required by the guide)
All root `.md` files: `candidate-info.md`, `tool-workflow.md`, `requirements-analysis.md`, `acceptance-criteria.md`, `implementation-plan.md`, `design-notes.md`, `api-contract.md`, `data-model.md`, `ui-flow.md`, `test-strategy.md`, `test-results.md`, `debugging-notes.md`, `code-review-notes.md`, `review-fixes.md`, `pr-description.md`, `reflection.md`, `final-ai-usage-summary.md`

### Copy-paste prompts (`ai-prompts/`)

| File | Prompts | Purpose |
|------|---------|---------|
| `planning.md` | 3 | Requirements, acceptance criteria, plan |
| `design.md` | 4 | Architecture, API, UI, test strategy |
| `implementation.md` | 5 | DB, state machine, API, frontend |
| `testing.md` | 3 | Integration tests |
| `debugging.md` | 3 | Fix bugs as they arise |
| `code-review.md` | 3 | Review and apply fixes |
| `documentation.md` | 4 | README, PR, reflection, checklist |

### Cursor-specific (`tool-specific/cursor-workflow/`)
`project-context.md`, `spec.md`, `tasks.md`, `cursor-rules-or-instructions.md`

### Placeholders for code (fill via implementation prompts)
`src/`, `tests/`, `database/schema-or-migrations/`, `database/seed-data/`

## Suggested first step

1. Fill in your name/dates in `candidate-info.md`
2. Open `ai-prompts/planning.md` → **Prompt 1**
3. Start a new chat, `@` the listed files, paste the prompt

The design docs (`api-contract.md`, `data-model.md`, etc.) are pre-filled for **Option 1 — Support Ticket Management** so prompts have context from day one. Update them as you refine requirements in the planning chats.

If you want, I can run **Prompt 1** with you next and start building the actual app.

