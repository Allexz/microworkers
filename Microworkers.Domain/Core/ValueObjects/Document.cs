using Microworkers.Domain.Core.Enums;

namespace Microworkers.Domain.Core.ValueObjects;
public record Document
{
    private Document() { }
    public DocumentType DocType { get; init; }
    public string Number { get; init; }
    internal Document(string number, DocumentType type)
    { 
        Number = number;
        DocType = type;
    }
}
