namespace PersonGrpcApi.Models;

public class Person
{
    public int Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string NationalCode { get; set; }

    public DateTime? BirthDate { get; set; }
}
