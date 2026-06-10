using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CONTRACT.DTO;

namespace BLL.Interfaces;

/// <summary>
/// Інтерфейс для керування електронним розкладом занять та пов'язаними довідниками.
/// </summary>
public interface IScheduleManager
{
    // ==========================================
    // Операції з елементами розкладу (ScheduleItem)
    // ==========================================

    /// <summary>
    /// Отримує розклад занять для конкретної академічної групи.
    /// </summary>
    Task<IReadOnlyCollection<ScheduleItemInfo>> GetScheduleForGroupAsync(int currentUserId, int groupId);

    /// <summary>
    /// Отримує розклад занять для конкретного викладача.
    /// </summary>
    Task<IReadOnlyCollection<ScheduleItemInfo>> GetScheduleForTeacherAsync(int currentUserId, int teacherId);

    /// <summary>
    /// Отримує розклад занять для конкретної навчальної аудиторії.
    /// </summary>
    Task<IReadOnlyCollection<ScheduleItemInfo>> GetScheduleForClassroomAsync(int currentUserId, int classroomId);

    /// <summary>
    /// Отримує сумарний розклад занять для всього підрозділу (кафедри/факультету).
    /// </summary>
    Task<IReadOnlyCollection<ScheduleItemInfo>> GetScheduleForDepartmentAsync(int currentUserId, int departmentId);

    /// <summary>
    /// Отримує окремий елемент розкладу за його ідентифікатором.
    /// </summary>
    Task<ScheduleItemInfo> GetScheduleItemByIdAsync(int currentUserId, int itemId);

    /// <summary>
    /// Отримує весь розклад. Використовується редактором розкладу.
    /// </summary>
    Task<IReadOnlyCollection<ScheduleItemInfo>> GetAllScheduleItemsAsync(int currentUserId);

    /// <summary>
    /// Додає нове заняття в розклад із перевіркою конфліктів часу та ресурсів.
    /// </summary>
    Task CreateScheduleItemAsync(int currentUserId, int groupId, int teacherId, int classroomId, int disciplineId, DayOfWeek dayOfWeek, int lessonNumber, string weekType);

    /// <summary>
    /// Оновлює інформацію про заняття в розкладі з повторною перевіркою конфліктів.
    /// </summary>
    Task UpdateScheduleItemAsync(int currentUserId, int itemId, int groupId, int teacherId, int classroomId, int disciplineId, DayOfWeek dayOfWeek, int lessonNumber, string weekType);

    /// <summary>
    /// Видаляє заняття з розкладу.
    /// </summary>
    Task DeleteScheduleItemAsync(int currentUserId, int itemId);

    // ==========================================
    // Отримання списків довідників (Reference Lists)
    // ==========================================

    /// <summary>
    /// Отримує список усіх підрозділів.
    /// </summary>
    Task<IReadOnlyCollection<DepartmentInfo>> GetAllDepartmentsAsync(int currentUserId);

    /// <summary>
    /// Отримує список усіх академічних груп.
    /// </summary>
    Task<IReadOnlyCollection<GroupInfo>> GetAllGroupsAsync(int currentUserId);

    /// <summary>
    /// Отримує список усіх викладачів.
    /// </summary>
    Task<IReadOnlyCollection<TeacherInfo>> GetAllTeachersAsync(int currentUserId);

    /// <summary>
    /// Отримує список усіх аудиторій.
    /// </summary>
    Task<IReadOnlyCollection<ClassroomInfo>> GetAllClassroomsAsync(int currentUserId);

    /// <summary>
    /// Отримує список усіх навчальних дисциплін.
    /// </summary>
    Task<IReadOnlyCollection<DisciplineInfo>> GetAllDisciplinesAsync(int currentUserId);

    // ==========================================
    // Керування довідниками (CRUD - тільки для Адміністраторів)
    // ==========================================

    /// <summary>
    /// Створює новий підрозділ.
    /// </summary>
    Task CreateDepartmentAsync(int currentUserId, string name);

    /// <summary>
    /// Оновлює підрозділ.
    /// </summary>
    Task UpdateDepartmentAsync(int currentUserId, int departmentId, string name);

    /// <summary>
    /// Видаляє підрозділ.
    /// </summary>
    Task DeleteDepartmentAsync(int currentUserId, int departmentId);

    /// <summary>
    /// Створює нову академічну групу із перевіркою формату назви через Regex.
    /// </summary>
    Task CreateGroupAsync(int currentUserId, string name, int departmentId);

    /// <summary>
    /// Оновлює академічну групу.
    /// </summary>
    Task UpdateGroupAsync(int currentUserId, int groupId, string name, int departmentId);

    /// <summary>
    /// Видаляє академічну групу.
    /// </summary>
    Task DeleteGroupAsync(int currentUserId, int groupId);

    /// <summary>
    /// Створює запис про викладача із вказанням вченого ступеня.
    /// </summary>
    Task CreateTeacherAsync(int currentUserId, string name, string degree, int departmentId, int? userId);

    /// <summary>
    /// Оновлює запис про викладача.
    /// </summary>
    Task UpdateTeacherAsync(int currentUserId, int teacherId, string name, string degree, int departmentId, int? userId);

    /// <summary>
    /// Видаляє запис про викладача.
    /// </summary>
    Task DeleteTeacherAsync(int currentUserId, int teacherId);

    /// <summary>
    /// Додає навчальну аудиторію.
    /// </summary>
    Task CreateClassroomAsync(int currentUserId, string name, string building, int capacity);

    /// <summary>
    /// Оновлює навчальну аудиторію.
    /// </summary>
    Task UpdateClassroomAsync(int currentUserId, int classroomId, string name, string building, int capacity);

    /// <summary>
    /// Видаляє навчальну аудиторію.
    /// </summary>
    Task DeleteClassroomAsync(int currentUserId, int classroomId);

    /// <summary>
    /// Додає нову навчальну дисципліну в довідник.
    /// </summary>
    Task CreateDisciplineAsync(int currentUserId, string name, int departmentId);

    /// <summary>
    /// Оновлює навчальну дисципліну в довіднику.
    /// </summary>
    Task UpdateDisciplineAsync(int currentUserId, int disciplineId, string name, int departmentId);

    /// <summary>
    /// Видаляє навчальну дисципліну з довідника.
    /// </summary>
    Task DeleteDisciplineAsync(int currentUserId, int disciplineId);
}
