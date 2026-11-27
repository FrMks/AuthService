using Microsoft.AspNetCore.Identity;

namespace AuthService.Domain;

/// <summary>
/// Роль, которую можно назначить нескольким пользователям
/// </summary>
public sealed class Role : IdentityRole<Guid>
{
    
}