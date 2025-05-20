using Microworkers.Domain.Core.Aggregates;
using Microworkers.Domain.Core.Enums;
using Microworkers.Domain.Core.Exceptions;
using Microworkers.Tests.TestData;

namespace Microworkers.Tests.Domain;
public class TaskisTests
{

    [Fact]
    public void Should_Validate_User_Has_Three_Taskis_InProgress()
    {
        User user = UserTestData.GenerateValidUser();

        List<Taski> taskis = new List<Taski>();
        Taski taski1 = Taski.Create(user.Id, Guid.NewGuid(), Guid.NewGuid(), "Task 1", taskis);
        taskis.Add(taski1);
        taski1.StartTask();

        Taski taski2 = Taski.Create(user.Id, Guid.NewGuid(), Guid.NewGuid(), "Task 2", taskis);
        taskis.Add(taski2);
        taski2.StartTask();

        Taski taski3 = Taski.Create(user.Id, Guid.NewGuid(), Guid.NewGuid(), "Task 3", taskis);
        taskis.Add(taski3);
        taski3.StartTask();

        user.AddTaski(taski1);
        user.AddTaski(taski2);
        user.AddTaski(taski3);

        // Act
        var inProgressTaskis = user.Taskis.Count(t => t.Status == TaskiStatus.InProgress);

        // Assert
        Assert.Equal(3, inProgressTaskis);
    }

    [Fact]
    public void Should_Validate_User_Has_Max_Of_Three_Taskis_InProgress_()
    {
        var user = UserTestData.GenerateValidUser();

        Guid customerId = Guid.NewGuid();
        List<Taski> taskis = new List<Taski>();
        Taski taski1 = Taski.Create(user.Id, Guid.NewGuid(), Guid.NewGuid(), "Task 1", taskis);
        taskis.Add(taski1);
        taski1.StartTask();

        Taski taski2 = Taski.Create(user.Id, Guid.NewGuid(), Guid.NewGuid(), "Task 2", taskis);
        taskis.Add(taski2);
        taski2.StartTask();

        Taski taski3 = Taski.Create(user.Id, Guid.NewGuid(), Guid.NewGuid(), "Task 3", taskis);
        taskis.Add(taski3);
        taski3.StartTask();

        user.AddTaski(taski1);
        user.AddTaski(taski2);
        user.AddTaski(taski3);

        // Act
        var inProgressTaskis = user.Taskis.Count(t => t.Status == TaskiStatus.InProgress);

        // Assert
        Assert.Equal(3, inProgressTaskis);

        // Trying to add a fourth taski in progress
        Assert.Throws<DomainException>(() => Taski.Create(user.Id, Guid.NewGuid(), Guid.NewGuid(), "Task 4", taskis));
    }
}
