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
    }
}
