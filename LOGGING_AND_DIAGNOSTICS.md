# Logging and Diagnostics Guide

## Overview

The UK Parliament .NET Library includes comprehensive HTTP request/response logging with Guid-based request tracking to help diagnose API issues and debug integration problems.

## Features

### 1. Guid-Based Request Tracking
Every HTTP request is assigned a unique Guid identifier that appears in the log scope for all related messages. This allows correlation of requests with responses in structured logging systems.

**Example with scope-aware logger (like Serilog):**
```json
{
  "Timestamp": "2025-01-18T12:03:13Z",
  "Level": "Debug",
  "Message": "HTTP REQUEST GET https://committees-api.parliament.uk/api/Committees?take=10",
  "Properties": {
    "RequestId": "ac9f47b8-9a71-47a3-9357-d63874bd7113"
  }
}
```

**With console logger (scope not shown by default):**
```
[Debug] [ParliamentClient] ========== HTTP REQUEST ==========
GET https://committees-api.parliament.uk/api/Committees?take=10
...
```

You can enable scope inclusion in console output:
```csharp
var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .SetMinimumLevel(LogLevel.Debug)
        .AddConsole(options =>
        {
            options.IncludeScopes = true; // Shows RequestId in output
        });
});
```

With scopes enabled:
```
[Debug] [ParliamentClient] [RequestId: ac9f47b8-9a71-47a3-9357-d63874bd7113] ========== HTTP REQUEST ==========
```

### 2. Full Request/Response Logging
- **Request Method and URL**
- **All Request Headers**
- **Request Body** (when `EnableVerboseLogging = true`)
- **Response Status Code**
- **All Response Headers**
- **Response Body** (always shown for errors, optional for success)
- **Request Duration** (in milliseconds)

### 3. Intelligent Log Levels
- **Debug**: Successful requests (2xx, 3xx) when verbose logging is enabled
- **Warning**: Client errors (4xx) with full response body
- **Error**: Server errors (5xx) with full response body

### 4. Error Response Handling
Even when `EnableVerboseLogging = false`, error responses (4xx, 5xx) always include:
- Full response headers
- Complete response body (up to 10,000 characters)
- This ensures diagnostic information is never lost

## Setup

### Basic Console Logging

```csharp
using Microsoft.Extensions.Logging;
using Uk.Parliament;

// Create logger factory
var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .SetMinimumLevel(LogLevel.Debug)
        .AddConsole();
});

var logger = loggerFactory.CreateLogger("ParliamentClient");

// Create client with logging
var client = new ParliamentClient(new ParliamentClientOptions
{
    Logger = logger,
    EnableVerboseLogging = true // Show full request/response bodies
});
```

### xUnit Test Logging

```csharp
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;
using Uk.Parliament.Test; // Contains XUnitLoggerFactory

public class MyTests
{
    private readonly ITestOutputHelper _output;

    public MyTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task TestWithLogging()
    {
        // Create logger that writes to xUnit test output
        var loggerFactory = new XUnitLoggerFactory(_output, LogLevel.Debug);
        var logger = loggerFactory.CreateLogger("ParliamentClient");
        
        var client = new ParliamentClient(new ParliamentClientOptions
        {
            Logger = logger,
            EnableVerboseLogging = true
        });
        
        // All HTTP traffic will appear in test output
        var response = await client.Committees.GetCommitteesAsync(take: 10);
    }
}
```

### ASP.NET Core / Production Logging

```csharp
// In Program.cs
builder.Services.AddSingleton<IParliamentClient>(sp =>
{
    var logger = sp.GetRequiredService<ILogger<ParliamentClient>>();
    
    return new ParliamentClient(new ParliamentClientOptions
    {
        Logger = logger,
        EnableVerboseLogging = false, // Disable in production for performance
        Timeout = TimeSpan.FromSeconds(30)
    });
});

// Optional: Configure Serilog to capture scopes
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext() // Captures scope properties like RequestId
    .WriteTo.Console()
    .WriteTo.File("logs/parliament-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
```

## Log Output Examples

### Successful Request (Debug Level)

**Without scope inclusion (default):**
```
[Debug] [ParliamentClient] ========== HTTP REQUEST ==========
GET https://petition.parliament.uk/petitions.json?take=10
Headers:
  User-Agent: Uk.Parliament.NET/2.0
  Accept: application/json

[Debug] [ParliamentClient] ========== HTTP RESPONSE (234ms) ==========
Status: 200 OK
Headers:
  Content-Type: application/json; charset=utf-8
Body (2453 chars):
{"data":[{"id":"700143",...}]}
```

