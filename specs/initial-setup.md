# Initial Project Setup

## Project Overview
Gym Tracker — a gamified gym workout tracker built for the MSA 2026 Phase 2 Software Stream assessment.

Users can log workouts (exercises, sets, weight, reps), view their gym history on a calendar, and earn XP, streaks, and badges for consistency and hitting personal records.

## Theme — Gamification
Gamification is applied through:
- **XP system** — users earn XP for each workout logged
- **Streaks** — consecutive gym days tracked and displayed on a calendar
- **Badges / Achievements** — awarded for milestones (e.g. first workout, 7-day streak, new personal record)
- **Personal Records** — the app detects and highlights when a user beats their best weight or reps on an exercise

## Tech Stack

**Frontend**
- React + TypeScript (Vite)
- React Router for navigation
- Redux for state management

**Backend**
- C# with .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQLite (development)

**Infrastructure**
- Docker + Docker Compose
- GitHub Actions (CI)

## Advanced Features Chosen
1. Docker — containerise frontend, backend, and database
2. State management — Redux for global workout and user state
3. End-to-end testing — Cypress

## Initial Folder Structure
```
gymquest/
├── backend/
│   ├── Controllers/
│   │   └── WorkoutsController.cs
│   ├── Data/
│   │   └── AppDbContext.cs
│   ├── Models/
│   │   ├── Workout.cs
│   │   ├── Exercise.cs
│   │   └── Set.cs
│   └── Program.cs
├── frontend/
│   └── src/
│       ├── pages/
│       │   ├── WorkoutLog.tsx
│       │   └── Calendar.tsx
│       ├── api.ts
│       ├── types.ts
│       └── App.tsx
├── specs/
│   └── initial-setup.md
└── README.md
```

## AI Usage — Initial Setup
**Tool used:** Claude (claude.ai)

**Prompts used during this phase:**

- "Given these MSA Phase 2 requirements, how do I set up the project from scratch?"
- "What about a gym tracker — logs gym workouts including exercises, weight and reps, also has a calendar for days they've been to the gym"
- "How can I set this up on Mac?"
- "I want a simple structure like the MSA demo first"
- "I don't want any code files, just the basic structure and empty files"
- "Should I have gitignores in both frontend and backend folders or one gitignore in the root?"
- "Given the /specs requirements for this project, make me an .md file for the initial commit"

**How AI was used:**
Claude was used to plan the initial project structure, decide on tech stack, choose appropriate advanced features, and generate boilerplate configuration files (`.gitignore`). All AI output was reviewed and adapted to fit the project's specific needs before being committed.

## Decisions Made
- **SQLite for dev** — no database setup required locally, keeps onboarding simple
- **Redux for state** — widely used in industry, good to have on a portfolio project
- **Cypress for e2e** — tests real user flows end-to-end across frontend and backend
- **One root `.gitignore`** — covers both frontend and backend to keep the repo clean