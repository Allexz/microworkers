using Microworkers.Domain.Core.Entities;
using Microworkers.Domain.Core.Events.User;
using Microworkers.Domain.Core.Exceptions;
using Microworkers.Domain.Core.Validations;
using Microworkers.Domain.Core.ValueObjects;

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

    public static User Create(Guid id, string name, string document, string password, Phone phone, string username, Address address)
    {
        var user = new User(id, name, document, password, phone, username, address);
        var validationResult = new UserValidator();
        var result = validationResult.Validate(user);
        if (!result.IsValid)
        {
            var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new InvalidUserDomainException($"User creation failed: {string.Join(", ", errors)}");
        }

        user.AddDomainEvent(new UserCreatedEvent(user.Id, user.Name, user.Document, user.Phone, user.Username));

        return user;
    }

    private User(Guid id, string name, string document, string password, Phone phone, string username, Address address)
    {
        Id = id;
        Name = name;
        Document = document;
        Password = password;
        Phone = phone;
        Username = username;
        Address = address;
    }


    #region Phone & Address
    public void ChangePhone(Phone newPhone)
    {
        Phone = newPhone;
    }

    public void ChangeAddress(Address newAddress)
    {
        address = newAddress;
    }
    #endregion

    #region Skills
    public void AddSkill(Skill skill)
    {
        _skills.Add(skill);
       
    }

    public void RemoveSkill(Skill skill)
    {
        _skills.Remove(skill);
         
    }

    public void ClearSkills() => _skills.Clear();
    #endregion

    #region Taskis
    public void AddTaski(Taski taski)
    {
        _taskis.Add(taski);
    }

    public void RemoveTaski(Taski taski)
    {
        _taskis.Remove(taski);
    }

    public void ClearTaskis() => _taskis.Clear();
    #endregion
}