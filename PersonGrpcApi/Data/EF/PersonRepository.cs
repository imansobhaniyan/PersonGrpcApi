using PersonGrpcApi.Data.Abstractions;
using PersonGrpcApi.Models;

namespace PersonGrpcApi.Data.EF
{
    public class PersonRepository : IPersonRepository
    {
        private readonly PersonDbContext dbContext;

        public PersonRepository(PersonDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<Person> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Person>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(string nationalCode)
        {
            throw new NotImplementedException();
        }

        public Task<Person> CreateAsync(string firstName, string lastName, string nationalCode, DateTime? birthDate)
        {
            throw new NotImplementedException();
        }

        public Task<Person> UpdateAsync(int id, string firstName, string lastName, string nationalCode, DateTime? birthDate)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
