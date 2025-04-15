# .NET Web API with Redis Caching and Dynamic UI

This project demonstrates a .NET Web API that fetches data from JSONPlaceholder API, implements both Redis and in-memory caching, and includes a dynamic frontend with optimized image loading.

## Prerequisites

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) or newer
- [Redis](https://redis.io/download) (local installation or cloud service)
- Web browser with JavaScript enabled

## Setup Instructions

### 1. Clone the Repository

```bash
git clone https://github.com/yourusername/dotnet-redis-caching-demo.git
cd dotnet-redis-caching-demo
```

### 2. Redis Setup

#### Option A: Local Redis Installation

- **Windows**: 
  - Download and install [Redis for Windows](https://github.com/tporadowski/redis/releases)
  - Start Redis Server

- **macOS**:
  ```bash
  brew install redis
  brew services start redis
  ```

- **Linux (Ubuntu/Debian)**:
  ```bash
  sudo apt update
  sudo apt install redis-server
  sudo systemctl start redis-server
  ```

#### Option B: Redis Cloud Service

- Create a free account on [Redis Labs](https://redis.com/try-free/)
- Create a database instance
- Note the connection string (endpoint and password)

### 3. Configure Application

1. Update `appsettings.json` with your Redis connection string:

```json
{
  "ConnectionStrings": {
    "Redis": "localhost:6379"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

> For Redis Labs, use format: `yourinstance.redis.cache.windows.net:6380,password=yourpassword,ssl=true,abortConnect=false`

### 4. Build and Run the Application

```bash
dotnet restore
dotnet build
dotnet run
```

The application will start and listen on:
- https://localhost:7001
- http://localhost:5001

(Your actual ports may vary, check the console output)

## Usage

1. Navigate to `http://localhost:5001` (or the port shown in the console)
2. The application will load users from JSONPlaceholder API with caching
3. Use the search box to filter users
4. Click the refresh button to reload data

## Features

- **Redis Caching**: Fetches data from an external API and caches it in Redis
- **In-Memory Caching**: Implements IMemoryCache for faster access to frequently used data
- **Dynamic UI**: Updates the UI without page reload using Fetch API
- **Image Optimization**: Uses lazy loading for images and WebP format when supported

## Project Structure

- `Program.cs`: Application entry point and service configuration
- `Controllers/UsersController.cs`: API endpoints for user data
- `Services/`:
  - `RedisCacheService.cs`: Redis caching implementation
  - `MemoryCacheService.cs`: In-memory caching implementation
  - `HybridCacheService.cs`: Combined caching strategy
  - `UserService.cs`: Service to fetch and manage user data
- `Models/`: Data models for the application
- `wwwroot/`: Static files for the frontend
  - `index.html`: Main page
  - `js/users.js`: Dynamic UI JavaScript code

## API Endpoints

- `GET /api/users`: Returns a list of users (cached)
- `GET /api/users/clear-cache`: Clears the user cache (for testing)

## Troubleshooting

### Redis Connection Issues

- Ensure Redis is running:
  ```bash
  redis-cli ping
  ```
  Should return `PONG`

- Check Redis connection with:
  ```bash
  redis-cli
  > keys *
  ```

### API Not Responding

- Check if JSONPlaceholder API is accessible:
  ```bash
  curl https://jsonplaceholder.typicode.com/users
  ```

- Verify network connectivity and firewall settings

### Images Not Loading

- Clear browser cache
- Check browser console for errors
- Verify Picsum service is accessible

## License

MIT
