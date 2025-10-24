namespace Clean.Application.Abstractions;

public interface IDbContexfet
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
