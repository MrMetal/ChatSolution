using Data;
using Microsoft.EntityFrameworkCore;
using Shared.Results;

namespace ChatServer.Services;

public interface IContactService
{
    Task<IEnumerable<ContactsResult>> ObterTodos(string ownerEmail);
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
                Id = x.Id
            })
            .OrderBy(x => x.Name)
            .ToListAsync();

        return result;
    }
}