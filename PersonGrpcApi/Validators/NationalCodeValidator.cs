using System.Text.RegularExpressions;

namespace PersonGrpcApi.Validators
{
    public class NationalCodeValidator
    {
        public static bool Validate(string nationalCode)
        {
            if (!Regex.IsMatch(nationalCode, @"^(?!([0-9])\1{9})\d{10}$"))
                return false;

            int check = int.Parse(nationalCode[9].ToString());

            int sum = nationalCode
                .Take(9)
                .Select((x, i) => (x - '0') * (10 - i))
                .Sum();

            int remainder = sum % 11;

            return (remainder < 2 && check == remainder) ||
                   (remainder >= 2 && check == 11 - remainder);
        }
    }
}
