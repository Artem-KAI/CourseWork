using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CONTRACT.DTO;
using BLL.Interfaces;
using BLL.Enums;
using BLL.Exceptions;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services;

/// <summary>
/// Реалізація сервісу керування розкладом, довідниками та валідацією конфліктів розкладу.
/// </summary>
public sealed class ScheduleManager : IScheduleManager
{
    private readonly IDataStore _store;
    private readonly IMapper _mapper;

    // Регулярний вираз для валідації назви групи (наприклад, ПІ-221, КН-11, IPS-42)
    private static readonly System.Text.RegularExpressions.Regex GroupNameRegex = new(@"^[A-ZА-ЯІЄЇҐ]{2,4}-\d{2,4}$", System.Text.RegularExpressions.RegexOptions.Compiled);

    /// <summary>
    /// Ініціалізує новий екземпляр класу <see cref="ScheduleManager"/>.
    /// </summary>
    /// <param name="store">Репозиторій Unit of Work</param>
    /// <param name="mapper">Конвертер AutoMapper</param>
    public ScheduleManager(IDataStore store, IMapper mapper)
    {
        _store = store;
        _mapper = mapper;
    }

    #region Access Validation Helper
    /// <summary>
    /// Перевіряє існування користувача та його роль на відповідність дозволеним ролям.
    /// </summary>
    private async Task<User> GetAndValidateUserAsync(int currentUserId, string[]? allowedRoles = null, string restrictionMessage = "У вас немає прав для виконання цієї операції.")
    {
        var user = await _store.Users.GetAsync(currentUserId);
        if (user == null)
        {
            throw new AccessDeniedException("Користувач не авторизований.");
        }

        if (allowedRoles != null)
        {
            bool hasAccess = false;
            foreach (var role in allowedRoles)
            {
                if (user.Role == role)
                {
                    hasAccess = true;
                    break;
                }
            }
            if (!hasAccess)
            {
                throw new AccessDeniedException(restrictionMessage);
            }
        }

        return user;
    }
    #endregion

    #region ScheduleItem CRUD
    /// <summary>
    /// Повертає список занять для певної групи. Доступно для всіх авторизованих користувачів.
    /// </summary>
    public async Task<IReadOnlyCollection<ScheduleItemInfo>> GetScheduleForGroupAsync(int currentUserId, int groupId)
    {
        await GetAndValidateUserAsync(currentUserId);
        
        var group = await _store.Groups.GetAsync(groupId);
        if (group == null)
        {
            throw new EntityNotFoundException("Групу не знайдено.");
        }

        var allItems = await _store.ScheduleItems.GetAllAsync();
        var items = new List<ScheduleItem>();
        foreach (var s in allItems)
        {
            if (s.GroupId == groupId)
            {
                items.Add(s);
            }
        }

        return _mapper.Map<IReadOnlyCollection<ScheduleItemInfo>>(items);
    }

    /// <summary>
    /// Повертає список занять для певного викладача з перевіркою обмежень доступу.
    /// </summary>
    /// <exception cref="AccessDeniedException">Викладач може бачити тільки свій розклад, студент не має доступу</exception>
    public async Task<IReadOnlyCollection<ScheduleItemInfo>> GetScheduleForTeacherAsync(int currentUserId, int teacherId)
    {
        var user = await GetAndValidateUserAsync(currentUserId);
        
        var teacher = await _store.Teachers.GetAsync(teacherId);
        if (teacher == null)
        {
            throw new EntityNotFoundException("Викладача не знайдено.");
        }

        if (user.Role == "Teacher")
        {
            if (teacher.UserId != user.Id)
            {
                throw new AccessDeniedException("Викладачі можуть переглядати тільки свій власний розклад.");
            }
        }
        else if (user.Role == "Student")
        {
            throw new AccessDeniedException("Студенти не можуть переглядати розклад конкретних викладачів.");
        }

        var allItems = await _store.ScheduleItems.GetAllAsync();
        var items = new List<ScheduleItem>();
        foreach (var s in allItems)
        {
            if (s.TeacherId == teacherId)
            {
                items.Add(s);
            }
        }

        return _mapper.Map<IReadOnlyCollection<ScheduleItemInfo>>(items);
    }

