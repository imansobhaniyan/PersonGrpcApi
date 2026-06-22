using Microsoft.EntityFrameworkCore;

using PersonGrpcApi.Data.Abstractions;
using PersonGrpcApi.Models;

namespace PersonGrpcApi.Data.EF
{
    public class PersonDbContext : DbContext, IUnitOfWork
    {
        public PersonDbContext(DbContextOptions<PersonDbContext> options) : base(options)
        {
            PersonRepository = new PersonRepository(this);
        }

        public DbSet<Person> People { get; set; }

        public IPersonRepository PersonRepository { get; }

        public async Task CommitAsync()
        {
            await SaveChangesAsync();
        }
    }
}
