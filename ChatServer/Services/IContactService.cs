using Data;
using Microsoft.EntityFrameworkCore;
using Shared.Results;

namespace ChatServer.Services;

public interface IContactService
{
    Task<IEnumerable<ContactsResult>> ObterTodos(string ownerEmail);
    Task<ContactsResult?> ObterPorId(Guid id);
}

public class ContactService(ApplicationDbContext context) : IContactService
{
    public async Task<IEnumerable<ContactsResult>> ObterTodos(string ownerEmail)
    {
        var result = await context.Contacts
            .AsNoTracking()
            .Where(c => c.OwnerEmail == ownerEmail) // <-- filtra por dono
            .Select(x => new ContactsResult
            {
                ConnectionId = x.ConnectionId,
                Email = x.Email,
                Name = x.Name,
                Status = x.Status,
            })
            .ToListAsync();

        return result;
    }

    public async Task<ContactsResult?> ObterPorId(Guid id)
    {
        var result = await context.Contacts
            .AsNoTracking().Select(x => new ContactsResult
            {
                ConnectionId = x.ConnectionId,
                Email = x.Email,
                Name = x.Name,
                Status = x.Status,
            })
            .FirstOrDefaultAsync(x => x.Id == id);

        return result;
    }
}