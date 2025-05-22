using Microworkers.Domain.Core.Entities;
using Microworkers.Domain.Core.Events ;
using Microworkers.Domain.Core.Exceptions;
using Microworkers.Domain.Core.Validations;
using Microworkers.Domain.Core.ValueObjects;
using System.Net.Sockets;
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

    public static User With(
    User currentUser,
    string name = null,
    string document = null,
    Phone phone = null,
    string username = null,
    Address address = null)
    {
        if (currentUser is null)
            throw new ArgumentNullException("User cannot be null");

        // Se a validação passar, aplicar as alterações ao usuário real
        currentUser.Name = !string.IsNullOrWhiteSpace(name)
            ? name
            : currentUser.Name;
        currentUser.Document = !string.IsNullOrWhiteSpace(document)
            ? document
            : currentUser.Document;
        currentUser.Username = !string.IsNullOrWhiteSpace(username)
            ? username
            : currentUser.Username;

        currentUser.Phone = phone != null
            ? phone
            : currentUser.Phone;
        
        currentUser.Address = address != null
            ? address
            : currentUser.Address;

        // Validar o usuário temporário
        var userValidator = new UserValidator(false);
        var validationResult = userValidator.Validate(currentUser);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new InvalidUserDomainException($"User update failed: {string.Join(", ", errors)}");
        }

        return currentUser;
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