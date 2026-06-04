using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CONTRACT.DTO;
using BLL.Interfaces;
using BLL.Helpers;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services;

/// <summary>
/// Реалізація сервісу керування обліковими даними (автентифікація та реєстрація).
/// </summary>
public sealed class CredentialManager : ICredentialManager
{
    private readonly IDataStore _store;
    private readonly IMapper _mapper;

    /// <summary>
    /// Ініціалізує новий екземпляр класу <see cref="CredentialManager"/>.
    /// </summary>
    /// <param name="store">Репозиторій-сховище Unit of Work</param>
    /// <param name="mapper">Конвертер AutoMapper</param>
    public CredentialManager(IDataStore store, IMapper mapper)
    {
        _store = store;
        _mapper = mapper;
    }

    /// <summary>
    /// Автентифікує користувача за email та паролем.
    /// </summary>
    /// <param name="email">Електронна адреса</param>
    /// <param name="password">Пароль (у відкритому вигляді)</param>
    /// <returns>Дані про користувача в разі успіху; інакше null</returns>
    public async Task<UserInfo?> AuthenticateAsync(string email, string password)
    {
        var allUsers = await _store.Users.GetAllAsync();
        
        // Шукаємо користувача без урахування регістру символів у email
        var user = allUsers.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        if (user == null)
        {
            return null;
        }

        // Хешуємо введений пароль та порівнюємо зі збереженим
        string hashedInput = HashPassword(password);
        if (user.PasswordHash != hashedInput)
        {
            return null;
        }

        return _mapper.Map<UserInfo>(user);
    }

    /// <summary>
    /// Реєструє нового користувача в системі з суворою валідацією ролі.
    /// </summary>
    /// <param name="username">Ім'я користувача</param>
    /// <param name="email">Електронна пошта</param>
    /// <param name="password">Пароль</param>
    /// <param name="role">Роль користувача (наприклад, студент, вчитель, адміністратор тощо)</param>
    /// <returns>Інформація про створеного користувача</returns>
    /// <exception cref="ArgumentException">Виникає у разі некоректних даних або вже існуючого імені/email</exception>
    public async Task<UserInfo> RegisterAccountAsync(string username, string email, string password, string role)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Всі поля обов'язкові для заповнення.");
        }

        var allUsers = await _store.Users.GetAllAsync();

        // Перевіряємо унікальність імені
        if (allUsers.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
        {
            throw new ArgumentException("Користувач з таким ім'ям вже існує.");
        }

        // Перевіряємо унікальність email
        if (allUsers.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
        {
            throw new ArgumentException("Користувач з таким email вже існує.");
        }

        // Строго валідуємо роль через наш хелпер (запобігаємо несанкціонованим ролям)
        string normRole = UserRoleHelper.ValidateAndNormalizeRole(role);

        var user = new User
        {
            Username = username,
            Email = email,
            PasswordHash = HashPassword(password),
            Role = normRole
        };

        await _store.Users.CreateAsync(user);
        await _store.CommitAsync();

        return _mapper.Map<UserInfo>(user);
    }

    /// <summary>
    /// Обчислює криптографічний SHA-256 хеш пароля для безпечного збереження.
    /// </summary>
    private string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        var sb = new StringBuilder();
        foreach (var b in bytes)
        {
            sb.Append(b.ToString("x2"));
        }
        return sb.ToString();
    }
}
