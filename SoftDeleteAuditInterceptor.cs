using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ef_softdeletion_audit.Entities;

namespace ef_softdeletion_audit;

sealed class SoftDeleteAuditInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("SavingChangesAsync called");

        if (eventData.Context is null)
        {
            return base.SavingChangesAsync(
                eventData, result, cancellationToken);
        }

        Console.WriteLine("Getting all context changes except unchanged status");

        IEnumerable<EntityEntry<Audit>> edited = eventData.Context.ChangeTracker
               .Entries<Audit>()
               .Where(e => e.State != EntityState.Unchanged);

        var now = DateTimeOffset.Now;
        foreach (EntityEntry<Audit> entry in edited)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
            }
            entry.Entity.UpdatedAt = now;
        }

        Console.WriteLine("Getting all context deletion to set IsDelete to true");

        IEnumerable<EntityEntry<ISoftDelete>> entries = eventData.Context.ChangeTracker
                .Entries<ISoftDelete>()
                .Where(e => e.State == EntityState.Deleted);

        foreach (EntityEntry<ISoftDelete> softDeletable in entries)
        {
            softDeletable.State = EntityState.Modified;
            softDeletable.Entity.IsDeleted = true;
        }

        Console.WriteLine("Saving changes to the context");
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
