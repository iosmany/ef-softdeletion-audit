

namespace ef_softdeletion_audit.Entities;

interface ISoftDelete
{
    bool IsDeleted { get; set; }
}
