using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ArsalanApp.Models;

public class AuditEntry
{
    public long Id { get; set; }
    public string? EntityName { get; set; }
    public string ActionType { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public DateTime TimeStamp { get; set; }
    public string EntityId { get; set; } = string.Empty;
    public Dictionary<string, object?>? OldValues { get; set; }
    public Dictionary<string, object?> CurrentValues { get; set; } = new();

    [NotMapped]
    // TempProperties are used for properties that are only generated on save, e.g. ID's
    public List<PropertyEntry>? TempProperties { get; set; }
}
