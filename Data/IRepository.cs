using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Domain.Models;

namespace Data;

public interface IRepository<TEntity> : IDisposable where TEntity : Entity
{
    void Adicionar(TEntity entity);
    void Atualizar(TEntity entity);
    Task Remover(Guid id);
    Task<TEntity?> ObterPorId(Guid id);
    Task<IEnumerable<TEntity>> ObterTodos();
    Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate);
    Task SaveChangesAsync();
}

public class Repository<TEntity>(ApplicationDbContext db) : IRepository<TEntity> where TEntity : Entity
{
    protected readonly ApplicationDbContext Db = db;
    protected readonly DbSet<TEntity> DbSet = db.Set<TEntity>();

    public void Adicionar(TEntity entity) => DbSet.Add(entity);

    public void Atualizar(TEntity entity) => DbSet.Update(entity);

    public async Task Remover(Guid id)
    {
        var entity = await DbSet.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
        DbSet.Remove(entity ?? throw new Exception($"{entity} não encontrado(a)."));
    }

    public async Task<TEntity?> ObterPorId(Guid id)
        => await DbSet.AsNoTracking().IgnoreAutoIncludes().Where(x => x.Id == id && x.Ativo).FirstOrDefaultAsync();

    public async Task<IEnumerable<TEntity>> ObterTodos()
        => await DbSet.AsNoTracking().Where(x => x.Ativo).IgnoreAutoIncludes().ToListAsync();

    public async Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate)
        => await DbSet.AsNoTracking().Where(predicate).ToListAsync();

    public Task SaveChangesAsync() => Db.SaveChangesAsync();

    public void Dispose()
    {
        Db.Dispose();
        GC.SuppressFinalize(this);
    }
}