# HNG_Stage_0

Lightweight ASP.NET Core Web API that classifies a person's likely gender using the external `genderize.io` API.

## Project overview

- Project: `HNG_Stage_0` (single ASP.NET Core Web API project)
- Target framework: .NET 10
- C# language version: 14
- Main purpose: expose a simple `GET /api/classify` endpoint that returns a standardized `BaseResponse<T>` with prediction data.

## Key files

- `Program.cs` - app startup, DI, CORS and Swagger configuration
- `Controllers/ClassifyController.cs` - API endpoint `GET /api/classify?name={name}`
- `Service/GenderizeService.cs` - calls `https://api.genderize.io` using `HttpClient` and maps results
- `Models/GenderizeDto.cs` - DTOs for incoming and outgoing data
- `Models/BaseResponse.cs` - generic response wrapper used across the API
- `Interfaces/IGenderizeService.cs` - service contract for the classification service

## How it works

1. Client calls `GET /api/classify?name=alice`.
2. Controller validates the `name` query parameter and delegates to the registered `IGenderizeService`.
3. `GenderizeService` calls `https://api.genderize.io?name={name}`, deserializes the response, and builds a `BaseResponse<GenderizeResponseDto>`.
4. API returns JSON with `Status`, optional `Message`, and `Data` when available.

## Example request

curl:

```
curl "https://localhost:5001/api/classify?name=alice"
```

Example successful response body:

```
{
  "status": "success",
  "data": {
    "name": "alice",
    "gender": "female",
    "probability": 0.98,
    "sample_size": 12345,
    "is_confident": true,
    "processed_at": "2026-04-14 12:34:56Z"
  }
}
```

Example error response (missing name):

```
{
  "status": "error",
  "message": "Name parameter is required"
}
```

## Running locally

Requirements:
- .NET 10 SDK
- (Optional) Visual Studio 2026

From the repository root:

- Using dotnet CLI:

```
cd HNG_Stage_0
dotnet run
```

- Using Visual Studio: open the project/solution and run (F5 or Ctrl+F5).

Swagger UI will be available at `/swagger` when the app is running.

## Configuration & notes

- `HttpClient` for `IGenderizeService` is registered in `Program.cs`.
- CORS policy `AllowAll` is enabled for development convenience.
- The service marks predictions as confident when `Probability >= 0.7` and `Count >= 100`.
- The project does not include unit tests by default.


## License

This repository contains example/demo code — choose and add a license if you intend to distribute it.
