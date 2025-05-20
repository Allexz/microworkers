using FluentResults;
using System.Text;

namespace Microworkers.Domain.Core.Services;
public static class ErrorMessageServices
{
    public static string BuildUserInvalidDomainMessage(List<IError> errors)
    {
        var sb = new StringBuilder();
        errors.ForEach(error =>
        {
            sb.Append(error.Message);
            sb.AppendLine();
        });
        return sb.ToString();
    }
}
