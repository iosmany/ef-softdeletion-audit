

namespace ef_softdeletion_audit.Entities;
public abstract class Audit
{
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
