using Grpc.Core;

using PersonGrpcApi.Data.Abstractions;
using PersonGrpcApi.GrpcModels;
using PersonGrpcApi.Models;
using PersonGrpcApi.Validators;

using System.Globalization;

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

        public override async Task<PersonResponse> Create(CreatePersonRequest request, ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.FirstName?.Trim()))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "First Name is required"));

            if (string.IsNullOrWhiteSpace(request.LastName?.Trim()))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Last Name is required"));

            if (string.IsNullOrWhiteSpace(request.NationalCode?.Trim()))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "National Code is required"));

            if (!NationalCodeValidator.Validate(request.NationalCode))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "National Code is invalid"));

            if (await unitOfWork.PersonRepository.ExistsAsync(request.NationalCode))
                throw new RpcException(new Status(StatusCode.AlreadyExists, "National Code already exists"));

            DateTime? birthDate = DateTime.TryParseExact(request.BirthDate, PersonResponse.BIRTH_DATE_FORMAT, null, DateTimeStyles.None, out var res) ? res : null;

            var person = await unitOfWork.PersonRepository.CreateAsync(request.FirstName, request.LastName, request.NationalCode, birthDate);

            await unitOfWork.CommitAsync();

            return new PersonResponse(person);
        }

        public override async Task<Empty> Delete(PersonIdRequest request, ServerCallContext context)
        {
            var deleteResult = await unitOfWork.PersonRepository.DeleteAsync(request.Id);

            if (deleteResult is false)
                throw new RpcException(new Status(StatusCode.NotFound, "Person not found"));

            return new Empty();
        }
    }
}
