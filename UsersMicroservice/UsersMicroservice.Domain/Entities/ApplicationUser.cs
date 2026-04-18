
namespace UsersMicroservice.Domain.Entities;

/// <summary>
/// Define the ApplicationUser class which as entity in database
/// </summary>
public class ApplicationUser
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string PersonName { get; set; } = null!;
    public string? Gender { get; set; }
}
