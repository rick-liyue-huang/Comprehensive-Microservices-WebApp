namespace usersService.Core.Dtos;

public record RegisterRequest(
    string? Email,
    string? Password,
    string? PersonName,
    GenderOptions? Gender
);