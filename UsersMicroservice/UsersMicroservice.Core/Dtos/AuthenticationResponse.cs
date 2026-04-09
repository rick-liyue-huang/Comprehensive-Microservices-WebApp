namespace UsersMicroservice.Core.Dtos;

public record AuthenticationResponse(
    Guid UserId,
    string? Email,
    string? PersonName,
    string? Gender,
    string? Token,
    bool Success
)
{
    // parameterless constructor
    public AuthenticationResponse() : this(default, default, default, default, default, default)
    {}
}
