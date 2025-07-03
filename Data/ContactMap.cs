using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data;

public class ContactMap : IEntityTypeConfiguration<Contacts>
{
    public void Configure(EntityTypeBuilder<Contacts> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasMany(p => p.Messages)
            .WithOne(m => m.Contact)
            .HasForeignKey(p => p.ContactId);

        builder.ToTable("Contacts");
    }
}