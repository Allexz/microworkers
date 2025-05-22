using Microworkers.Domain.Core.Entities;
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
        private set => address = value ?? throw new ArgumentNullException(nameof(Address));
    }
    public IReadOnlyCollection<Skill> Skills => _skills.AsReadOnly();
    private readonly List<Skill> _skills = new();
    public IReadOnlyCollection<Taski> Taskis => _taskis.AsReadOnly();
    private readonly List<Taski> _taskis = new();

    private readonly List<Proposal> _proposals = new();
    public IReadOnlyCollection<Proposal> Proposals => _proposals.AsReadOnly();

    private User() { }
    internal User(Guid id, string name, string document, string password, Phone phone, string username, Address address)
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
        => Phone = newPhone ?? throw new DomainException("Phone cannot be null", nameof(newPhone));

    public void ChangeAddress(Address newAddress)
        => address = newAddress ?? throw new DomainException("Address cannot be null", nameof(newAddress));
    #endregion

    #region Skills
    public void AddSkill(Skill skill)
    {
        skill = skill ?? throw new DomainException("Skill cannot be null", nameof(skill));
        _skills.Add(skill);

    }

    public void RemoveSkill(Skill skill)
    {
        skill = skill ?? throw new DomainException("Skill cannot be null", nameof(skill));
        _skills.Remove(skill);
    }

    public void ClearSkills() => _skills.Clear();
    #endregion

    #region Taskis
    public void AddTaski(Taski taski)
    {
        taski = taski ?? throw new DomainException("Taski cannot be null", nameof(taski));
        _taskis.Add(taski);
    }

    public void RemoveTaski(Taski taski)
    {
        taski = taski ?? throw new DomainException("Taski cannot be null", nameof(taski));
        _taskis.Remove(taski);
    }

    public void ClearTaskis() => _taskis.Clear();
    #endregion

    #region Proposals
    public void AddProposal(Proposal proposal)
    {
        proposal = proposal ?? throw new DomainException("Proposal cannot be null", nameof(proposal));
        _proposals.Add(proposal);
    }
    public void RemoveProposal(Proposal proposal)
    {
        proposal = proposal ?? throw new DomainException("Proposal cannot be null", nameof(proposal));
        _proposals.Remove(proposal);
    }
    public void ClearProposals() => _proposals.Clear();
    #endregion
}