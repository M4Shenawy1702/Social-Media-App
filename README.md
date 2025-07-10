# Link Up 👥💬

**Link Up** is a Fullstack Social Media Web Application that allows users to connect, post updates, and chat in real-time.

---

## 📸 Demo

(https://www.linkedin.com/posts/m4elshenawy_fullstackdevelopment-aspnetcore-angular-activity-7349048003158016000-Ic0s?utm_source=share&utm_medium=member_desktop&rcm=ACoAADYFfNQBcoASD-7FaKqAcqVJpD7ohQ_83QY)

---

## 🚀 Features

- ✅ **Authentication & Profile Management**
  - Register, Login, Edit Profile, Delete Account

- 📝 **Posts System**
  - Full CRUD for Posts, Comments, and Likes

- 🤝 **Friendship System**
  - Send, Accept, Reject, and Remove Friend Requests

- 💬 **Real-time Chat**
  - One-to-one chat between friends using SignalR (WebSockets)

---

## 🛠️ Tech Stack

### Frontend
- [Angular](https://angular.io/)
- [Bootstrap](https://getbootstrap.com/)

### Backend
- [ASP.NET Core Web API](https://dotnet.microsoft.com/)
- [SignalR](https://learn.microsoft.com/en-us/aspnet/core/signalr/introduction)

### Architecture & Patterns
- Repository Pattern
- Unit of Work
- Specification Pattern

### Authentication
- JWT (JSON Web Tokens)

---

## 📂 Project Structure

\`\`\`
/ClientApp        → Angular frontend
/Api            → API Backend
\`\`\`

---

## ⚙️ Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [Node.js & npm](https://nodejs.org/)
- [Angular CLI](https://angular.io/cli)
- SQL Server or PostgreSQL (based on your config)

### Backend Setup

\`\`\`bash
cd API
dotnet restore
dotnet ef database update
dotnet run
\`\`\`

### Frontend Setup

\`\`\`bash
cd Client
npm install
ng serve
\`\`\`

> The frontend will run at \`http://localhost:4200\`  
> The backend API will run at \`http://localhost:5043\`

---

## 🧪 Future Improvements

- Group Chats
- Reactions to Posts
- Notifications system
- File Uploads (Images & Media)
- Responsive design for mobile devices

---

## 📎 Project Link

🔗 [GitHub Repository](https://github.com/M4Shenawy1702/Social-Media-App.git)

---

## 🤝 Contributing

Contributions are welcome!  
If you find any bugs or have ideas for new features, feel free to open an issue or submit a pull request.

---

## 📧 Contact

For questions or collaboration, feel free to reach out:  
💼 [LinkedIn](https://www.linkedin.com/in/m4elshenawy/)  
📩 Or message me directly via GitHub.

---

## 📜 License

This project is licensed under the [MIT License](LICENSE).
EOF
