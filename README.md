

#  Artify – Digital Marketplace for Artists

Artify is a digital marketplace that connects artists with buyers and agencies through a secure and intelligent platform. The system focuses on artist visibility, originality protection, and direct hiring or artwork selling.

This repository currently contains the **backend MVP prototype** developed for **FYDP D2**, validating system feasibility and core logic.

---

##  Tech Stack

* **Backend:** ASP.NET Core (C#)
* **Database:** SQL Server
* **Frontend:** React (planned)
* **AI / Protection:** Python (hashing, watermarking – partial)
* **Tools:** Visual Studio, Swagger, Postman, GitHub

---

##  Setup & Run (Local)

1. Clone the repository

   ```bash
   git clone https://github.com/AtharShigri/Artify.git
   ```

2. Open the solution in **Visual Studio**

3. Update SQL Server connection string in `appsettings.json`

4. Apply migrations

   ```bash
   Update-Database
   ```

5. Run the project (F5)

---

##  API Testing

Swagger UI is used as the prototype interface.

```
https://localhost:<port>/swagger
```

---

##  Current Features (MVP)

* User registration & login (JWT-based)
* Role-based authorization (Artist/Admin)
* Artist profile management
* Artwork upload with hashing & watermarking logic
* Plagiarism check API (partial)
* Database integration with SQL Server

---

##  Project Status (D2)

* Backend MVP implemented
* Frontend UI planned for D3
* AI modules partially integrated

---

##  Branching Strategy

* `main` – stable
* `dev` – ongoing development
* `feature/*` – module-wise development

---

##  License

This project is licensed under the **MIT License**.
