using System.Collections.Generic;
using System.Threading.Tasks;
using CONTRACT.DTO;

namespace BLL.Interfaces;

/// <summary>
/// Інтерфейс для отримання аналітичних даних та статистики навантаження.
/// </summary>
public interface IStatisticsManager
{
    /// <summary>
    /// Отримує статистику навантаження викладачів (кількість проведених занять).
    /// </summary>
    Task<IReadOnlyCollection<TeacherWorkloadDto>> GetTeachersWorkloadAsync(int currentUserId);

    /// <summary>
    /// Отримує статистику завантаженості аудиторій у відсотковому відношенні.
    /// </summary>
    Task<IReadOnlyCollection<ClassroomUtilizationDto>> GetClassroomsUtilizationAsync(int currentUserId);

    /// <summary>
    /// Отримує обсяг навчального навантаження для студентських груп.
    /// </summary>
    Task<IReadOnlyCollection<GroupLoadDto>> GetGroupsLoadAsync(int currentUserId);
}
