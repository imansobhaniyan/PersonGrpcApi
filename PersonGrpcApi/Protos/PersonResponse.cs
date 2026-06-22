
//changed namespace to match the actual auto generated class
using PersonGrpcApi.Models;

namespace PersonGrpcApi.GrpcModels
{
    public partial class PersonResponse
    {
        public PersonResponse(Person person)
        {
            Id = person.Id;
            FirstName = person.FirstName;
            LastName = person.LastName;
            NationalCode = person.NationalCode;
            BirthDate = person.BirthDate?.ToString("yyyy-MM-dd");
        }
    }
}
