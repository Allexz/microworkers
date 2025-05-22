// 📂 Microworkers.Domain/Shared/Result.cs
namespace Microworkers.Domain.Shared;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string Error { get; }

    protected Result(bool isSuccess, string error)
    {
        if (isSuccess && !string.IsNullOrEmpty(error))
            throw new InvalidOperationException("Sucesso não pode ter erro.");

        if (!isSuccess && string.IsNullOrEmpty(error))
            throw new InvalidOperationException("Falha requer mensagem de erro.");

        IsSuccess = isSuccess;
        Error = error;
    }

    // Fábrica de sucesso
    public static Result Ok() => new Result(true, string.Empty);

    // Fábrica de falha
    public static Result Fail(string error) => new Result(false, error);

    // Fábrica genérica (para resultados com valor)
    public static Result<T> Ok<T>(T value) => new Result<T>(value, true, string.Empty);
    public static Result<T> Fail<T>(string error) => new Result<T>(default!, false, error);
}

// Versão genérica (para retornar valores)
public class Result<T> : Result
{
    public T Value { get; }

    internal Result(T value, bool isSuccess, string error)
        : base(isSuccess, error)
    {
        Value = value;
    }
}