**With scope inclusion enabled:**
```
[Debug] [ParliamentClient] [RequestId: 7f3d9a2b-1c4e-4a5f-b2d8-9e1a6c3f7b8e] ========== HTTP REQUEST ==========
GET https://petition.parliament.uk/petitions.json?take=10
Headers:
  User-Agent: Uk.Parliament.NET/2.0
  Accept: application/json

[Debug] [ParliamentClient] [RequestId: 7f3d9a2b-1c4e-4a5f-b2d8-9e1a6c3f7b8e] ========== HTTP RESPONSE (234ms) ==========
Status: 200 OK
...
```

**With structured logging (Serilog JSON):**
```json
{
  "Timestamp": "2025-01-18T10:15:30.1234567Z",
  "Level": "Debug",
  "MessageTemplate": "{RequestLog}",
  "Properties": {
    "RequestId": "7f3d9a2b-1c4e-4a5f-b2d8-9e1a6c3f7b8e",
    "RequestLog": "========== HTTP REQUEST ==========\nGET https://petition.parliament.uk/petitions.json?take=10\n...",
    "SourceContext": "ParliamentClient"
  }
}
```

### Server Error (Error Level)

**Default console output:**
```
[Error] [ParliamentClient] ========== HTTP RESPONSE (3767ms) ==========
Status: 500 Internal Server Error
Headers:
  Server: cloudflare
  CF-RAY: 9a078df1aa307798-LHR
Content Headers:
  Content-Type: text/plain; charset=UTF-8
  Content-Length: 15
Body (15 chars):
error code: 500
```

**With structured logging (queryable by RequestId):**
```json
{
  "Timestamp": "2025-01-18T12:03:17Z",
  "Level": "Error",
  "MessageTemplate": "{ResponseLog}",
  "Properties": {
    "RequestId": "ac9f47b8-9a71-47a3-9357-d63874bd7113",
    "ResponseLog": "========== HTTP RESPONSE (3767ms) ==========\nStatus: 500 Internal Server Error\n...",
    "SourceContext": "ParliamentClient"
  }
}
```

## Configuration Options

### ParliamentClientOptions Properties

| Property | Default | Description |
|----------|---------|-------------|
| `Logger` | `null` | ILogger instance for HTTP diagnostics |
| `EnableVerboseLogging` | `true` (DEBUG)<br>`false` (RELEASE) | Show full request/response bodies |

### Recommendations

**Development (with scope visibility):**
```csharp
var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .SetMinimumLevel(LogLevel.Debug)
        .AddConsole(options =>
        {
            options.IncludeScopes = true; // Show RequestId in console output
        });
});

var logger = loggerFactory.CreateLogger("ParliamentClient");

var options = new ParliamentClientOptions
{
    Logger = logger,
    EnableVerboseLogging = true,
    EnableDebugValidation = true
};
```

**Production (structured logging):**
```csharp
// Using Serilog for structured logs
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext() // Captures RequestId from scope
    .WriteTo.Console()
    .WriteTo.ApplicationInsights(telemetryConfiguration, TelemetryConverter.Traces)
    .CreateLogger();

var options = new ParliamentClientOptions
{
    Logger = logger,
    EnableVerboseLogging = false, // Still logs errors with full details
    EnableDebugValidation = false
};
```

**Testing (xUnit with scope support):**
```csharp
public class MyTests
{
    private readonly ITestOutputHelper _output;

    public MyTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task TestWithLogging()
    {
        var loggerFactory = new XUnitLoggerFactory(_output, LogLevel.Debug);
        var logger = loggerFactory.CreateLogger("ParliamentClient");
        
        var options = new ParliamentClientOptions
        {
            Logger = logger,
            EnableVerboseLogging = true,
            EnableDebugValidation = false // Committees API has unmapped properties
        };
        
        var client = new ParliamentClient(options);
        
        // RequestId automatically included in test output if XUnitLogger supports scopes
        var response = await client.Committees.GetCommitteesAsync(take: 10);
    }
}
```

## Analyzing Logs

### Finding Specific Requests

**With structured logging (recommended):**
Use the RequestId property to query logs:
```csharp
// Serilog query
Log.Filter.ByIncludingOnly(
    e => e.Properties.ContainsKey("RequestId") && 
         e.Properties["RequestId"].ToString().Contains("ac9f47b8"))
```

**With text-based logs (scope inclusion enabled):**
```bash
grep "ac9f47b8-9a71-47a3-9357-d63874bd7113" log.txt
```

**In Application Insights / Azure Monitor:**
```kusto
traces
| where customDimensions.RequestId == "ac9f47b8-9a71-47a3-9357-d63874bd7113"
| order by timestamp asc
```

### Performance Analysis
Check response times in the response log line:
```
========== HTTP RESPONSE (3767ms) ==========
```

Or query structured logs:
```csharp
// Find slow requests (>2 seconds)
traces
| where message contains "HTTP RESPONSE"
| extend duration = extract(@"(\d+)ms", 1, message)
| where toint(duration) > 2000
| project timestamp, customDimensions.RequestId, duration
