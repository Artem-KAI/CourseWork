using System.Threading.Tasks;
using CONTRACT.DTO;

namespace BLL.Interfaces;

/// <summary>
/// Інтерфейс для керування автентифікацією та реєстрацією користувачів.
/// </summary>
public interface ICredentialManager
{
    /// <summary>
    /// Перевіряє облікові дані користувача та повертає інформацію про нього в разі успіху.
    /// </summary>
    Task<UserInfo?> AuthenticateAsync(string email, string password);

    /// <summary>
    /// Реєструє нового користувача з перевіркою дозволених ролей.
    /// </summary>
    Task<UserInfo> RegisterAccountAsync(string username, string email, string password, string role);
}
