namespace usersService.Core.Dtos;

public record AuthenticationResponse(
    Guid UserId,
    string? Email,
    string? PersonName,
    string? Gender,
    string? Token,
    bool Success
)
{
    // Parameterless constructor for mapping purposes
    public AuthenticationResponse() 
        : this(Guid.Empty, null, null, null, null, false) { }
}