namespace UsersMicroservice.Core.Dtos;

public record RegisterRequest(
    string? Eamil,
    string? Password,
    string? PersonName,
    GenderOptions Gender
);
