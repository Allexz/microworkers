using Microworkers.Domain.Core.Aggregates;
using Microworkers.Domain.Core.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microworkers.Tests.TestData;
public static class TaskiTestData
{
    public static Taski GenerateValidTaski()
    {
        return TaskiFactory.Create(
            customer: Guid.NewGuid(),
            serviceProvider: UserTestData.GenerateValidUser(),  
            requiredSkill: Guid.NewGuid(),
            description: "This is a valid task description.").Value;
    }
}
