using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CONTRACT.DTO;
using BLL.Interfaces;
using BLL.Exceptions;
using BLL.Helpers;
using DAL.Interfaces;

namespace BLL.Services;

/// <summary>
/// Реалізація сервісу для управління профілями користувачів у системі.
/// </summary>
public sealed class UserManager : IUserManager
{
    private readonly IDataStore _store;
    private readonly IMapper _mapper;

    /// <summary>
    /// Ініціалізує новий екземпляр класу <see cref="UserManager"/>.
    /// </summary>
    /// <param name="store">Репозиторій Unit of Work</param>
    /// <param name="mapper">Конвертер AutoMapper</param>
    public UserManager(IDataStore store, IMapper mapper)
    {
        _store = store;
        _mapper = mapper;
    }

    /// <summary>
    /// Повертає список усіх зареєстрованих користувачів.
    /// </summary>
    public async Task<IReadOnlyCollection<UserInfo>> GetAllUsersAsync()
    {
        var users = await _store.Users.GetAllAsync();
        return _mapper.Map<IReadOnlyCollection<UserInfo>>(users);
    }

    /// <summary>
    /// Шукає користувача за унікальним ідентифікатором.
    /// </summary>
    /// <param name="id">Ідентифікатор користувача</param>
    /// <exception cref="EntityNotFoundException">Виникає, якщо користувача з вказаним ID немає в системі</exception>
    public async Task<UserInfo?> GetUserByIdAsync(int id)
    {
        var user = await _store.Users.GetAsync(id);
        if (user == null)
        {
            throw new EntityNotFoundException("Користувача не знайдено.");
        }
        return _mapper.Map<UserInfo>(user);
    }

    /// <summary>
    /// Оновлює дані існуючого користувача з перевіркою коректності нової ролі.
    /// </summary>
    /// <param name="id">Ідентифікатор користувача</param>
    /// <param name="username">Нове ім'я</param>
    /// <param name="email">Новий email</param>
    /// <param name="role">Нова роль (має належати до дозволених ролей)</param>
    /// <exception cref="EntityNotFoundException">Користувача не знайдено</exception>
    /// <exception cref="ArgumentException">Некоректні дані або недозволена роль</exception>
    public async Task UpdateUserAsync(int id, string username, string email, string role)
    {
        var user = await _store.Users.GetAsync(id);
        if (user == null)
        {
            throw new EntityNotFoundException("Користувача не знайдено.");
        }

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Ім'я та email є обов'язковими.");
        }

        user.Username = username;
        user.Email = email;

        // Перевіряємо та нормалізуємо роль за допомогою нашого хелпера перед оновленням
        if (!string.IsNullOrWhiteSpace(role))
        {
            user.Role = UserRoleHelper.ValidateAndNormalizeRole(role);
        }

        _store.Users.Update(user);
        await _store.CommitAsync();
    }

    /// <summary>
    /// Видаляє користувача за його ідентифікатором.
    /// </summary>
    /// <param name="userId">Ідентифікатор для видалення</param>
    /// <exception cref="EntityNotFoundException">Користувача не знайдено</exception>
    public async Task DeleteUserAsync(int userId)
    {
        var user = await _store.Users.GetAsync(userId);
        if (user == null)
        {
            throw new EntityNotFoundException("Користувача не знайдено.");
        }
        await _store.Users.DeleteAsync(userId);
        await _store.CommitAsync();
    }
}
