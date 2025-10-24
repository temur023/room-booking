namespace Clean.Application.Abstractions;

public interface IDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
