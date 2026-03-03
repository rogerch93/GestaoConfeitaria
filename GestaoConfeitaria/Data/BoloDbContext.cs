using Microsoft.EntityFrameworkCore;
using GestaoConfeitaria.Models;

namespace GestaoConfeitaria.Data;

public class BoloDbContext : DbContext
{
    public BoloDbContext(DbContextOptions<BoloDbContext> options) : base(options) { }

    public DbSet<Venda> Vendas { get; set; }
    public DbSet<Material> MateriaisUsados { get; set; }
    public DbSet<Gasto> Gastos { get; set; }
    public DbSet<User> Users { get; set; }
}