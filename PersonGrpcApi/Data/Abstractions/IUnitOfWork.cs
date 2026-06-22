namespace PersonGrpcApi.Data.Abstractions;

public interface IUnitOfWork
{
    IPersonRepository PersonRepository { get; }

    Task CommitAsync();
}