    /// <summary>
    /// Повертає список занять для певної аудиторії. Доступно для Admin, Editor, Management.
    /// </summary>
    public async Task<IReadOnlyCollection<ScheduleItemInfo>> GetScheduleForClassroomAsync(int currentUserId, int classroomId)
    {
        var user = await GetAndValidateUserAsync(currentUserId);
        
        if (user.Role == "Teacher" || user.Role == "Student")
        {
            throw new AccessDeniedException("У вас немає прав для перегляду розкладу аудиторій.");
        }

        var classroom = await _store.Classrooms.GetAsync(classroomId);
        if (classroom == null)
        {
            throw new EntityNotFoundException("Аудиторію не знайдено.");
        }

        var allItems = await _store.ScheduleItems.GetAllAsync();
        var items = new List<ScheduleItem>();
        foreach (var s in allItems)
        {
            if (s.ClassroomId == classroomId)
            {
                items.Add(s);
            }
        }

        return _mapper.Map<IReadOnlyCollection<ScheduleItemInfo>>(items);
    }

    /// <summary>
    /// Повертає список занять для підрозділу. Доступно для Admin, Editor, Management.
    /// </summary>
    public async Task<IReadOnlyCollection<ScheduleItemInfo>> GetScheduleForDepartmentAsync(int currentUserId, int departmentId)
    {
        var user = await GetAndValidateUserAsync(currentUserId);

        if (user.Role == "Teacher" || user.Role == "Student")
        {
            throw new AccessDeniedException("У вас немає прав для перегляду розкладу підрозділів.");
        }

        var department = await _store.Departments.GetAsync(departmentId);
        if (department == null)
        {
            throw new EntityNotFoundException("Підрозділ не знайдено.");
        }

        var allItems = await _store.ScheduleItems.GetAllAsync();
        var items = new List<ScheduleItem>();
        
        foreach (var s in allItems)
        {
            if ((s.Group != null && s.Group.DepartmentId == departmentId) || 
                (s.Teacher != null && s.Teacher.DepartmentId == departmentId))
            {
                bool exists = false;
                foreach (var added in items)
                {
                    if (added.Id == s.Id)
                    {
                        exists = true;
                        break;
                    }
                }
                if (!exists)
                {
                    items.Add(s);
                }
            }
        }

        return _mapper.Map<IReadOnlyCollection<ScheduleItemInfo>>(items);
    }

    /// <summary>
    /// Отримує один елемент розкладу за його унікальним ID.
    /// </summary>
    public async Task<ScheduleItemInfo> GetScheduleItemByIdAsync(int currentUserId, int itemId)
    {
        await GetAndValidateUserAsync(currentUserId);

        var item = await _store.ScheduleItems.GetAsync(itemId);
        if (item == null)
        {
            throw new EntityNotFoundException("Елемент розкладу не знайдено.");
        }

        return _mapper.Map<ScheduleItemInfo>(item);
    }

    /// <summary>
    /// Повертає весь розклад. Доступно для Admin та Editor.
    /// </summary>
    public async Task<IReadOnlyCollection<ScheduleItemInfo>>
        GetAllScheduleItemsAsync(int currentUserId)
    {
        await GetAndValidateUserAsync(currentUserId, new[] { "Admin", "Editor" }, "Тільки Адміністратори та Редактори можуть переглядати редактор розкладу.");

        var items = await _store.ScheduleItems.GetAllAsync();

        return _mapper.Map<IReadOnlyCollection<ScheduleItemInfo>>(items);
    }

