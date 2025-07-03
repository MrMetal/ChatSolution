namespace Data;

public interface IUnitOfWork : IDisposable
{
    Task SaveChangesAsync();
}

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public async Task SaveChangesAsync()
    {
        try
        {
            await context.SaveChangesAsync();
            Dispose();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }
}