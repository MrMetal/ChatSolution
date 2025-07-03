using Domain.Models;

namespace Data;

public interface IChatMessageRepository : IRepository<ChatMessage>;

public class ChatMessageRepository(ApplicationDbContext db) : Repository<ChatMessage>(db), IChatMessageRepository;