    /// <summary>
    /// Додає нову пару в розклад. Перевіряє права (Admin, Editor), наявність сутностей та конфлікти часу.
    /// </summary>
    public async Task CreateScheduleItemAsync(int currentUserId, int groupId, int teacherId, int classroomId, int disciplineId, DayOfWeek dayOfWeek, int lessonNumber, string weekType)
    {
        await GetAndValidateUserAsync(currentUserId, new[] { "Admin", "Editor" }, "Тільки Адміністратори та Редактори можуть створювати розклад.");

        await ValidateEntityExistenceAsync(groupId, teacherId, classroomId, disciplineId);

        WeekType wType = ParseWeekType(weekType);

        await CheckConflictsAsync(null, groupId, teacherId, classroomId, dayOfWeek, lessonNumber, wType);

        var item = new ScheduleItem
        {
            GroupId = groupId,
            TeacherId = teacherId,
            ClassroomId = classroomId,
            DisciplineId = disciplineId,
            DayOfWeek = dayOfWeek,
            LessonNumber = lessonNumber,
            WeekType = wType
        };

        await _store.ScheduleItems.CreateAsync(item);
        await _store.CommitAsync();
    }

    /// <summary>
    /// Оновлює пару в розкладі. Перевіряє права (Admin, Editor), наявність сутностей та конфлікти часу.
    /// </summary>
    public async Task UpdateScheduleItemAsync(int currentUserId, int itemId, int groupId, int teacherId, int classroomId, int disciplineId, DayOfWeek dayOfWeek, int lessonNumber, string weekType)
    {
        await GetAndValidateUserAsync(currentUserId, new[] { "Admin", "Editor" }, "Тільки Адміністратори та Редактори могут оновлювати розклад.");

        var item = await _store.ScheduleItems.GetAsync(itemId);
        if (item == null)
        {
            throw new EntityNotFoundException("Елемент розкладу не знайдено.");
        }

        await ValidateEntityExistenceAsync(groupId, teacherId, classroomId, disciplineId);

        WeekType wType = ParseWeekType(weekType);

        await CheckConflictsAsync(itemId, groupId, teacherId, classroomId, dayOfWeek, lessonNumber, wType);

        item.GroupId = groupId;
        item.TeacherId = teacherId;
        item.ClassroomId = classroomId;
        item.DisciplineId = disciplineId;
        item.DayOfWeek = dayOfWeek;
        item.LessonNumber = lessonNumber;
        item.WeekType = wType;

        _store.ScheduleItems.Update(item);
        await _store.CommitAsync();
    }

    /// <summary>
    /// Видаляє пару з розкладу.
    /// </summary>
    public async Task DeleteScheduleItemAsync(int currentUserId, int itemId)
    {
        await GetAndValidateUserAsync(currentUserId, new[] { "Admin", "Editor" }, "Тільки Адміністратори та Редактори можуть видаляти розклад.");

        var item = await _store.ScheduleItems.GetAsync(itemId);
        if (item == null)
        {
            throw new EntityNotFoundException("Елемент розкладу не знайдено.");
        }

        await _store.ScheduleItems.DeleteAsync(itemId);
        await _store.CommitAsync();
    }
    #endregion

    #region Reference Lists
    /// <summary>
    /// Отримує список усіх зареєстрованих підрозділів.
    /// </summary>
    public async Task<IReadOnlyCollection<DepartmentInfo>> GetAllDepartmentsAsync(int currentUserId)
    {
        await GetAndValidateUserAsync(currentUserId);
        var deps = await _store.Departments.GetAllAsync();
        return _mapper.Map<IReadOnlyCollection<DepartmentInfo>>(deps);
    }

    /// <summary>
    /// Отримує список усіх академічних груп.
    /// </summary>
    public async Task<IReadOnlyCollection<GroupInfo>> GetAllGroupsAsync(int currentUserId)
    {
        await GetAndValidateUserAsync(currentUserId);
        var grps = await _store.Groups.GetAllAsync();
        return _mapper.Map<IReadOnlyCollection<GroupInfo>>(grps);
    }

    /// <summary>
    /// Отримує список усіх викладачів.
    /// </summary>
    public async Task<IReadOnlyCollection<TeacherInfo>> GetAllTeachersAsync(int currentUserId)
    {
        var user = await GetAndValidateUserAsync(currentUserId);
        var teachers = await _store.Teachers.GetAllAsync();
        if (user.Role == "Teacher")
        {
            teachers = teachers
                .Where(t => t.UserId == user.Id)
                .ToList();
        }

        return _mapper.Map<IReadOnlyCollection<TeacherInfo>>(teachers);
    }

