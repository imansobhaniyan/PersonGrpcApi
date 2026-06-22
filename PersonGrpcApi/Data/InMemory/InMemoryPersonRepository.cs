using PersonGrpcApi.Data.Abstractions;
using PersonGrpcApi.Models;

namespace PersonGrpcApi.Data.InMemory
{
    public class InMemoryPersonRepository : IPersonRepository
    {
        private readonly List<Person> people;

        public InMemoryPersonRepository(List<Person> people)
        {
            this.people = people;
        }

        public Task<Person?> GetByIdAsync(int id)
        {
            return Task.FromResult(people.Where(f => f.Id == id).FirstOrDefault());
        }

        public Task<List<Person>> GetAllAsync()
        {
            return Task.FromResult(people);
        }

        public Task<bool> ExistsAsync(string nationalCode)
        {
            return Task.FromResult(people.Where(f => f.NationalCode == nationalCode).Any());
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

            people.Add(person);

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

            people.Remove(person);

            return true;
        }

        public List<Person> GetPeople()
        {
            return people;
        }
    }
}
