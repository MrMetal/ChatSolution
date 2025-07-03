using Domain.Models;

namespace Data;

public interface IContactRepository : IRepository<Contacts>;

public class ContactRepository(ApplicationDbContext db) : Repository<Contacts>(db), IContactRepository;