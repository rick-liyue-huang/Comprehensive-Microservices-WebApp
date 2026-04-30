# LoginRequest.cs

```csharp
namespace UsersMicroservice.Core.Dtos;

public record LoginRequest(
    string? Email,
    string? Password
);
```