    /// <summary>
    /// Отримує список усіх навчальних аудиторій.
    /// </summary>
    public async Task<IReadOnlyCollection<ClassroomInfo>> GetAllClassroomsAsync(int currentUserId)
    {
        await GetAndValidateUserAsync(currentUserId);
        var rooms = await _store.Classrooms.GetAllAsync();
        return _mapper.Map<IReadOnlyCollection<ClassroomInfo>>(rooms);
    }

    /// <summary>
    /// Отримує список усіх навчальних дисциплін.
    /// </summary>
    public async Task<IReadOnlyCollection<DisciplineInfo>> GetAllDisciplinesAsync(int currentUserId)
    {
        await GetAndValidateUserAsync(currentUserId);
        var disc = await _store.Disciplines.GetAllAsync();
        return _mapper.Map<IReadOnlyCollection<DisciplineInfo>>(disc);
    }
    #endregion

    #region Reference CRUD (Admin only)
    /// <summary>
    /// Створює новий підрозділ. Назва передається енамом для забезпечення уніфікації.
    /// </summary>
    public async Task CreateDepartmentAsync(int currentUserId, DepartmentName name)
    {
        await GetAndValidateUserAsync(currentUserId, new[] { "Admin" }, "Лише Адміністратор може створювати підрозділи.");

        var dep = new Department { Name = name.ToString() };
        await _store.Departments.CreateAsync(dep);
        await _store.CommitAsync();
    }

    /// <summary>
    /// Створює нову академічну групу. Назва строго валідується за допомогою Regex.
    /// </summary>
    public async Task CreateGroupAsync(int currentUserId, string name, int departmentId)
    {
        await GetAndValidateUserAsync(currentUserId, new[] { "Admin" }, "Лише Адміністратор може створювати групи.");
        
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Назва групи обов'язкова.");
        }

        // Перевірка формату назви групи за допомогою Regex (наприклад: ПІ-221)
        if (!GroupNameRegex.IsMatch(name))
        {
            throw new ArgumentException("Назва групи має відповідати формату 'XX-000' (наприклад, 'ПІ-221' чи 'КН-11').");
        }
        
        var dep = await _store.Departments.GetAsync(departmentId);
        if (dep == null)
        {
            throw new EntityNotFoundException("Вказаний підрозділ не знайдено.");
        }

