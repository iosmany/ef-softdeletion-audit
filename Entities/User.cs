namespace ef_softdeletion_audit.Entities;

public class User : Audit, ISoftDelete
{
    public long Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public int LoginAttempts { get; set; }
    public bool IsDeleted { get; set; }
}
