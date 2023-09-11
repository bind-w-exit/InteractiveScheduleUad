# Interactive Schedule UAD

![Status](https://img.shields.io/badge/status-active-47c219.svg)
![Made in Ukraine](https://img.shields.io/badge/made_in-Ukraine-ffd700.svg?labelColor=0057b7)

**Interactive Schedule UAD** is a web service that simplifies the management of student schedules. Originally developed as the backend for the [Interactive Schedule UAD](https://github.com/Seagullie/InteractiveScheduleUAD) React Native app, but it may become a standalone product in the future.

## Features

- 💾 Database Support
  - ✔️ PostgreSQL
  - ❌ MongoDB
  - ❌ Redis (cache?)
- 🐳 Docker Support
  - ✔️ Local development with a local database (compose, automatic)
  - ✔️ GitHub Codespaces with its own database (compose, automatic)
  - ✔️ Cloud deployment with a cloud database (Dockerfile, manual)
- 🔐 JWT
  - ✔️ Authentication with roles
  - ✔️ Refresh tokens
  - ✔️ Logout
- 💼 Business logic
  - ✔️ Mapping (Mapperly)
  - ✔️ Result Monad (FluentResults for now, but I will change it to OneOf)
  - 🔜 Validation (FluentValidation)
  - ❌ Run without a database connection

## Getting Started

### Using GitHub Codespaces (easiest way to test it out):
1. Run own Codespace from this repository (no additional configuration required).

### Running Locally in Visual Studio:
1. Create your own `.env` file (an example is provided in `.env.example`).
2. Run the Docker Compose project.

### Running on a Cloud or Virtual Machine (VM):

If you have access to the VM console:
1. Create your own `.env` file (an example is provided in `.env.example`).
2. Run the `docker compose` command in the console (make sure to install Docker before proceeding).

If you only have the ability to use Dockerfiles:
1. Find a database provider.
2. Manually enter all the environment variables (from the `.env` file).
3. Specify the path to the Dockerfile.

## ER Diagram

> This data structure is so terrible because it is auto-generated from code. Later, I will definitely rewrite it by hand using SQL or create normal model classes 🥲

```mermaid
	erDiagram
		Users{
			Id integer PK
			Username text
			PasswordHash bytea
			PasswordSalt bytea
			UserRole  integer
		}

		StudentsGroups{
			Id integer PK
			GroupName text
			FirstWeekScheduleId integer FK
			SecondWeekScheduleId integer FK
		}
		WeekSchedule{
			Id integer PK
		}
		StudentsGroups }o--|| WeekSchedule : "First week"
		StudentsGroups }o--|| WeekSchedule : "Second week"
		Lesson{
			Id integer PK
			Sequence integer
			SubjectId integer FK
			TeacherId integer FK
			RoomId integer FK
			ClassType integer
			WeekScheduleId integer FK
			WeekScheduleId1 integer FK
			WeekScheduleId2 integer FK
			WeekScheduleId3 integer FK
			WeekScheduleId4 integer FK
			WeekScheduleId5 integer FK
			WeekScheduleId6 integer FK
		}
		WeekSchedule ||--o{ Lesson : Monday
		WeekSchedule ||--o{ Lesson : Tuesday
		WeekSchedule ||--o{ Lesson : Wednesday
		WeekSchedule ||--o{ Lesson : Thursday
		WeekSchedule ||--o{ Lesson : Friday
		WeekSchedule ||--o{ Lesson : Saturday
		WeekSchedule ||--o{ Lesson : Sunday
		Rooms{
			Id integer PK
			Name text
		}
		Lesson ||--o{ Rooms : Room
		Subjects{
			Id integer PK
			Name text
		}
		Lesson ||--o{ Subjects : Subject
		Teachers{
			Id integer PK
			FirstName text
			LastName text
			MiddleName text
			Email text
			Qualifications text
			DepartmentId integer FK
		}
		Lesson ||--o{ Teachers : Teacher
		Departments{
			Id integer PK
			Name text
			Abbreviation text
			Link text
		}
		Teachers ||--o{ Departments : Department

		Articles{
			Id integer PK
			Title character_varying(200)
			Body character_varying(400)
			Published timestamp_with_time_zone
			AuthorId integer FK
		}
		Authors{
			Id  integer PK
			FirstName text
			LastName text
			NickName text
			Email text
			Bio text
		}
		Articles }o--|| Authors : Author
```