        var grp = new Group { Name = name, DepartmentId = departmentId };
        await _store.Groups.CreateAsync(grp);
        await _store.CommitAsync();
    }

    /// <summary>
    /// Додає викладача в систему. Науковий ступінь обирається строго з енаму.
    /// </summary>
    public async Task CreateTeacherAsync(int currentUserId, string name, TeacherDegree degree, int departmentId, int? userId)
    {
        await GetAndValidateUserAsync(currentUserId, new[] { "Admin" }, "Лише Адміністратор може додавати викладачів.");
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("ПІБ викладача обов'язкове.");
        }

        var dep = await _store.Departments.GetAsync(departmentId);
        if (dep == null)
        {
            throw new EntityNotFoundException("Вказаний підрозділ не знайдено.");
        }

        if (userId.HasValue)
        {
            var u = await _store.Users.GetAsync(userId.Value);
            if (u == null)
            {
                throw new EntityNotFoundException("Вказаного користувача не знайдено.");
            }
        }

        var t = new Teacher 
        { 
            Name = name, 
            Degree = degree.ToString(), 
            DepartmentId = departmentId, 
            UserId = userId 
        };
        await _store.Teachers.CreateAsync(t);
        await _store.CommitAsync();
    }

    /// <summary>
    /// Додає аудиторію із суворою перевіркою належності готової назви аудиторії до вказаного корпусу.
    /// </summary>
    public async Task CreateClassroomAsync(int currentUserId, ClassroomName name, ClassroomBuilding building, int capacity)
    {
        await GetAndValidateUserAsync(currentUserId, new[] { "Admin" }, "Лише Адміністратор може створювати аудиторії.");
        
        // Сувора бізнес-валідація сумісності назви кімнати з корпусом
        if (!building.IsValidRoomForBuilding(name))
        {
            throw new ArgumentException($"Аудиторія '{name}' не належить навчальному корпусу '{building}'. Будь ласка, оберіть коректну пару корпус/кімната.");
        }

        if (capacity <= 0)
        {
            throw new ArgumentException("Місткість повинна бути більше 0.");
        }

        var c = new Classroom 
        { 
            Name = name.ToString(), 
            Building = building.ToString(), 
            Capacity = capacity 
        };
        await _store.Classrooms.CreateAsync(c);
        await _store.CommitAsync();
    }

    /// <summary>
    /// Додає нову навчальну дисципліну в довідник.
    /// </summary>
    public async Task CreateDisciplineAsync(int currentUserId, string name)
    {
        await GetAndValidateUserAsync(currentUserId, new[] { "Admin" }, "Лише Адміністратор може додавати дисципліни.");
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Назва дисципліни обов'язкова.");
        }

        var d = new Discipline { Name = name };
        await _store.Disciplines.CreateAsync(d);
        await _store.CommitAsync();
    }
    #endregion

    #region Helper validation methods
    /// <summary>
    /// Допоміжний метод валідації існування пов'язаних сутностей для пари розкладу.
    /// </summary>
    private async Task ValidateEntityExistenceAsync(int groupId, int teacherId, int classroomId, int disciplineId)
    {
        var grp = await _store.Groups.GetAsync(groupId);
        if (grp == null)
        {
            throw new EntityNotFoundException("Групу не знайдено.");
        }

        var t = await _store.Teachers.GetAsync(teacherId);
        if (t == null)
        {
            throw new EntityNotFoundException("Викладача не знайдено.");
        }

        var c = await _store.Classrooms.GetAsync(classroomId);
        if (c == null)
        {
            throw new EntityNotFoundException("Аудиторію не знайдено.");
        }

        var d = await _store.Disciplines.GetAsync(disciplineId);
        if (d == null)
        {
            throw new EntityNotFoundException("Дисципліну не знайдено.");
        }
    }

    /// <summary>
    /// Допоміжний метод парсингу типу тижня (odd, even, both).
    /// </summary>
    private WeekType ParseWeekType(string weekType)
    {
        if (string.IsNullOrWhiteSpace(weekType))
        {
            return WeekType.Both;
        }
        
        string wt = weekType.Trim().ToLower();
        if (wt == "odd")
        {
            return WeekType.Odd;
        }
        if (wt == "even")
        {
            return WeekType.Even;
        }
        return WeekType.Both;
    }

    /// <summary>
    /// Допоміжний метод перевірки перетину типів тижнів.
    /// </summary>
    private bool WeekTypesConflict(WeekType w1, WeekType w2)
    {
        if (w1 == WeekType.Both || w2 == WeekType.Both)
        {
            return true;
        }
        return w1 == w2;
    }

    /// <summary>
    /// Головний алгоритм виявлення накладок розкладу (накладка часу для групи, викладача чи аудиторії).
    /// </summary>
    private async Task CheckConflictsAsync(int? currentItemId, int groupId, int teacherId, int classroomId, DayOfWeek dayOfWeek, int lessonNumber, WeekType newWeekType)
    {
        var allItems = await _store.ScheduleItems.GetAllAsync();

        foreach (var item in allItems)
        {
            if (currentItemId.HasValue && item.Id == currentItemId.Value)
            {
                continue;
            }

            if (item.DayOfWeek == dayOfWeek && item.LessonNumber == lessonNumber)
            {
                if (WeekTypesConflict(item.WeekType, newWeekType))
                {
                    if (item.GroupId == groupId)
                    {
                        throw new ScheduleConflictException($"Група вже має інше заняття ({item.Discipline?.Name ?? "пара"}) на {lessonNumber}-й парі в цей день.");
                    }

                    if (item.TeacherId == teacherId)
                    {
                        throw new ScheduleConflictException($"Викладач {item.Teacher?.Name ?? "викладач"} уже проводить інше заняття в цей час.");
                    }

                    if (item.ClassroomId == classroomId)
                    {
                        throw new ScheduleConflictException($"Аудиторія {item.Classroom?.Name ?? "кімната"} вже зайнята іншою парою в цей час.");
                    }
                }
            }
        }
    }
    #endregion
}
