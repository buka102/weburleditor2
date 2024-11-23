

The application is a URL shortening service. It provides functionality to shorten long URLs and resolve shortened URLs back to their original form.  
Functionality:

Shorten URL: Takes a long URL and returns a shortened version.
Resolve URL: Takes a shortened URL key and returns the original long URL.
T
ypes of Input:

Shorten URL:  
HTTP Method: POST
Endpoint: /api/shorten
Content-Type: application/json
Request Body: JSON object containing the original URL.
```
{
"originalUrl": "https://www.example.com"
}
```

Resolve URL: 

HTTP Method: GET
Endpoint: /api/resolve/{shortUrlKey}
Path Parameter: shortUrlKey (the key representing the shortened URL)
Example curl Commands:
Shorten URL:  
curl -X POST "http://localhost:5243/api/shorten" -H "Content-Type: application/json" -d '{"originalUrl": "https://www.example.com"}'
Resolve URL:
curl -X GET "http://localhost:5243/api/resolve/{shortUrlKey}"

# Steps to Run the Application

Follow these steps to run your application after submitting the code to GitHub:

1. **Clone the Repository**:
   Clone your GitHub repository to your local machine.
   ```sh
   git clone https://github.com/buka102/WebUrlEditor.git
   cd WebUrlEditor
   ```

## Configure the Application
update your launchSettings.json:

```
{
  "$schema": "http://json.schemastore.org/launchsettings.json",
  "profiles": {
    "http": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger",
      "applicationUrl": "http://localhost:5243",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "Kestrel:Endpoints:Http:Url": "5243"
      }
    },
    "https": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger",
      "applicationUrl": "https://localhost:7128;http://localhost:5243",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "Kestrel:Endpoints:Https:Url": "7128"
      }
    }
  }
}
```

## Restore Dependencies

Restore the project dependencies using the .NET CLI:
```
dotnet restore

dotnet build

dotnet run
``` 
Access the Application: Open your browser and navigate to the URLs specified in your launchSettings.json (e.g., http://localhost:5243 or https://localhost:7128).
he browser will display Swagger documentation, and it can be run in the browser.

Another options use curl:
```
curl -X GET "http://localhost:5243/api/resolve/{shortUrlKey}"

curl -X POST "http://localhost:5243/api/shorten" -H "Content-Type: application/json" -d '{"originalUrl": "https://www.example.com"}'
```
Or course there is Postman.

Real-world solution will been some enhancements, like:
# Scaling the URL Shortener Service

## Current Solution
The current solution uses an in-memory `ConcurrentDictionary` to store URL mappings. This is suitable for small-scale applications but has limitations in real-world scenarios.

## Scaling Suggestions

### * Caching
- Use Redis for caching

### *. Database Storage
- Use a sql or nosql database for persistence and scalability.
- Store original URLs and shortened keys in a database table.

### *. Microservices Architecture
- Keep the application small, independent services that can be developed, deployed, and scaled independently.

### * Add Authorization and Authentication
- Add authorization and authentication to secure the service.
- Use OAuth2 or JWT for authentication.

### *. Monitoring and Logging
- Better monitoring and logging to track performance, detect issues, and analyze usage patterns.

### UrlShortenerService with Database and Redis Cache
