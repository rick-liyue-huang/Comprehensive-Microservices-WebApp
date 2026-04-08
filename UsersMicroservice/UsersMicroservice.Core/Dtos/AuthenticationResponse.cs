namespace UsersMicroservice.Core.Dtos;

public record AuthenticationResponse(
    Guid UserId,
    string? Email,
    string? PersonName,
    string? Password,
    string? Gender,
    string? Token,
    bool Success
);
