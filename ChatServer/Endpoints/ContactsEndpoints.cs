using ChatServer.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Results;

namespace ChatServer.Endpoints;

public static class ContactsEndpoints
{
    public static void MapContactsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("Contacts");
        group.MapGet("/GetAllContactsEndpoint/{from}", GetAllContactsEndpoint.ExecuteAsync).WithTags("Get All");
        group.MapGet("/GetByIdContactsEndpoint/{id:guid}", GetByIdContactsEndpoint.ExecuteAsync).WithTags("Get By Id");
    }
}

public static class GetAllContactsEndpoint
{
    internal static async Task<IEnumerable<ContactsResult>> ExecuteAsync([FromRoute] string from, IContactService contactService)
    {
        var result = await contactService.ObterTodos(from);
        return result;
    }
}

public static class GetByIdContactsEndpoint
{
    internal static async Task<ContactsResult?> ExecuteAsync([FromRoute] Guid id, IContactService contactService)
    {
        var result = await contactService.ObterPorId(id);
        return result;
    }
}