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

            if (person == null)
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
            validateArguments(request.FirstName, request.LastName, request.NationalCode);

            if (!NationalCodeValidator.Validate(request.NationalCode))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "National Code is invalid"));

            if (await unitOfWork.PersonRepository.ExistsAsync(request.NationalCode))
                throw new RpcException(new Status(StatusCode.AlreadyExists, "National Code already exists"));

            DateTime? birthDate = parseBirthDate(request.BirthDate);

            var person = await unitOfWork.PersonRepository.CreateAsync(request.FirstName, request.LastName, request.NationalCode, birthDate);

            await unitOfWork.CommitAsync();

            return new PersonResponse(person);
        }

        public override async Task<PersonResponse> Update(UpdatePersonRequest request, ServerCallContext context)
        {
            validateArguments(request.FirstName, request.LastName, request.NationalCode);

            var person = await unitOfWork.PersonRepository.GetByIdAsync(request.Id);

            if (person == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Person not found"));

            if (person.NationalCode != request.NationalCode)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "National Code could not be changed"));

            DateTime? birthDate = parseBirthDate(request.BirthDate);

            person = await unitOfWork.PersonRepository.UpdateAsync(request.Id, request.FirstName, request.LastName, request.NationalCode, birthDate);

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

        private static DateTime? parseBirthDate(string birthDate)
        {
            return DateTime.TryParseExact(birthDate, PersonResponse.BIRTH_DATE_FORMAT, null, DateTimeStyles.None, out var res) ? res : null;
        }

        private static void validateArguments(string firstName, string lastName, string nationalCode)
        {
            if (string.IsNullOrWhiteSpace(firstName?.Trim()))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "First Name is required"));

            if (string.IsNullOrWhiteSpace(lastName?.Trim()))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Last Name is required"));

            if (string.IsNullOrWhiteSpace(nationalCode?.Trim()))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "National Code is required"));
        }
    }
}
