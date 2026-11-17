# PCRepairHub

**PCRepairHub** е уеб приложение, разработено на **ASP.NET Core MVC (.NET 8)**, предназначено за управление на заявки за компютърни ремонти, диагностика и профилактика. Проектът е реализиран по **трислойна архитектура (3-Layer Architecture)** с интегрирана автентикация и ролево-базиран контрол на достъпа (RBAC).

---

## Архитектура на проекта

Решението (`PCRepairHub.sln`) е разделено на три независими слоя:

1. **`BusinessLayer`** *(Домейн и бизнес модели)*
   * `Entities/AppUser.cs` – разширява `IdentityUser` с допълнителни данни за потребителите и навигационна колекция към техните заявки.
   * `Entities/Category.cs` – модел за категориите сервизни услуги (напр. хардуер, софтуер, профилактика).
   * `Entities/RepairTicket.cs` – модел за заявка за ремонт, свързан чрез външни ключове с `Client` (`AppUser`) и `Category`.
   * `Enums/RepairStatus.cs` – дефинира етапите на обработка на заявката (`Pending`, `InProgress`, `Completed`, `Cancelled`).

2. **`DataLayer`** *(Достъп до данни и бизнес логика)*
   * `AppDbContext.cs` – контекст на базата данни (Entity Framework Core + ASP.NET Core Identity), конфигуриращ връзките и ограниченията между таблиците.
   * `Migrations/` – миграции за автоматично изграждане на схемата в SQL Server (`PCRepairHubDb`).
   * `DbSeeder.cs` – автоматично създава ролите `Admin` и `User`, както и вграден администраторски акаунт при стартиране на приложението.
   * `Services/` – сървисен слой (`ICategoryService`, `CategoryService`, `IRepairTicketService`, `RepairTicketService`), капсулиращ всички CRUD операции и филтриране според ролята.

3. **`WebApplication`** *(Презентационен MVC слой)*
   * `Controllers/` – `HomeController`, `CategoriesController` и `RepairTicketsController`, защитени с `[Authorize]` и ролеви проверки.
   * `Views/` – Razor изгледи с Bootstrap 5, клиентска и сървърна валидация и динамичен интерфейс според ролята на влезлия потребител.
   * `wwwroot/css/site.css` – персонализирани стилове за таблици, карти и навигация.

---

## Роли и функционалности

* **Нерегистриран потребител (Гост):**
  * Достъп до началната страница с информация за услугите.
  * Възможност за регистрация на нов клиентски профил и вход в системата.
* **Клиент (`User`):**
  * Подаване на нови заявки за ремонт с избор на устройство, категория и описание на проблема.
  * Преглед на списък само със собствените си заявки, техния текущ статус и ориентировъчна цена.
  * Редакция и изтриване на собствените заявки.
* **Администратор (`Admin`):**
  * Пълен преглед на всички подадени заявки от всички клиенти.
  * Промяна на статуса на ремонта и определяне на цена за услугата.
  * Пълно CRUD управление (създаване, редакция, изтриване) на категориите услуги.

---

## Данни за тестов достъп (Администратор)

При първо стартиране `DbSeeder` автоматично регистрира администраторски профил:
* **Имейл:** `admin@pcrepairhub.com`
* **Парола:** `Admin123!`

---

## Технологии
* **Framework:** ASP.NET Core 8.0 MVC
* **ORM & Database:** Entity Framework Core 8.0, Microsoft SQL Server
* **Security:** ASP.NET Core Identity (Role-Based Authorization)
* **Frontend:** Razor Views, Bootstrap 5, Custom CSS3, jQuery Validation

---

## Инструкции за стартиране
1. Клонирайте хранилището и отворете `PCRepairHub.sln` във Visual Studio 2022.
2. Проверете низа за връзка `DefaultConnection` в `WebApplication/appsettings.json` и при нужда го адаптирайте към вашия локален SQL Server инстанс.
3. Отворете **Package Manager Console**, изберете `DataLayer` като Default project и изпълнете:
   ```powershell
   Update-Database