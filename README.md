# Artify – Digital Art Marketplace

**Artify** is a premium digital marketplace that connects artists with buyers and agencies through a secure and intelligent platform. It provides a seamless experience for discovering, buying, and selling original artworks while protecting artist originality.

---

## 🌟 Key Features

### Marketplace & Frontend
- **Browsing**: Explore artworks with robust filters (Category, Price, Rating).
- **Search**: Find specific artists or artworks instantly.
- **Roles**: Dedicated dashboards for **Artists** (Upload, Stats), **Buyers** (Orders, Wishlist), **Agencies**, and **Admins**.
- **Security**: Protected routes and role-based access control.
- **UI/UX**: Modern, responsive design with animations and dark/light accents.

### Backend & Logic
- **User Management**: JWT-based registration and login.
- **Moderation**: Admin approval workflows for Artists and Artworks.
- **Artist Integrity**: Watermarking and hashing logic to protect uploads.
- **Plagiarism Check**: Automated originality checks upon upload to flag potential plagiarism.
- **Data**: Secure SQL Server integration for robust data management.

---

## 🛠 Technology Stack

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

## 🚀 Quick Start (Recommended)

To quickly start the entire application stack (both Frontend and Backend):

1. **Run the startup script**:
   Double click the `start-app.bat` file in the root directory, or run it via terminal:
   ```powershell
   ./start-app.bat
   ```
2. The script will automatically launch:
   - The ASP.NET Core Backend API (`https://localhost:7294/swagger`)
   - The Vite Frontend Server (`http://localhost:5173`)

---

## 💻 Manual Installation & Setup

### Prerequisites
- **Node.js** (v18+)
- **.NET SDK** (v8)
- **SQL Server**

### 1. Backend Setup
1. Open the solution `Artify.sln` in **Visual Studio** or your preferred IDE.
2. Update the connection string in `Artify.Api/appsettings.json` to point to your local SQL Server instance.
3. Apply Entity Framework Migrations to create the database:
   ```powershell
   dotnet ef database update --project Artify.Api
   ```
4. Start the backend API. It will run on `https://localhost:7294`.

### 2. Frontend Setup
1. Navigate to the frontend directory:
   ```bash
   cd Artifyfrontend
   ```
2. Install dependencies:
   ```bash
   npm install
   ```
3. Start the development server:
   ```bash
   npm run dev
   ```
4. Open `http://localhost:5173` in your browser.

---

## 📖 Usage Instructions & Roles

Artify operates using a robust Role-Based Access Control (RBAC) system. Depending on the account type, the user experience completely changes:

### 1. Admin
- **Access**: Log in via the `/admin` portal.
- **Features**: 
  - Manage and monitor users (Block/Unblock, Approve pending Artists).
  - Review uploaded artworks in the **Approvals** tab.
  - View flagged artworks that trigger the "Potential Plagiarism" warning.
  - Review global platform financials and moderation reports.

### 2. Artist
- **Access**: Standard registration, select "Artist" role.
- **Features**: 
  - All new artists and uploads start as **Pending** and must be approved by an Admin.
  - Upload artworks securely (automatically hashed and watermarked by the server).
  - Browse the **Job Feed** (Project Board) to submit proposals for custom gigs.
  - View real-time sales and order stats on the Dashboard.

### 3. Buyer
- **Access**: Standard registration, select "Buyer" role.
- **Features**: 
  - Browse the marketplace and purchase artworks using the secure Escrow system.
  - **Post a Project** to the Job Feed to hire artists for custom artwork.
  - Real-time chat with artists to negotiate and monitor custom project milestones.

---

## 🤝 Contribution Guidelines

We welcome contributions!
1. **Fork** the repository.
2. **Branch** for your feature (`git checkout -b feature/NewFeature`).
3. **Commit** your changes.
4. **Push** to your branch.
5. **Open a Pull Request**.

---

## 📬 Team & Contact

For inquiries, support, or collaboration, please contact the development team:

*   **Athar Ali**: [atharalishigri41@gmail.com](mailto:atharalishigri41@gmail.com)
*   **Hassanain Abbas**: [hsnabbas40@gmail.com](mailto:hsnabbas40@gmail.com)
*   **Yahya Safi**: [ismailsafi7736@gmail.com](mailto:ismailsafi7736@gmail.com)

---

&copy; 2026 Artify. All rights reserved.
