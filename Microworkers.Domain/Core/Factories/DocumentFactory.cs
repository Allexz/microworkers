using Microworkers.Domain.Core.Enums;
using Microworkers.Domain.Core.ValueObjects;
using Microworkers.Domain.Shared;

namespace Microworkers.Domain.Core.Factories;
public static class DocumentFactory
{
    public static Result<Document> Create(string number, DocumentType docType)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(number))
            errors.Add("Document cannot be null nor empty");
        else if (!IsValidCpf(number))
            errors.Add("Document is not a valid CPF");
        else if (docType != DocumentType.CPF)
            errors.Add("Document type is not valid");

        if (errors.Any())
            return Result.Fail<Document>(string.Join("; ", errors));
        
        return Result.Ok(new Document(FormatCpf(number),docType));
    }

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
}
