## 📘 Intermediate Exercise: Issue Tracker with File Attachments

### Business Context
Build a simple **internal issue tracking system** where teams can log bugs/tasks, track status, and attach screenshots.

---

## 🎯 Core Requirements (3-4 days for experienced intermediate)

### Backend (.NET 8+)
- [ ] REST API with these endpoints:
  - `GET /api/issues` – list issues (with pagination & filtering by status)
  - `GET /api/issues/{id}` – get single issue
  - `POST /api/issues` – create issue (title, description, priority: Low/Medium/High)
  - `PUT /api/issues/{id}` – update issue (status, priority, assignee)
  - `DELETE /api/issues/{id}` – delete issue
  - `POST /api/issues/{id}/attachments` – upload file (max 5MB, images only)
  - `GET /api/issues/{id}/attachments/{fileId}` – download attachment
- [ ] **SQLite or PostgreSQL** with Entity Framework Core (code-first)
- [ ] **Repository pattern** (simplified – one generic or per entity)
- [ ] **AutoMapper** for DTOs
- [ ] **FluentValidation** for request validation
- [ ] **Global exception handling middleware**
- [ ] **File storage** – local disk with structured folders (`/uploads/{issueId}/`)

### Frontend (Angular 17+)
- [ ] **Two main views**:
  - **List view** – table showing title, priority (color-coded), status, assignee
  - **Detail view** – edit issue, change status, view/upload attachments
- [ ] **Reactive forms** with validation (title required, min length 3)
- [ ] **File upload component** with:
  - Preview thumbnails for images
  - Remove attachment button
  - Upload progress indicator
- [ ] **Routing** with parameter for detail view (`/issues/:id`)
- [ ] **Services** for API calls (separate `IssueService`, `AttachmentService`)
- [ ] **Interceptors** for error handling (display toast notifications)
- [ ] **Basic state management** using **Angular Signals** (no NgRx needed)
- [ ] **Responsive CSS** (Flex/Grid, works on tablet/desktop)

### Docker
- [ ] Dockerfile for backend (production-ready)
- [ ] Dockerfile for frontend (nginx serving Angular build)
- [ ] `docker-compose.yml` with:
  - Backend API (ports 5000-5001)
  - Frontend (port 8080)
  - Database (volume persisted)
- [ ] **Volume mount** for uploaded files (so attachments survive container restart)
- [ ] **Environment variables** for connection string (no hardcoding)

---

## 📅 1-Week Schedule (4.5 days work + 0.5 day buffer)

| Day | Focus | Specific Deliverables |
|-----|-------|----------------------|
| **Day 1** | Backend Core | API project, EF Core + SQLite, CRUD endpoints, Swagger working |
| **Day 2** | Backend Advanced | File upload, FluentValidation, middleware, AutoMapper, repository |
| **Day 3** | Angular Setup | Project structure, routing, services, list view with API data |
| **Day 4** | Angular Features | Detail view, reactive form, file upload component, error interceptor |
| **Day 5** | Docker + Polish | Dockerfiles, compose, volume testing, README, final integration |

---

## 🎨 Stretch Goals (If done early)

- [ ] **Search/filter** by title text (contains)
- [ ] **Sorting** on table columns
- [ ] **Basic authentication** (JWT – hardcode one test user)
- [ ] **Delete attachment** endpoint + UI button
- [ ] **Optimistic UI updates** (toggle status without full page reload)
- [ ] **Unit tests** (xUnit for backend, Jasmine for frontend – 2-3 each)

---

## 🛠️ Tech Stack (Specified)

```
Backend:  .NET 8 Web API + EF Core + SQLite/PostgreSQL
Frontend: Angular 18 + ReactiveForms + HttpClient + Signals
Storage:  Local disk (mounted volume in Docker)
Container: Docker + Docker Compose
Testing:  Swagger/Postman for manual, optional xUnit
```

---

## ✅ Success Criteria

Developer can demonstrate:

1. **Run** `docker-compose up --build` → access app at `http://localhost:8080`
2. **Create** an issue with title, description, priority
3. **Upload** 2-3 images to an issue
4. **View** thumbnails in detail view
5. **Update** issue status from "Open" → "In Progress" → "Closed"
6. **Delete** an issue (attachments also deleted from disk)
7. **Restart** containers → issues + attachments still there

---

## 🔧 Common Pitfalls (What to watch for in review)

| Pitfall | Why it matters |
|---------|----------------|
| Files stored inside container without volume | Attachments disappear on restart |
| No file type/size validation | Security risk, easy DoS |
| N+1 queries in EF Core | Performance terrible with 100+ issues |
| CORS misconfigured | Angular can't call API in Docker |
| No loading states | UI feels broken on slow connections |
| Hardcoded connection strings | Won't work in different environments |
| Swagger doesn't support file upload | Hard to test attachment endpoint manually |

---

## 📝 Deliverables Checklist

- [ ] GitHub repo with **clean commits** (no `node_modules` or `bin/obj`)
- [ ] `README.md` with:
  - Tech stack list
  - Setup instructions (both Docker and manual)
  - API documentation table
  - Screenshots of UI
- [ ] `docker-compose.yml` works on **clean machine** (tested)
- [ ] **No hardcoded secrets** (use environment variables)
- [ ] **.gitignore** properly excludes build artifacts

---

## 📊 Evaluation Rubric (For reviewer)

| Criterion | Weight | Pass if... |
|-----------|--------|------------|
| API completeness | 20% | All 7 endpoints work with proper HTTP status codes |
| Data persistence | 15% | Issues + attachments survive container restart |
| File upload | 15% | Images preview, validation works, error handled |
| Angular functionality | 20% | CRUD operations, file upload, routing all work |
| Docker setup | 15% | `docker-compose up` works first time on new machine |
| Code organization | 10% | Clear separation of concerns, no 500-line files |
| Error handling | 5% | User sees friendly messages on failures |

---

## 🆚 How This Differs from Junior Exercise

| Aspect | Junior (Task Manager) | Intermediate (Issue Tracker) |
|--------|----------------------|------------------------------|
| Storage | In-memory (lost on restart) | Database + file system |
| Data relationships | None (one entity) | One-to-many (issue → attachments) |
| API complexity | Basic CRUD | File upload, DTOs, validation |
| Frontend state | Simple array in component | Signals, multiple services |
| File handling | None | Upload, preview, download |
| Docker volumes | Not needed | Required for persistence |
| Error handling | Minimal | Interceptors + user feedback |
