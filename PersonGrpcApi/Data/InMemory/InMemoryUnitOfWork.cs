using PersonGrpcApi.Data.Abstractions;
using PersonGrpcApi.Models;

namespace PersonGrpcApi.Data.InMemory
{
    public class InMemoryUnitOfWork : IUnitOfWork
    {
        private static List<Person> people = new List<Person>();

        public InMemoryUnitOfWork()
        {
            PersonRepository = new InMemoryPersonRepository(people.ConvertAll(person => new Person
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                NationalCode = person.NationalCode,
                BirthDate = person.BirthDate
            }));
        }

        public IPersonRepository PersonRepository { get; }

        public Task CommitAsync()
        {
            people = ((InMemoryPersonRepository)PersonRepository).GetPeople();

            foreach (var person in people)
                if (person.Id == 0)
                    person.Id = people.Max(f => f.Id) + 1;

            return Task.CompletedTask;
        }
    }
}
