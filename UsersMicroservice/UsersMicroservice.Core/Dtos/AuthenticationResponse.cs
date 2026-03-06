namespace UsersMicroservice.Domain.Dtos;

public record AuthenticationResponse(
    Guid UserId,
    string? Email,
    string? PersonName,
    string? Gender,
    string? Token,
    bool? Success
)
{
    public AuthenticationResponse() : this(Guid.Empty, null, null, null, null, null) { }
}