using ArsalanApp.Contracts;
using ArsalanApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace ArsalanApp.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    private readonly string _username;


    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        _username = "Test User";
    }

    public DbSet<AuditEntry> AuditEntries { get; set; }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var dictionaryConverter = new ValueConverter<Dictionary<string, object?>, string>(
          v => JsonConvert.SerializeObject(v),
          v => JsonConvert.DeserializeObject<Dictionary<string, object?>>(v) ?? new Dictionary<string, object?>()
        );

        var dictionaryComparer = new ValueComparer<Dictionary<string, object?>>(
            (d1, d2) => JsonConvert.SerializeObject(d1) == JsonConvert.SerializeObject(d2),
            d => JsonConvert.SerializeObject(d).GetHashCode(),
            d => JsonConvert.DeserializeObject<Dictionary<string, object?>>(
                     JsonConvert.SerializeObject(d))!
        );

        modelBuilder.Entity<AuditEntry>().Property(ae => ae.CurrentValues)
            .HasConversion(dictionaryConverter)
            .Metadata.SetValueComparer(dictionaryComparer);

        modelBuilder.Entity<AuditEntry>().Property(ae => ae.OldValues)
            .HasConversion(dictionaryConverter!)
            .Metadata.SetValueComparer(dictionaryComparer);
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        // Get audit entries
        var auditEntries = OnBeforeSaveChanges();

        // Save current entity
        var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

        // Save audit entries
        await OnAfterSaveChangesAsync(auditEntries);
        return result;
    }

    private List<AuditEntry> OnBeforeSaveChanges()
    {
        ChangeTracker.DetectChanges();

        var x = ChangeTracker.Entries();

        var entries = new List<AuditEntry>();

        foreach (var entry in ChangeTracker.Entries())
        {
            // Dot not audit entities that are not tracked, not changed, or not of type IAuditable
            if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged || !(entry.Entity is IAuditable))
                continue;



            var auditEntry = new AuditEntry
            {
                ActionType = entry.State == EntityState.Added ? "INSERT" : entry.State == EntityState.Deleted ? "DELETE" : "UPDATE",
                EntityId = entry.Properties.Single(p => p.Metadata.IsPrimaryKey()).CurrentValue!.ToString()!,
                EntityName = entry.Metadata.ClrType.Name,
                UserName = _username,
                TimeStamp = DateTime.UtcNow,
                CurrentValues = GetPropertyValues(entry, useOriginal: false),
                OldValues = entry.State == EntityState.Modified ? GetPropertyValues(entry, useOriginal: true) : null,
                // TempProperties are properties that are only generated on save, e.g. ID's
                // These properties will be set correctly after the audited entity has been saved
                TempProperties = entry.Properties.Where(p => p.IsTemporary).ToList(),
            };

            entries.Add(auditEntry);
        }

        return entries;
    }

    private Dictionary<string, object?> GetPropertyValues(EntityEntry entry, bool useOriginal)
    {
        return entry.Properties.ToDictionary(
            p => p.Metadata.Name,
            p => useOriginal ? p.OriginalValue : p.CurrentValue
        );
    }

    private Task OnAfterSaveChangesAsync(List<AuditEntry> auditEntries)
    {
        if (auditEntries == null || auditEntries.Count == 0)
            return Task.CompletedTask;

        // For each temporary property in each audit entry - update the value in the audit entry to the actual (generated) value
        foreach (var entry in auditEntries)
        {
            if (entry.TempProperties == null || entry.TempProperties.Count == 0)
                continue;

            foreach (var prop in entry.TempProperties)
            {
                if (prop.Metadata.IsPrimaryKey())
                {
                    entry.EntityId = prop.CurrentValue!.ToString()!;
                    entry.CurrentValues[prop.Metadata.Name] = prop.CurrentValue;
                }
                else
                {
                    entry.CurrentValues[prop.Metadata.Name] = prop.CurrentValue;
                }
            }
        }

        AuditEntries.AddRange(auditEntries);
        return SaveChangesAsync();
    }


}