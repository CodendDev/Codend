using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Codend.Infrastructure.Authorization;

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
    public static readonly OperationAuthorizationRequirement Edit = new() { Name = nameof(Edit) };
    
    /// <summary>
    /// Represents the "delete" operation authorization requirement,
    /// which allows to delete project.
    /// </summary>
    public static readonly OperationAuthorizationRequirement Delete = new() { Name = nameof(Delete) };
}