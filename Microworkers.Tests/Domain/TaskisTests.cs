using FluentResults;
using Microworkers.Domain.Core.Aggregates;
using Microworkers.Domain.Core.Enums;
using Microworkers.Domain.Core.Exceptions;
using Microworkers.Domain.Core.ValueObjects;

namespace Microworkers.Tests.Domain;
public class TaskisTests
{
    [Fact]
    public void Should_Validate_User_Has_Three_Taskis_InProgress()
    {
        Result<User> user = User.Create(
         Guid.NewGuid(),
         "John Doe",
         "123456789",
         "password1234",
         Phone.Create(123456789).Value,
         "johndoe@email.com"
     );
        List<Taski> taskis = new List<Taski>();
        Taski taski1 = Taski.Create(user.Value.Id, Guid.NewGuid(), Guid.NewGuid(), "Task 1", taskis);
        taskis.Add(taski1);
        taski1.StartTask();

        Taski taski2 = Taski.Create(user.Value.Id, Guid.NewGuid(), Guid.NewGuid(), "Task 2", taskis);
        taskis.Add(taski2);
        taski2.StartTask();

        Taski taski3 = Taski.Create(user.Value.Id, Guid.NewGuid(), Guid.NewGuid(), "Task 3", taskis);
        taskis.Add(taski3);
        taski3.StartTask();

        user.Value.AddTaski(taski1);
        user.Value.AddTaski(taski2);
        user.Value.AddTaski(taski3);

        // Act
        var inProgressTaskis = user.Value.Taskis.Count(t => t.Status == TaskiStatus.InProgress);

        // Assert
        Assert.Equal(3, inProgressTaskis);
    }

    [Fact]
    public void Should_Validate_User_Has_Max_Of_Three_Taskis_InProgress_()
    {

        Result<User> userer = User.Create(
          Guid.NewGuid(),
          "John Doe",
          "123456789",
          "password1234",
          Phone.Create(123456789).Value,
          "johndoe@email.com"
      );
        Guid customerId = Guid.NewGuid();
        List<Taski> taskis = new List<Taski>();
        Taski taski1 = Taski.Create(userer.Value.Id, Guid.NewGuid(), Guid.NewGuid(), "Task 1", taskis);
        taskis.Add(taski1);
        taski1.StartTask();

        Taski taski2 = Taski.Create(userer.Value.Id, Guid.NewGuid(), Guid.NewGuid(), "Task 2", taskis);
        taskis.Add(taski2);
        taski2.StartTask();

        Taski taski3 = Taski.Create(userer.Value.Id, Guid.NewGuid(), Guid.NewGuid(), "Task 3", taskis);
        taskis.Add(taski3);
        taski3.StartTask();

        userer.Value.AddTaski(taski1);
        userer.Value.AddTaski(taski2);
        userer.Value.AddTaski(taski3);

        // Act
        var inProgressTaskis = userer.Value.Taskis.Count(t => t.Status == TaskiStatus.InProgress);

        // Assert
        Assert.Equal(3, inProgressTaskis);

        // Trying to add a fourth taski in progress
        Assert.Throws<DomainException>(() => Taski.Create(userer.Value.Id, Guid.NewGuid(), Guid.NewGuid(), "Task 4", taskis));
    }
}
