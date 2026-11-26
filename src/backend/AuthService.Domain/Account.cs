using Microsoft.AspNetCore.Identity;

namespace AuthService.Domain;

// IdentityUser<Guid> уже содержит email, нормализованные значения, хэш пароля, lockout-поля и security stamp
// Повторно добавлять их в Account нельзя
public sealed class Account : IdentityUser<Guid>
{
    
}
