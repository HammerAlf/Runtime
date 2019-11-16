﻿/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Reflection;
using Dolittle.Globalization;
using Dolittle.Logging;
using Dolittle.Runtime.Transactions;
using Dolittle.Runtime.Commands;
using Dolittle.Runtime.Commands.Handling;
using Dolittle.Runtime.Commands.Security;
using Dolittle.Runtime.Commands.Validation;
using Dolittle.Rules;
using System.Collections.Generic;
using Dolittle.Collections;
using System.Linq;
using Dolittle.Events;

namespace Dolittle.Runtime.Commands.Coordination
{
    /// <summary>
    /// Represents a <see cref="ICommandCoordinator">ICommandCoordinator</see>
    /// </summary>
    public class CommandCoordinator : ICommandCoordinator
    {
        readonly ICommandHandlerManager _commandHandlerManager;
        readonly ICommandContextManager _commandContextManager;
        readonly ICommandValidators _commandValidationService;
        readonly ICommandSecurityManager _commandSecurityManager;
        readonly IRuleContexts _ruleContexts;
        readonly ILocalizer _localizer;
        readonly ILogger _logger;


        /// <summary>
        /// Initializes a new instance of the <see cref="CommandCoordinator">CommandCoordinator</see>
        /// </summary>
        /// <param name="commandHandlerManager">A <see cref="ICommandHandlerManager"/> for handling commands</param>
        /// <param name="commandContextManager">A <see cref="ICommandContextManager"/> for establishing a <see cref="CommandContext"/></param>
        /// <param name="commandSecurityManager">A <see cref="ICommandSecurityManager"/> for dealing with security and commands</param>
        /// <param name="commandValidators">A <see cref="ICommandValidators"/> for validating a <see cref="CommandRequest"/> before handling</param>
        /// <param name="ruleContexts"></param>
        /// <param name="localizer">A <see cref="ILocalizer"/> to use for controlling localization of current thread when handling commands</param>
        /// <param name="logger"><see cref="ILogger"/> to log with</param>
        public CommandCoordinator(
            ICommandHandlerManager commandHandlerManager,
            ICommandContextManager commandContextManager,
            ICommandSecurityManager commandSecurityManager,
            ICommandValidators commandValidators,
            IRuleContexts ruleContexts,
            ILocalizer localizer,
            ILogger logger)
        {
            _commandHandlerManager = commandHandlerManager;
            _commandContextManager = commandContextManager;
            _commandSecurityManager = commandSecurityManager;
            _commandValidationService = commandValidators;
            _localizer = localizer;
            _logger = logger;
            _ruleContexts = ruleContexts;
        }

        /// <inheritdoc/>
        public CommandResult Handle(CommandRequest command)
        {
            return Handle(_commandContextManager.EstablishForCommand(command), command);
        }

        CommandResult Handle(ICommandContext commandContext, CommandRequest command)
        {
            var commandResult = new CommandResult();
            try
            {
                using (_localizer.BeginScope())
                {
                    _logger.Information($"Handle command of type {command.Type}");

                    commandResult = CommandResult.ForCommand(command);

                    _logger.Trace("Authorize");
                    var authorizationResult = _commandSecurityManager.Authorize(command);
                    if (!authorizationResult.IsAuthorized)
                    {
                        _logger.Trace("Command not authorized");
                        commandResult.SecurityMessages = authorizationResult.BuildFailedAuthorizationMessages();
                        commandContext.Rollback();
                        return commandResult;
                    }

                    _logger.Trace("Validate");
                    var validationResult = _commandValidationService.Validate(command);
                    commandResult.ValidationResults = validationResult.ValidationResults;
                    commandResult.CommandValidationMessages = validationResult.CommandErrorMessages;

                    if (commandResult.Success)
                    {
                        _logger.Trace("Command is considered valid");
                        try
                        {
                            _logger.Trace("Handle the command");
                            _commandHandlerManager.Handle(command);

                            _logger.Trace("Collect any broken rules");
                            commandResult.BrokenRules = commandContext.GetObjectsBeingTracked()
                                .SelectMany(_ => _.BrokenRules)
                                .Select(_ => new BrokenRuleResult(
                                    _.Rule.GetType().Name,
                                    $"EventSource: {_.Instance.GetType().Name} - with id {((IEventSource)_.Context.Target).EventSourceId.Value}",
                                    _.Reasons));

                            _logger.Trace("Commit transaction");
                            commandContext.Commit();
                        }
                        catch (TargetInvocationException ex)
                        {
                            _logger.Error(ex, "Error handling command");
                            commandResult.Exception = ex.InnerException;
                            commandContext.Rollback();
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(ex, "Error handling command");
                            commandResult.Exception = ex;
                            commandContext.Rollback();
                        }
                    }
                    else
                    {
                        _logger.Information("Command was not successful, rolling back");
                        commandContext.Rollback();
                    }
                }
            }
            catch (TargetInvocationException ex)
            {
                _logger.Error(ex, "Error handling command");
                commandResult.Exception = ex.InnerException;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error handling command");
                commandResult.Exception = ex;
            }

            return commandResult;
        }
    }
}