# Support Ticket Management System

**.NET AI Capability Exercise** — Option 1 (Backend-Heavy), Core submission.

A full-stack app for managing support tickets with an enforced status state machine, comments, search/filter, and integration tests.

## Tech Stack

- **Backend:** ASP.NET Core 8 Web API
- **Frontend:** React + TypeScript (Vite)
- **Database:** PostgreSQL + EF Core
- **Tests:** xUnit integration tests

## Quick Start

> _Complete these steps after running the implementation prompts from `ai-prompts/implementation.md`._

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)
- [PostgreSQL 14+](https://www.postgresql.org/)

### 1. Clone and configure

```bash
git clone <your-repo-url>
cd ai-practical-assessment
cp .env.example .env   # Edit with your DB credentials
```

### 2. Database setup

See [`database/setup-notes.md`](database/setup-notes.md) for full instructions.

```bash
cd src/[ApiProject]
dotnet ef database update
dotnet run
```

### 3. Frontend

```bash
cd src/[FrontendProject]
npm install
npm run dev
```

Open `http://localhost:5173` (or your Vite port).

### 4. Run tests

```bash
cd tests
dotnet test
```

## Project Structure

```
ai-practical-assessment/
├── README.md                    # This file
├── candidate-info.md            # Your details
├── tool-workflow.md             # Part A — AI workflow
├── requirements-analysis.md     # Requirements
├── acceptance-criteria.md       # Definition of done
├── implementation-plan.md       # Task plan
├── design-notes.md              # Architecture
├── api-contract.md              # API spec
├── data-model.md                # Database schema
├── ui-flow.md                   # UI flows
├── test-strategy.md             # Test plan
├── test-results.md              # Test run results
├── debugging-notes.md           # Bug investigation log
├── code-review-notes.md         # Review findings
├── review-fixes.md              # Fixes applied
├── pr-description.md            # PR summary
├── reflection.md                # Part C reflection
├── final-ai-usage-summary.md    # AI usage log
├── src/                         # Application code
├── tests/                       # Integration tests
├── database/                    # Migrations & seed data
├── ai-prompts/                  # Copy-paste chat prompts ⭐
└── tool-specific/cursor-workflow/  # Cursor context files
```

## How to Use AI Prompts

**Start here:** [`ai-prompts/`](ai-prompts/)

Each file contains **separate copy-paste prompts** for new Cursor chat sessions:

| File | Phase | Prompts |
|------|-------|---------|
| `planning.md` | Requirements & planning | 3 |
| `design.md` | Architecture & API design | 4 |
| `implementation.md` | Build the app | 5 |
| `testing.md` | Integration tests | 3 |
| `debugging.md` | Fix bugs | 3 |
| `code-review.md` | Review & fixes | 3 |
| `documentation.md` | README & submission | 4 |

**Workflow:**
1. Open the prompt file for your current phase
2. Start a **new Cursor chat**
3. Copy the prompt block into chat
4. `@`-reference the listed files
5. Log the response in the **Response Log** section of that prompt

## Lifecycle Artifacts

All required assessment artifacts are at the repository root. See the [Participant Guide](../Net_AI_Assessment_ProjectGuide.docx.pdf) for full requirements.

## License

Assessment submission — internal use.
