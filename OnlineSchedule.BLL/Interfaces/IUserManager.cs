using System.Collections.Generic;
using System.Threading.Tasks;
using CONTRACT.DTO;

namespace BLL.Interfaces;

/// <summary>
/// Інтерфейс для керування обліковими записами користувачів у системі.
/// </summary>
public interface IUserManager
{
    /// <summary>
    /// Отримує всіх користувачів системи.
    /// </summary>
    Task<IReadOnlyCollection<UserInfo>> GetAllUsersAsync();

    /// <summary>
    /// Отримує користувача за його ідентифікатором.
    /// </summary>
    Task<UserInfo?> GetUserByIdAsync(int id);

    /// <summary>
    /// Оновлює дані користувача з обов'язковою перевіркою дозволених ролей.
    /// </summary>
    Task UpdateUserAsync(int id, string username, string email, string role);

    /// <summary>
    /// Видаляє користувача за його ідентифікатором.
    /// </summary>
    Task DeleteUserAsync(int userId);
}
