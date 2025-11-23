# AccentInt:

The solution is in a form of an API which has 3 endpoints for the 3 required tasks:

1. POST /api/Holidays/three
2. POST /api/Holidays/count
3. POST /api/Holidays/common

The request model is generic for the 3 endpoints.
Year is optional for /api/Holidays/three
The solution is made generic, thus you can use more than 2 countries for /api/Holidays/common, but at least 2

{
"Year": 2021,
"Countries": ["NL", "AD", "AL", "AM", "AT", "AU", "RO"]
}

# How to run:

1. For windows - install visual studio including .NET 8 and open the solution.
   The solution uses aspire which opens a dashboard with telemetry and logs.
   You can find the URLs to access the API with or without https in the opened page.
2. For Mac - you can build the docker image with: "docker build -f AccentInt.ApiService/Dockerfile -t accentint.api ." in the root folder (where sln is)
   And run with: "docker run --rm -p 8080:8080 accentint.api"
   You can use "http://localhost:8080/api/Holidays/common"
   ! On windows you can also build&run using docker, but it's nice to see the dashboard in case any logs are displayed.

# Considerations:

- I created the project using clean architecture in mind.
- I used domain layer to hold all my models and encapsulate state based on requirements. This is very helpful when business requires a more complex approach and use DDD for example, to hold aggregates or value objects. It has no dependencies, only c# code.
- Application layer expresses the 3 use cases from requirements. It has a dependency on Domain and its main job is to fulfill business requirements. No external dependency.
- Infrastructure layer is the projects which holds the actual implementation. In our case I used IMemoryCache - for caching results and provide quicker answers and HttpClient - for external API calls.
- ApiService - the actual API which calls Application layer to return user's request.
- A noticeable aspect is the time response between first and second call when country codes are cached.
- The solution uses multi-threading to run threads in parallel and get holidays for each country code. The solution is also thread safe.
- HttpClientFactory is used under the hood when using typed clients.

# Improvements:

- There are lots of improvements that can be done of course, I would start right away with a better error handling and adding more custom exception when bad state happens.
- Many more but requires of course a discussion and a proper analysis.
