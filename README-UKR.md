# Інтерактивний розклад UAD (АПІ для розкладів)

![Status](https://img.shields.io/badge/status-active-47c219.svg)
![Made in Ukraine](https://img.shields.io/badge/made_in-Ukraine-ffd700.svg?labelColor=0057b7)

**Інтерактивний розклад UAD** - це веб-сервіс, створений для спрощення управління розкладами студентів. Спочатку розроблявся як бекенд для [Interactive Schedule UAD](https://github.com/Seagullie/InteractiveScheduleUAD) React Native додатку, але в майбутньому може стати самостійним продуктом.

## Можливості

- 💾 Підтримка баз даних
  - ✔️ PostgreSQL
  - ❌ MongoDB
  - ❌ Redis (кеш?)
- 🐳 Підтримка Docker
  - ✔️ Локальна розробка з локальною базою даних (compose, автоматично)
  - ✔️ GitHub Codespaces з власною базою даних (створюється автоматично)
  - ✔️ Хмарне розгортання з хмарною базою даних (Dockerfile, вручну)
- 🔐 JWT
  - ✔️ Аутентифікація за допомогою ролей
  - ✔️ Оновлення токенів
  - ✔️ Вихід з системи
- 💼 Бізнес-логіка
  - ✔️ Маппінг (Mapperly)
  - ✔️ Монада результатів (поки що `FluentResults`, але буде змінено на `OneOf`)
  - 🔜 Валідація (FluentValidation)
  - ❌ Запуск без підключення до бази даних

## Початок роботи

### Використання GitHub Codespaces (найпростіший спосіб протестувати):

1. Запустіть свій Codespace з цього репозиторію (додаткового налаштування не потрібно).

### Запуск локально в Visual Studio:

1. Створіть власний файл `.env` (приклад наведено у файлі `.env.example`).
2. Запустіть проект Docker Compose.

### Запуск на хмарі або віртуальній машині (VM):

Якщо у вас є доступ до консолі ВМ:

1. Створіть власний файл `.env` (приклад наведено у файлі `.env.example`).
2. Виконайте команду `docker compose` в консолі (переконайтеся, що ви встановили Docker, перш ніж продовжувати).

Якщо у вас є можливість використовувати тільки Docker-файли:

1. Знайдіть провайдера бази даних.
2. Вручну введіть всі змінні оточення (з файлу `.env`).
3. Вкажіть шлях до Docker-файлу.

## ER-діаграма

> Ця структура даних виглядає не найкраще, оскільки автоматично генерується з коду. Планується пізніше переписати структуру вручну, використовуючи SQL.

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
