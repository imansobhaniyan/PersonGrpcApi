using PersonGrpcApi.Models;

namespace PersonGrpcApi.Data.Abstractions;

public interface IPersonRepository
{
    Task<Person> GetByIdAsync(int id);

    Task<List<Person>> GetAllAsync();

    Task<bool> ExistsAsync(string nationalCode);

    Task<Person> CreateAsync(string firstName, string lastName, string nationalCode, DateTime? birthDate);

    Task<Person> UpdateAsync(int id, string firstName, string lastName, string nationalCode, DateTime? birthDate);

    Task<bool> DeleteAsync(int id);
}
