using Microsoft.EntityFrameworkCore;

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

        public async Task<Person?> GetByIdAsync(int id)
        {
            return await dbContext.People.Where(f => f.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Person>> GetAllAsync()
        {
            return await dbContext.People.ToListAsync();
        }

        public async Task<bool> ExistsAsync(string nationalCode)
        {
            return await dbContext.People.Where(f => f.NationalCode == nationalCode).AnyAsync();
        }

        public Task<Person> CreateAsync(string firstName, string lastName, string nationalCode, DateTime? birthDate)
        {
            var person = new Person
            {
                FirstName = firstName,
                LastName = lastName,
                NationalCode = nationalCode,
                BirthDate = birthDate
            };

            dbContext.People.Add(person);

            return Task.FromResult(person);
        }

        public async Task<Person> UpdateAsync(int id, string firstName, string lastName, string nationalCode, DateTime? birthDate)
        {
            var person = await GetByIdAsync(id);

            person!.FirstName = firstName;
            person.LastName = lastName;
            person.NationalCode = nationalCode;
            person.BirthDate = birthDate;

            return person;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var person = await GetByIdAsync(id);

            if (person == null)
                return false;

            dbContext.People.Remove(person);

            return true;
        }
    }
}
