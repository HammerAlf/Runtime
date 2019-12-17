﻿// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Machine.Specifications;

namespace Dolittle.Tasks.Specs.for_TaskManager
{
    public class when_starting : given.a_task_manager_with_one_reporter
    {
        static OurTask task;
        static OurTask result;

        Establish context = () =>
        {
            task = new OurTask
            {
                CurrentOperation = 1
            };
            container.Setup(c => c.Get<OurTask>()).Returns(task);
        };

        Because of = () => result = task_manager.Start<OurTask>();

        It should_return_the_created_task = () => result.ShouldEqual(task);
        It should_call_begin_on_the_task = () => task.BeginCalled.ShouldBeTrue();
        It should_execute_the_task = () => task_scheduler.Verify(t => t.Start(task, Moq.It.IsAny<Action<Task>>()), Moq.Times.Once());
        It should_reset_current_operation = () => task.CurrentOperation.ShouldEqual(0);
        It should_call_the_status_reporter = () => task_status_reporter.Verify(t => t.Started(task), Moq.Times.Once());
        It should_save_task = () => task_repository.Verify(t => t.Save(task), Moq.Times.Once());
    }
}
