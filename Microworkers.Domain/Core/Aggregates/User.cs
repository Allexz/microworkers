using FluentResults;
using Microworkers.Domain.Core.Entities;
using Microworkers.Domain.Core.Events.User;
using Microworkers.Domain.Core.ValueObjects;
using System.Text.RegularExpressions;

namespace Microworkers.Domain.Core.Aggregates;

public class User : AggregateRoot
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Document { get; private set; }
    public string Password { get; private set; }
    public Phone Phone { get; private set; }
    public string Username { get; private set; }

    private Address address;
    public Address Address
    {
        get => address;
        private set => address = value ?? throw new ArgumentNullException(nameof(Address));
    }
    public IReadOnlyCollection<Skill> Skills => _skills.AsReadOnly();
    private readonly List<Skill> _skills = new();

    public IReadOnlyCollection<Taski> Taskis => _taskis.AsReadOnly();
    private readonly List<Taski> _taskis = new();

    private User() { }

    public static Result<User> Create(Guid id, string name, string document, string password, Phone phone, string username)
    {
        var validationResult = ValidateUserParameters(id, name, document, password, username);
        if (validationResult.IsFailed)
            return validationResult;

        if (!IsValidEmail(username))
            return Result.Fail<User>(new Error("Invalid email format").WithMetadata("Field", nameof(username)));

        var user = new User(id, name, document, password, phone, username);
        user.AddDomainEvent(new UserCreatedEvent(user.Id, user.Name, user.Document, user.Phone, user.Username));

        return Result.Ok(user);
    }

    private static Result ValidateUserParameters(Guid id, string name, string document, string password, string username)
    {
        var result = new Result();

        if (id == Guid.Empty)
            result
                .WithError(new Error("Id cannot be empty")
                .WithMetadata("Field", nameof(id)));

        if (string.IsNullOrWhiteSpace(name) || name.Length > 75)
            result
                .WithError(new Error("Name is required and must have up to 75 characters")
                .WithMetadata("Field", nameof(name)));

        if (string.IsNullOrWhiteSpace(document) || document.Length > 20)
            result
                .WithError(new Error("Document is required and must have up to 20 characters")
                .WithMetadata("Field", nameof(document)));

        if (string.IsNullOrWhiteSpace(password) || password.Length != 12)
            result
                .WithError(new Error("Password is required and must have exactly 12 characters")
                .WithMetadata("Field", nameof(password)));

        if (string.IsNullOrWhiteSpace(username) || username.Length > 75)
            result
                .WithError(new Error("Username is required and must have up to 75 characters")
                .WithMetadata("Field", nameof(username)));

        return result;
    }

    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        string pattern = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";

        try
        {
            return Regex.IsMatch(email, pattern);
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

    internal User(Guid id, string name, string document, string password, Phone phone, string username)
    {
        Id = id;
        Name = name;
        Document = document;
        Password = password;
        Phone = phone;
        Username = username;
    }

    #region Phone & Address
    public Result ChangePhone(Phone newPhone)
    {
        Phone = newPhone;
        return Result.Ok();
    }

    public Result ChangeAddress(Address newAddress)
    {
        if (newAddress == null)
            return Result.Fail(new Error("Address cannot be null").WithMetadata("Field", nameof(newAddress)));

        address = newAddress;
        return Result.Ok();
    }
    #endregion

    #region Skills
    public Result AddSkill(Skill skill)
    {
        if (skill == null)
            return Result
                .Fail(new Error("Skill cannot be null")
                .WithMetadata("Field", nameof(skill)));

        if (_skills.Any(s => s.Id == skill.Id))
            return Result
                .Fail(new Error("Skill already exists")
                .WithMetadata("Field", nameof(skill)));

        _skills.Add(skill);
        return Result.Ok();
    }

    public Result RemoveSkill(Guid skillId)
    {
        if (skillId == Guid.Empty)
            return Result
                .Fail(new Error("Skill ID cannot be empty")
                .WithMetadata("Field", nameof(skillId)));

        var skill = _skills.FirstOrDefault(s => s.Id == skillId);
        if (skill == null)
            return Result
                .Fail(new Error("Skill not found")
                .WithMetadata("Field", nameof(skillId)));

        _skills.Remove(skill);
        return Result.Ok();
    }

    public void ClearSkills() => _skills.Clear();
    #endregion

    #region Taskis
    public Result AddTaski(Taski taski)
    {
        if (taski == null)
            return Result
                .Fail(new Error("Taski cannot be null")
                .WithMetadata("Field", nameof(taski)));

        if (_taskis.Any(t => t.Id == taski.Id))
            return Result
                .Fail(new Error("Taski already exists")
                .WithMetadata("Field", nameof(taski)));

        if (_taskis.Count(x => x.Status == Enums.TaskiStatus.InProgress) >= 3)
            return Result
                .Fail(new Error("User can only have 3 taskis in progress at a time")
                .WithMetadata("Field", nameof(taski)));

        _taskis.Add(taski);
        return Result.Ok();
    }

    public Result RemoveTaski(Guid taskiId)
    {
        if (taskiId == Guid.Empty)
            return Result
                .Fail(new Error("Taski ID cannot be empty")
                .WithMetadata("Field", nameof(taskiId)));

        var taski = _taskis.FirstOrDefault(t => t.Id == taskiId);
        if (taski == null)
            return Result
                .Fail(new Error("Taski not found")
                .WithMetadata("Field", nameof(taskiId)));

        _taskis.Remove(taski);
        return Result.Ok();
    }

    public void ClearTaskis() => _taskis.Clear();
    #endregion
}