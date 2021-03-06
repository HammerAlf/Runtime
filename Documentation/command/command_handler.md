---
title: Handling Commands
description: Recommendations and Requirements when Handling a Command
keywords: 
author: einari, smithmx
aliases:
    - /runtime/runtime/command/command_handler
---

# Command Handler

## Background

A *Command Handler* handles commands.  The command handler is responsible for executing the appropriate methods on Aggregate Roots and Domain Services to fulfill the intent of the *Command*.

## Requirements

A *Command Handler* must implement the *IHandleCommands* interface.  This is a simple marker interface with no methods to implement.

To handle a *Command*, you create a method called *Handle* with a single parameter that is the *Command* you wish to handle.  A *Command Handler* does not return anything.  It is not permitted to return a value from a *Command*.  When a *Command* is executed, you will receive a *CommandResult* which will indicate whether the *Command* succeeded and, in the case of failure, why the *Command* failed. 

No entities or representations that were created or updated as a result of the *Command* are present on the *CommandResult*, as it is not meant to return such state. To inspect state (other than a result indicating success or failure) after sending a *Command* you can use the *read-side* with its queries and read-models.

```csharp
public void Handle(AddRecommendationToCart cmd)
```

There **MUST** be a single handler for a Command.  Dolittle will throw an exception if there is no handler for a command or if there is more than one handler for a command.  There can only be a single handler for a command as there must be a clear and unambiguous indication of the result of the execution of the command.  Multiple command handlers for a single command would require co-ordination of multiple results with associated rollback of successful handlers after unsuccessful ones.  Where you wish multiple things to happen on receipt of a Command, you must implement and handle this yourself in your *Command Handler*.

## Responsibilities

The sole responsibility of the *Command Handler* is to execute the *Command*.

A *Command Handler* should not implement any validation, security checks, or such like.  These should be handled earlier in the command pipeline by validators.  When we invoke a *Command Handler*, it is with the expectation that the action will succeed.  We should have validated the command,
checked business rule conditions, checked the permissions and therefore the command should succeed.  It is for this reason that within a *Command Handler*, the only way to indicate that the *Command* did not succeed is via an Exception.  This is because, due to our previous checks,
it is truly an exceptional condition for the *Command* to fail.

## Design

It is **REQUIRED** that a *Command Handler* invoke a single method on a single *Aggregate Root* or none at all. The *Command* is intended to be transactional, either succeeding or failing totally. Think of the *Command*, it's *Command Handler* and the single method on the *Aggregate Root* as one transaction that must either succeed or fail. It can be useful to think of a *Command* as a serialized call on a method on an *Aggregate Root*. If you could call multiple *Aggregate Roots* you would need to handle one of them failing and rolling back the transaction across multiple *Aggregate Roots*, which is likely impossible.

{{% notice tip %}}
Calling multiple *Aggregate Roots* from the same command handler for the same commands would indicate that you need to create a new *Aggregate Root* that captures this business transaction as one thing. In our experience it can be fruitful to consider modelling the verb, not the noun (i.e. the adding of a product to the cart, not the cart with an add -method).
{{% /notice %}}

As nothing can be returned from a *Command Handler*, we discourage reliance on database-generated identities or keys. The invoker of the *Command* would not be able to receive the Id generated by the database. We **RECOMMENDED** using meaningless but unique keys such as Guids on a *Command* which can be provided by the invoker of the *Command* and *Command Handler*. Where a database generated key is required, it is recommended to include another candidate key that can be provided by the caller.
