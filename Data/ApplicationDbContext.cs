using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class ApplicationDbContext(DbContextOptions options) : IdentityDbContext<IdentityUser>(options)
{
    #region Entities

    public DbSet<Contacts> Contacts { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Mapeamento Fluent API

        modelBuilder.ApplyConfiguration(new ContactMap());

        #endregion

        //foreach (var entity in modelBuilder.Model.GetEntityTypes())
        //    modelBuilder.Entity(entity.Name).Property("Id").HasColumnName(entity.GetTableName() + "Id");

        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.ClientCascade;

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
        {
            if (entry.State == EntityState.Added) entry.Property("DataCadastro").CurrentValue = DateTime.Now;

            if (entry.State == EntityState.Modified) entry.Property("DataCadastro").IsModified = false;
        }

        foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataAlteracao") != null))
        {
            if (entry.State == EntityState.Added) entry.Property("DataAlteracao").CurrentValue = DateTime.Now;

            if (entry.State == EntityState.Modified) entry.Property("DataAlteracao").CurrentValue = DateTime.Now;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}