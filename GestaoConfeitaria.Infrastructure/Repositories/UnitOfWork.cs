using GestaoConfeitaria.Infrastructure.Data;
using GestaoConfeitaria.Infrastructure.Repositories.Interfaces;
using GestaoConfeitaria.Infrastructure.UnitOfWork;

namespace GestaoConfeitaria.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BoloDbContext _context;

        public IVendaRepository VendaRepository { get; }
        public IMaterialRepository MaterialRepository { get; }
        public IGastoRepository GastoRepository { get; }

        public IUserRepository UserRepository { get; }

        public UnitOfWork(BoloDbContext context)
        {
            _context = context;
            VendaRepository = new VendaRepository(context);
            MaterialRepository = new MaterialRepository(context);
            GastoRepository = new GastoRepository(context);
            UserRepository = new UserRepository(context);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
