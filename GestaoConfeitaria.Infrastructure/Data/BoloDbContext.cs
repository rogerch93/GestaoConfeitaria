using GestaoConfeitaria.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoConfeitaria.Infrastructure.Data;

public class BoloDbContext : DbContext
{
    public BoloDbContext(DbContextOptions<BoloDbContext> options) : base(options) { }

    public DbSet<Venda> Vendas { get; set; }
    public DbSet<Material> MateriaisUsados { get; set; }
    public DbSet<Gasto> Gastos { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Venda>().ToTable("Vendas");
        modelBuilder.Entity<Material>().ToTable("MateriaisUsados");
        modelBuilder.Entity<Gasto>().ToTable("Gastos");
        modelBuilder.Entity<User>().ToTable("Users");

        // Venda
        modelBuilder.Entity<Venda>()
            .Property(v => v.ValorTotal)
            .HasPrecision(18, 2);           // 18 dígitos no total, 2 casas decimais

        // Gasto
        modelBuilder.Entity<Gasto>()
            .Property(g => g.Valor)
            .HasPrecision(18, 2);

        // Material
        modelBuilder.Entity<Material>()
            .Property(m => m.CustoUnitario)
            .HasPrecision(18, 4);           // 4 casas decimais para custo unitário (mais precisão)

        modelBuilder.Entity<Material>()
            .Property(m => m.Quantidade)
            .HasPrecision(18, 3);           // 3 casas decimais para quantidade (ex: 2,5 kg, 0,750 un)

        // Configurações de soft delete (opcional, mas recomendado)
        modelBuilder.Entity<Venda>().HasQueryFilter(v => v.DataExclusao == null);
        modelBuilder.Entity<Material>().HasQueryFilter(m => m.DataExclusao == null);
        modelBuilder.Entity<Gasto>().HasQueryFilter(g => g.DataExclusao == null);
        modelBuilder.Entity<User>().HasQueryFilter(u => u.DataExclusao == null);

        base.OnModelCreating(modelBuilder);
    }
}