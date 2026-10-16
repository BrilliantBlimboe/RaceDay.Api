<img width="1365" height="767" alt="Screenshot 2026-10-16 162217" src="https://github.com/user-attachments/assets/51cfc9e8-ff47-4934-9621-606c7269b4ce" />
<img width="1365" height="763" alt="Screenshot 2026-10-16 163943" src="https://github.com/user-attachments/assets/4a0269d7-1c26-4cc4-b41d-4e803b568204" />

# RaceDay.Api

RESTful API for managing race events, enrolments, and results. Built with ASP.NET Core and Entity Framework.

## 🚀 Setup Instructions

### Prerequisites
- .NET 8 SDK
- SQL Server (or LocalDB)
- Visual Studio 2022

### Installation
1. Clone the repository
```bash
git clone https://github.com/BrilliantBlimboe/RaceDay.Api.gitRestore packagesbashdotnet restoreConfigure Database
Update appsettings.json:json"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=RaceDayDb;Trusted_Connection=True;"
}Apply Migrationsbashdotnet ef database updateRun the APIbashdotnet runAPI will run at: https://localhost:5001 and http://localhost:5000
Swagger UI: https://localhost:5001/swagger
Branches
master - Part 1 submissionpart2-enrolments - Part 2 with enrolment fix (Status Active, 201 Created)👥 User Roles
1. Participant (Athlete / User)
Purpose: Regular user who joins races.
Permissions:
Register and loginView all available race events GET /api/eventsEnrol in an event POST /api/events/{id}/enrolmentsView own enrolments GET /api/enrolments/meView own results and profileCannot create or delete eventsEnrolment Flow:
When a Participant enrols, status is set to Active and returns 201 Created. PaymentStatus remains Pending until payment.
2. Organizer (Admin / Event Manager)
Purpose: Manages races and participants.
Permissions:
All Participant permissionsCreate new race events POST /api/eventsUpdate event details PUT /api/events/{id}Delete events DELETE /api/events/{id}View all enrolments for an event GET /api/events/{id}/enrolmentsManage results and approve enrolmentsAccess to admin dashboard📸 Proof of Fix - Part 2
Enrolment now correctly uses Status = "Active" (DB only allows DNS, Cancelled, Active) and returns HTTP 201.
![Enrolment Controller Fix]
Technologies
ASP.NET Core 8 Web APIEntity Framework CoreJWT AuthenticationSQL ServerAuthor
BrilliantBlimboe - CTU Formative Assessment<img width="1365" height="767" alt="Screenshot 2026-10-16 153155" src="https://github.com/user-attachments/assets/92da7808-0396-46b1-a993-e0a2ca6ed02a" />
<img width="1365" height="767" alt="Screenshot 2026-10-16 153125" src="https://github.com/user-attachments/assets/8976b997-e5ac-4a0b-887f-a176039e8a69" />


<img width="1365" height="767" alt="Screenshot 2026-10-16 162217" src="https://github.com/user-attachments/assets/4cb3dee7-8bdd-4f7c-9057-cde77704d260" />
# RaceDay.Api
