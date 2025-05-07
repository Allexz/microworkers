using Microworkers.Domain.Core.Exceptions;

namespace Microworkers.Domain.Core.ValueObjects;
public readonly record struct Document(string Number)
{
    public static Document Create(string number) => new Document(ValidateAndFormat(number));
    public override string ToString() => ValidateAndFormat(Number);

    // Conversão implícita para string
    public static implicit operator string(Document document) => document.Number;

    // Conversão explícita de string
    public static explicit operator Document(string number) => new(number);

    private static bool IsValidCpf(string cpf)
    {
        // Remove caracteres não numéricos
        var digitsOnly = new string(cpf.Where(char.IsDigit).ToArray());

        // Verifica tamanho
        if (digitsOnly.Length != 11)
            return false;

        // Verifica dígitos repetidos (ex: 111.111.111-11)
        if (digitsOnly.Distinct().Count() == 1)
            return false;

        // Cálculo do primeiro dígito verificador
        var sum = 0;
        for (var i = 0; i < 9; i++)
            sum += int.Parse(digitsOnly[i].ToString()) * (10 - i);

        var remainder = sum % 11;
        var firstDigit = remainder < 2 ? 0 : 11 - remainder;

        // Cálculo do segundo dígito verificador
        sum = 0;
        for (var i = 0; i < 10; i++)
            sum += int.Parse(digitsOnly[i].ToString()) * (11 - i);

        remainder = sum % 11;
        var secondDigit = remainder < 2 ? 0 : 11 - remainder;

        // Verifica se os dígitos calculados conferem
        return digitsOnly.EndsWith($"{firstDigit}{secondDigit}");
    }

    private static string FormatCpf(string cpf)
    {
        var digitsOnly = new string(cpf.Where(char.IsDigit).ToArray());
        return Convert.ToUInt64(digitsOnly).ToString(@"000\.000\.000\-00");
    }

    private static string ValidateAndFormat(string number)
    {
        // Mesma lógica de validação anterior
        if (!IsValidCpf(number))
            throw new DomainException("Document isn't valid", nameof(Document));

        return FormatCpf(number);
    }
}
