﻿using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Extensions;
using Codend.Domain.ValueObjects.Abstractions;
using Codend.Domain.ValueObjects.Primitives;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.StringValueObject;

namespace Codend.Domain.ValueObjects;

/// <summary>
/// ProjectTaskStatus name value object.
/// </summary>
public sealed class ProjectTaskStatusName : StringValueObject, IStringValueObject<ProjectTaskStatusName>
{
    /// <summary>
    /// Maximum ProjectTaskStatus length.
    /// </summary>
    public static int MaxLength => 150;

    private ProjectTaskStatusName(string value) : base(value)
    {
    }

    /// <summary>
    /// Creates new <see cref="ProjectTaskStatusName"/> value object with given <paramref name="value"/> string.
    /// Additionally checks whether the maximum length is exceeded or string value is not null or empty.
    /// </summary>
    /// <param name="value">Name value for the ProjectTaskStatus.</param>
    /// <returns>The <see cref="Result"/> of creation. Contains <see cref="ProjectTaskStatusName"/> or an <see cref="DomainErrors.DomainError"/>.</returns>
    public static Result<ProjectTaskStatusName> Create(string value)
    {
        return Result
            .Ok(new ProjectTaskStatusName(value))
            .Ensure(() => !string.IsNullOrEmpty(value), new NullOrEmpty<ProjectTaskStatusName>())
            .Ensure(() => value.Length < MaxLength, new TooLong<ProjectTaskStatusName>());
    }
}