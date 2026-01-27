# Artify – Digital Art Marketplace

**Artify** is a premium digital marketplace that connects artists with buyers and agencies through a secure and intelligent platform. It provides a seamless experience for discovering, buying, and selling original artworks while protecting artist originality.

---

##  Key Features

###  Marketplace & Frontend
- **Browsing**: Explore artworks with robust filters (Category, Price, Rating).
- **Search**: Find specific artists or artworks instantly.
- **Roles**: Dedicated dashboards for **Artists** (Upload, Stats) and **Buyers** (Orders, Wishlist).
- **Security**: Protected routes and role-based access control.
- **UI/UX**: Modern, responsive design with animations and dark/light accents.

###  Backend & Logic
- **User Management**: JWT-based registration and login.
- **Artist Integrity**: Watermarking and hashing logic to protect uploads.
- **Plagiarism Check**: AI-driven modules (partial) to verify originality.
- **Data**: Secure SQL Server integration for robust data management.

---

##  Technology Stack

### Frontend
- **Framework**: React (Vite)
- **Styling**: Tailwind CSS v3
- **State**: React Context API
- **Icons**: Lucide React

### Backend
- **Core**: ASP.NET Core (C#)
- **Database**: SQL Server
- **AI/Tools**: Python (Watermarking/Hashing)
- **API**: Swagger UI

---

## Installation & Setup

### Prerequisites
- **Node.js** (v16+)
- **.NET SDK** (v6/v7/v8 as required)
- **SQL Server**

### 1. Backend Setup
1.  Navigate to the solution root.
2.  Open `Artify.sln` in **Visual Studio**.
3.  Update the connection string in `appsettings.json` to point to your local SQL Server instance.
4.  Run migrations:
    ```powershell
    Update-Database
    ```
5.  Start the backend API (F5). It will run on `https://localhost:<port>`.

    cd Artifyfrontend
    ```
2.  Install dependencies:
    ```bash
    npm install
    ```
3.  Start the development server:
    
    npm run dev
    4.  Open `http://localhost:5173` in your browser.

---

##  Contribution Guidelines

We welcome contributions!
1.  **Fork** the repository.
2.  **Branch** for your feature (`git checkout -b feature/NewFeature`).
3.  **Commit** your changes.
4.  **Push** to your branch.
5.  **Open a Pull Request**.

---

##  Team & Contact

For inquiries, support, or collaboration, please contact the development team:

*   **Athar Ali**: [atharalishigri41@gmail.com](mailto:atharalishigri41@gmail.com)
*   **Hassanain Abbas**: [hsnabbas40@gmail.com](mailto:hsnabbas40@gmail.com)
*   **Yahya Safi**: [ismailsafi7736@gmail.com](mailto:ismailsafi7736@gmail.com)

---

&copy; 2025 Artify. All rights reserved.
