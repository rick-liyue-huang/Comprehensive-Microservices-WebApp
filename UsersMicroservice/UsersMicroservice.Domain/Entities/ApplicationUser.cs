namespace UsersMicroservice.Domain.Entities;

/// <summary>
/// Define the ApplicationUsser class which as entity in database
/// </summary>
public class ApplicationUser
{
    public Guid UserId { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? PersonName { get; set; }
    public string? Gender { get; set; }
}
