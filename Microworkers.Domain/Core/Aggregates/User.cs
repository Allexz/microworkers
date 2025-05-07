using Microworkers.Domain.Core.Entities;
using Microworkers.Domain.Core.Events.User;
using Microworkers.Domain.Core.Exceptions;
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
        private set => address = value ?? throw new DomainException("Address cannot be empty", nameof(Address));
    }
    public IReadOnlyCollection<Skill> Skills => _skills.AsReadOnly();

    private readonly List<Skill> _skills = new();

    public IReadOnlyCollection<Taski> Taskis => _taskis.AsReadOnly();
    private readonly List<Taski> _taskis = new();

    private User() { }

    public static User Create(Guid id, string name, string document, string password, Phone phone, string username)
    {
        User user = new User(id, name, document, password, phone, username);
        user.AddDomainEvent(new UserCreatedEvent(user.Id, user.Name, user.Document,  user.Phone, user.Username));
        
        return user;
    }


    internal User(Guid id, string name, string document, string password, Phone phone, string username)
    {
        Id = ValidateId(id);
        Name = ValidateName(name);
        Document = ValidateDocument(document);
        Password = ValidatePassword(password);
        Phone = ChangePhone(phone);
        Username = ValidateUserName(username);
    }

    internal User(Guid id, string document, string username)
    {
        Id = ValidateId(id);
        Document = ValidateDocument(document);
        Username = ValidateUserName(username);
    }


    #region Properties
    private string ValidateName(string name) =>
        !string.IsNullOrWhiteSpace(name) && name.Length <= 75
            ? name
            : throw new DomainException("Name is required and must have up to 75 characters", nameof(name));
    private Guid ValidateId(Guid id) =>
        id != Guid.Empty
            ? id
            : throw new DomainException("Id cannot be empty", nameof(id));
    private string ValidateDocument(string document) =>
        !string.IsNullOrWhiteSpace(document) && document.Length <= 20
            ? document
            : throw new DomainException("Document is required and must have up to 20 characters", nameof(document));
    private string ValidatePassword(string password) =>
        !string.IsNullOrWhiteSpace(password) && password.Length != 12
            ? password
            : throw new DomainException("Password is required and must have up to 12 characters", nameof(password));
    private string ValidateUserName(string username) =>
        !string.IsNullOrWhiteSpace(username) && username.Length != 75
            ? username
            : throw new DomainException("Username is required and must have up to 75 characters", nameof(username));

    #endregion

    #region Phone & Address
    public Phone ChangePhone(Phone newPhone) => Phone = newPhone;
    public void ChangeAddress(Address address) => Address = address ?? throw new DomainException("Address cannot be null", nameof(Address));
    #endregion

    #region Skills
    public void AddSkill(Skill? skill)
    {
        if (skill == null)
            throw new DomainException("Skill cannot be null", nameof(skill));
        if (_skills.Any(s => s.Id == skill.Value.Id))
            throw new DomainException("Skill already exists", nameof(skill));
        _skills.Add(skill.Value);
    }
    public void RemoveSkill(Guid? skillId)
    {
        if (skillId == Guid.Empty)
            throw new DomainException("Skill ID cannot be empty", nameof(skillId));

        Skill? skill = _skills.FirstOrDefault(s => s.Id == skillId);

        if (skill == null)
            throw new DomainException("Skill not found", nameof(skill));

        _skills.Remove(skill.Value);
    }
    public void ClearSkills() => _skills.Clear();
    #endregion
    
    #region Taskis
    public void AddTaski(Taski? taski)
    {
        if (taski == null)throw new DomainException("Taski cannot be null", nameof(taski));
            
        if (_taskis.Any(t => t.Id == taski.Id))throw new DomainException("Taski already exists", nameof(taski));

        if (_taskis.Count(x => x.Status == Enums.TaskiStatus.InProgress) == 3) 
            throw new DomainException("User can only have 3 taskis in progress at a time", nameof(taski));

        _taskis.Add(taski);
    }
    public void RemoveTaski(Guid? taskiId)
    {
        if (taskiId == Guid.Empty)
            throw new DomainException("Taski ID cannot be empty", nameof(taskiId));

        Taski? taski = _taskis.FirstOrDefault(t => t.Id == taskiId);

        if (taski == null)
            throw new DomainException("Taski not found", nameof(taski));

        _taskis.Remove(taski);
    }
    public void ClearTaskis() => _taskis.Clear();
    #endregion
 
}
