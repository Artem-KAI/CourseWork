using System;

namespace BLL.Helpers;

/// <summary>
/// Допоміжний клас для строгої валідації та нормалізації ролей користувачів.
/// Забезпечує, що користувачі можуть бути лише: студент, вчитель, адміністратор, редактор або управлінський склад.
/// </summary>
public static class UserRoleHelper
{
    /// <summary>
    /// Перевіряє вхідне значення ролі (українською або англійською) та повертає її нормалізований варіант.
    /// Якщо роль неприпустима, викидає ArgumentException.
    /// </summary>
    /// <param name="role">Назва ролі для перевірки</param>
    /// <returns>Нормалізована назва ролі (Admin, Editor, Management, Teacher, Student)</returns>
    /// <exception cref="ArgumentException">Виникає, якщо роль не входить до списку дозволених</exception>
    public static string ValidateAndNormalizeRole(string role)
    {
        if (string.IsNullOrWhiteSpace(role))
        {
            throw new ArgumentException("Роль користувача є обов'язковою.");
        }

        string r = role.Trim().ToLower();
        return r switch
        {
            "admin" or "administrator" or "адміністратор" => "Admin",
            "editor" or "редактор" => "Editor",
            "management" or "управлінський склад" or "управлінський" => "Management",
            "teacher" or "вчитель" => "Teacher",
            "student" or "студент" => "Student",
            _ => throw new ArgumentException("Неприпустима роль користувача. Допустимі ролі: Student (студент), Teacher (вчитель), Admin (адміністратор), Editor (редактор), Management (управлінський склад).")
        };
    }
}
