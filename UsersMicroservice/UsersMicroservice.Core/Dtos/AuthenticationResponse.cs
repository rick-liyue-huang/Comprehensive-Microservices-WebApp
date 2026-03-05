namespace UsersMicroservice.Domain.Dtos;

public record AuthenticationResponse(
    Guid Id,
    string? Email,
    string? PersonName,
    string? Gender,
    string? Token,
    bool? Success
);