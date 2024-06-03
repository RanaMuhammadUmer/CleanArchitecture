namespace CleanArchitecture.Infrastructure.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CleanArchitectureDbContext _context;

        public UnitOfWork(CleanArchitectureDbContext context)
        {
            _context = context;
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
