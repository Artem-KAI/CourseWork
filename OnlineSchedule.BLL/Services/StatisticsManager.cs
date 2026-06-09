using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CONTRACT.DTO;
using BLL.Interfaces;
using BLL.Exceptions;
using DAL.Interfaces;

namespace BLL.Services;

/// <summary>
/// Реалізація сервісу збору статистики навантаження навчальних ресурсів (викладачі, аудиторії, групи).
/// </summary>
public sealed class StatisticsManager : IStatisticsManager
{
    private readonly IDataStore _store;

    /// <summary>
    /// Ініціалізує новий екземпляр класу <see cref="StatisticsManager"/>.
    /// </summary>
    /// <param name="store">Репозиторій Unit of Work</param>
    public StatisticsManager(IDataStore store)
    {
        _store = store;
    }

    /// <summary>
    /// Допоміжний метод для перевірки доступу користувача до статистики (лише Admin та Management).
    /// </summary>
    private async Task ValidateAccessAsync(int currentUserId)
    {
        var user = await _store.Users.GetAsync(currentUserId);
        if (user == null)
        {
            throw new AccessDeniedException("Користувач не авторизований.");
        }

        // Статистика доступна тільки адміністраторам та управлінському складу
        if (!string.Equals(user.Role, "Admin", StringComparison.OrdinalIgnoreCase) && !string.Equals(user.Role, "Management", StringComparison.OrdinalIgnoreCase))
        {
            throw new AccessDeniedException("Доступ до статистики дозволений лише Адміністраторам та Управлінському складу.");
        }
    }

    /// <summary>
    /// Розраховує кількість проведених занять для кожного викладача.
    /// Результат відсортовано за спаданням загальної кількості занять.
    /// </summary>
    public async Task<IReadOnlyCollection<TeacherWorkloadDto>> GetTeachersWorkloadAsync(int currentUserId)
    {
        await ValidateAccessAsync(currentUserId);

        var teachers = await _store.Teachers.GetAllAsync();
        var items = await _store.ScheduleItems.GetAllAsync();

        var workload = new List<TeacherWorkloadDto>();

        foreach (var teacher in teachers)
        {
            int count = 0;
            foreach (var item in items)
            {
                if (item.TeacherId == teacher.Id)
                {
                    count++;
                }
            }
            workload.Add(new TeacherWorkloadDto(teacher.Id, teacher.Name, count));
        }

        return workload.OrderByDescending(w => w.TotalLessons).ToList();
    }

    /// <summary>
    /// Розраховує відсоток завантаженості аудиторій, виходячи з 36 можливих слотів на тиждень (6 днів по 6 пар).
    /// Результат відсортовано за спаданням відсотка утилізації.
    /// </summary>
    public async Task<IReadOnlyCollection<ClassroomUtilizationDto>> GetClassroomsUtilizationAsync(int currentUserId)
    {
        await ValidateAccessAsync(currentUserId);

        var classrooms = await _store.Classrooms.GetAllAsync();
        var items = await _store.ScheduleItems.GetAllAsync();

        var utilization = new List<ClassroomUtilizationDto>();
        // Загальна кількість можливих слотів на тиждень (наприклад, 6 днів по 6 занять на день)
        double totalSlotsPerWeek = 36.0;

        foreach (var classroom in classrooms)
        {
            int count = 0;
            foreach (var item in items)
            {
                if (item.ClassroomId == classroom.Id)
                {
                    count++;
                }
            }
            // Розраховуємо відсоток завантаження та округлюємо до 2 знаків після коми
            double percentage = Math.Round((count / totalSlotsPerWeek) * 100, 2);
            utilization.Add(new ClassroomUtilizationDto(classroom.Id, classroom.Name, count, percentage));
        }

        return utilization.OrderByDescending(u => u.UtilizationPercentage).ToList();
    }

    /// <summary>
    /// Обчислює загальну кількість занять та середнє завантаження на день для кожної академічної групи.
    /// Результат відсортовано за спаданням загальної кількості занять.
    /// </summary>
    public async Task<IReadOnlyCollection<GroupLoadDto>> GetGroupsLoadAsync(int currentUserId)
    {
        await ValidateAccessAsync(currentUserId);

        var groups = await _store.Groups.GetAllAsync();
        var items = await _store.ScheduleItems.GetAllAsync();

        var load = new List<GroupLoadDto>();

        foreach (var group in groups)
        {
            int count = 0;
            foreach (var item in items)
            {
                if (item.GroupId == group.Id)
                {
                    count++;
                }
            }
            // Розраховуємо середню кількість занять на день (для 6 робочих днів)
            double avgPerDay = Math.Round(count / 6.0, 2);
            load.Add(new GroupLoadDto(group.Id, group.Name, count, avgPerDay));
        }

        return load.OrderByDescending(l => l.TotalLessons).ToList();
    }
}
