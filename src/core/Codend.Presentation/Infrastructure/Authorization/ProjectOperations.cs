using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Codend.Presentation.Infrastructure.Authorization;

/// <summary>
/// This class defines authorization requirements for various operations on the project itself and
/// all its dependent entities.
/// </summary>
public static class ProjectOperations
{
    /// <summary>
    /// Represents the "edit" operation authorization requirement,
    /// which allows to manage project properties and entities.
    /// </summary>
    public static readonly OperationAuthorizationRequirement Owner = new() { Name = nameof(Owner) };
    
    /// <summary>
    /// Represents the "delete" operation authorization requirement,
    /// which allows to delete project.
    /// </summary>
    public static readonly OperationAuthorizationRequirement Member = new() { Name = nameof(Member) };
}