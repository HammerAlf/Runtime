﻿using System;
using Machine.Specifications;

namespace Dolittle.Runtime.Commands.Coordination.Specs.for_CommandCoordinator
{
    [Subject(typeof(CommandCoordinator))]
    public class when_handling_command_and_an_exception_occurs_during_authorization : given.a_command_coordinator
    {
        static CommandResult result;
        static Exception exception;

        Establish context = () =>
        {
            exception = new Exception();
            command_security_manager_mock.Setup(cvs => cvs.Authorize(command)).Throws(exception);
        };

        Because of = () => result = coordinator.Handle(command);

        It should_have_exception_in_result = () => result.Exception.ShouldEqual(exception);
        It should_have_success_set_to_false = () => result.Success.ShouldBeFalse();
    }
}