﻿namespace Codend.Contracts.Responses;

/// <summary>
/// Represents default user response.
/// </summary>
/// <param name="Id">User id.</param>
/// <param name="FirstName">User first name.</param>
/// <param name="LastName">User last name.</param>
/// <param name="Email">User email.</param>
/// <param name="ImageUrl">The URL that points to an image file that is user's profile image.</param>
public record UserDetails
(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string ImageUrl
)
{
    /// <inheritdoc />
    public override string ToString() => $"{FirstName} {LastName} {Email}";
}