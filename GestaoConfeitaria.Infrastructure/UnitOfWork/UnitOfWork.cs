using GestaoConfeitaria.Infrastructure.Repositories.Interfaces;

namespace GestaoConfeitaria.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IVendaRepository VendaRepository { get; }
        IMaterialRepository MaterialRepository { get; }
        IGastoRepository GastoRepository { get; }
        IUserRepository UserRepository { get; }

        Task SaveChangesAsync();
    }
}
