
namespace myBookSolution.Application.Services.CpfValidator;

public class CpfValidator : ICpfValidator
{
    public bool IsValid(string cpf)
    {
        string cleanCpf = cpf.Trim().Replace(".", "").Replace("-", "");

        if (cleanCpf.Length != 11 || new string(cleanCpf[0], 11) == cleanCpf)
            return false;

        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            sum += (cleanCpf[i] - '0') * (10 - i);
        }

        int firstDigit = 11 - (sum % 11);
        if (firstDigit > 9)
            firstDigit = 0;

        if (int.Parse(cleanCpf[9].ToString()) != firstDigit)
            return false;

        //calculo do segundo dígito verificador
        sum = 0;
        for (int i = 0; i < 10; i++)
        {
            sum += (cleanCpf[i] - '0') * (11 - i);
        }

        int secondDigit = 11 - (sum % 11);
        if (secondDigit > 9)
            secondDigit = 0;

        if (int.Parse(cleanCpf[10].ToString()) != secondDigit)
            return false;

        return true;
    }
}
