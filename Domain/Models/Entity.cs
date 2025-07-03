namespace Domain.Models;

public abstract class Entity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime DataCadastro { get; set; } = DateTime.Now;
    public DateTime DataAlteracao { get; set; }
    public bool Ativo { get; set; } = true;
    public int Codigo { get; set; }
    public bool IsSynced { get; set; } = true;
}