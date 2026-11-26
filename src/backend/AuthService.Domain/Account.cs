using Microsoft.AspNetCore.Identity;

namespace AuthService.Domain;

// IdentityUser<Guid> уже содержит email, нормализованные значения, хэш пароля, lockout-поля и security stamp
// Повторно добавлять их в Account нельзя
/// <summary>
/// Учетные данные и состояние пользователя: email, хэш пароля, lockout и secutiry stamp
/// </summary>
public sealed class Account : IdentityUser<Guid>
{
    
}
