# 🥗 Nutrition App Backend

A robust Backend API for the Nutrition App, providing AI-powered food analysis, meal logging, and comprehensive health progress tracking.

---

## 🏗 Architecture: Vertical Slice Architecture (VSA)

This project is built using **Vertical Slice Architecture**, moving away from traditional layered constraints to focus on features as independent pillars.

- **Feature-Centric Logic:** Business logic is encapsulated within **Services inside each Feature**. This ensures that every "slice" of the system contains its own specific rules and behavior, making the codebase highly maintainable.
- **Core Domain:** The `Domains` layer is kept lean, acting as the source of truth for **Core Entities** and shared definitions across the system.
- **Scalability:** By isolating features, the system allows for rapid development and easy modification of specific functionalities without side effects on other parts of the application.

---

## 🚀 Key Features

- **🤖 AI-Powered Food Analysis:** Leverages AI to analyze food images/descriptions and extract precise nutritional data.
- **📊 Meal Management:** Comprehensive system for logging daily intake and managing meal history.
- **📈 Progress Tracking:** Detailed analytics to monitor health trends and nutritional goals.
- **🛡️ Global Exception Handling:** A centralized `ExceptionHandler` ensuring consistent, secure, and clean API responses.
- **🐳 DevOps Ready:** Fully containerized using Docker with Nginx as a Reverse Proxy for production-grade stability.

---

## 🛠 Tech Stack

- **Framework:** .NET (C#)
- **Architecture:** Vertical Slice Architecture (VSA)
- **Persistence:** Entity Framework Core
- **API Documentation:** Swagger / OpenAPI
- **Containerization:** Docker & Docker Compose
- **Reverse Proxy:** Nginx
- **AI Integration:** LLM-based food recognition and nutritional analysis.

---

## 📁 Project Structure

```text
├── Domains/            # Core Entities (Shared models and business objects)
├── Features/           # Vertical Slices: Contains Controllers, Services (Business Logic), and DTOs per feature
├── Datas/              # Data Access Layer, DbContext, and Migrations
├── ExceptionHandler/   # Global error handling middleware
├── nutritionapp/       # Host project & API Configuration
├── nginx.conf          # Nginx Reverse Proxy configuration
└── docker-compose.yml  # Multi-container orchestration (App + Web Server)
```

---

## ⚙️ Getting Started

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

### Installation & Run

1. **Clone the repository:**
   ```bash
   git clone https://github.com/tquocan04/nutritionapp-backend.git
   cd nutritionapp-backend
   ```

2. **Launch with Docker:**
   ```bash
   docker-compose up -d
   ```

3. **Explore the API:**
  Open your browser and navigate to http://localhost:8080 (or your configured port) to view the Swagger API documentation.

---

<p align="center"> <i>Empowering healthy lifestyles through intelligent backend solutions 🚀</i> </p>
