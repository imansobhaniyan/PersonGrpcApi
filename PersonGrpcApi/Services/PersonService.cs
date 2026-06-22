using Grpc.Core;

using PersonGrpcApi.Data.Abstractions;
using PersonGrpcApi.GrpcModels;

namespace PersonGrpcApi.Services
{
    public class PersonService : PersonProtoService.PersonProtoServiceBase
    {
        private readonly IUnitOfWork unitOfWork;

        public PersonService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public override async Task<PersonResponse> GetById(PersonIdRequest request, ServerCallContext context)
        {
            var person = await unitOfWork.PersonRepository.GetByIdAsync(request.Id);

            if (request == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Person not found"));

            return new PersonResponse(person);
        }

        public override async Task<PersonListResponse> GetAll(Empty request, ServerCallContext context)
        {
            var people = await unitOfWork.PersonRepository.GetAllAsync();

            var result = new PersonListResponse();

            result.Persons.AddRange(people.ConvertAll(person => new PersonResponse(person)));

            return result;
        }
    }
}
