using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL;
using BLL.Exceptions;
using BLL.Services;
using CONTRACT.DTO;
using DAL.Entities;
using DAL.Interfaces;
using Moq;
using NUnit.Framework;

namespace TESTS;

[TestFixture]
public class ScheduleManagerTests
{
    private Mock<IDataStore> _storeMock;
    private IMapper _mapper;
    private ScheduleManager _scheduleManager;

    private List<User> _users;
    private List<Department> _departments;
    private List<Group> _groups;
    private List<Teacher> _teachers;
    private List<Classroom> _classrooms;
    private List<Discipline> _disciplines;
    private List<ScheduleItem> _scheduleItems;

    [SetUp]
    public void SetUp()
    {
        // 1. Configure AutoMapper
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BLLMappingProfile>());
        _mapper = config.CreateMapper();

        // 2. Initialize mock datasets
        _users = new List<User>
        {
            new User { Id = 1, Username = "admin", Email = "admin@test.com", Role = "Admin" },
            new User { Id = 2, Username = "editor", Email = "editor@test.com", Role = "Editor" },
            new User { Id = 3, Username = "director", Email = "director@test.com", Role = "Management" },
            new User { Id = 4, Username = "teacher_smith", Email = "smith@test.com", Role = "Teacher" },
            new User { Id = 5, Username = "student_john", Email = "john@test.com", Role = "Student" }
        };

        _groups = new List<Group>
        {
            new Group { Id = 101, Name = "ПІ-221", DepartmentId = 1 },
            new Group { Id = 102, Name = "ПІ-222", DepartmentId = 1 }
        };

        _teachers = new List<Teacher>
        {
            new Teacher { Id = 201, Name = "Дишлевий О.П.", DepartmentId = 1, UserId = 4 },
            new Teacher { Id = 202, Name = "Коваленко І.І.", DepartmentId = 2, UserId = null }
        };

        _classrooms = new List<Classroom>
        {
            new Classroom { Id = 301, Name = "1-305", Capacity = 30 },
            new Classroom { Id = 302, Name = "1-402", Capacity = 50 }
        };

        _departments = new List<Department>
        {
            new Department { Id = 1, Name = "FICT" },
            new Department { Id = 2, Name = "IPSA" }
        };

        _disciplines = new List<Discipline>
        {
            new Discipline { Id = 401, Name = "Архітектура ПЗ" },
            new Discipline { Id = 402, Name = "Бази даних" }
        };

        var mockGroup = _groups.First(g => g.Id == 101);
        var mockTeacher = _teachers.First(t => t.Id == 201);
        var mockClassroom = _classrooms.First(c => c.Id == 301);
        var mockDiscipline = _disciplines.First(d => d.Id == 401);

        _scheduleItems = new List<ScheduleItem>
        {
            new ScheduleItem 
            { 
                Id = 1, 
                GroupId = 101, 
                Group = mockGroup,
                TeacherId = 201, 
                Teacher = mockTeacher,
                ClassroomId = 301, 
                Classroom = mockClassroom,
                DisciplineId = 401, 
                Discipline = mockDiscipline,
                DayOfWeek = DayOfWeek.Monday, 
                LessonNumber = 1, 
                WeekType = WeekType.Both 
            }
        };

        // 3. Configure Mock repositories using ReturnsAsync
        var usersRepoMock = new Mock<IRepository<User>>();
        usersRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(_users);
        usersRepoMock.Setup(r => r.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) => _users.FirstOrDefault(u => u.Id == id));

        var groupsRepoMock = new Mock<IRepository<Group>>();
        groupsRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(_groups);
        groupsRepoMock.Setup(r => r.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) => _groups.FirstOrDefault(g => g.Id == id));

        var teachersRepoMock = new Mock<IRepository<Teacher>>();
        teachersRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(_teachers);
        teachersRepoMock.Setup(r => r.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) => _teachers.FirstOrDefault(t => t.Id == id));

        var classroomsRepoMock = new Mock<IRepository<Classroom>>();
        classroomsRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(_classrooms);
        classroomsRepoMock.Setup(r => r.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) => _classrooms.FirstOrDefault(c => c.Id == id));

        var disciplinesRepoMock = new Mock<IRepository<Discipline>>();
        disciplinesRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(_disciplines);
        disciplinesRepoMock.Setup(r => r.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) => _disciplines.FirstOrDefault(d => d.Id == id));

        var departmentsRepoMock = new Mock<IRepository<Department>>();
        departmentsRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(_departments);
        departmentsRepoMock.Setup(r => r.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) => _departments.FirstOrDefault(d => d.Id == id));
        departmentsRepoMock.Setup(r => r.CreateAsync(It.IsAny<Department>())).Callback<Department>(d => _departments.Add(d)).Returns(Task.CompletedTask);

        var scheduleItemsRepoMock = new Mock<IRepository<ScheduleItem>>();
        scheduleItemsRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(_scheduleItems);
        scheduleItemsRepoMock.Setup(r => r.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) => _scheduleItems.FirstOrDefault(s => s.Id == id));
        scheduleItemsRepoMock.Setup(r => r.CreateAsync(It.IsAny<ScheduleItem>())).Callback<ScheduleItem>(item => _scheduleItems.Add(item)).Returns(Task.CompletedTask);

        // 4. Configure Mock IDataStore
        _storeMock = new Mock<IDataStore>();
        _storeMock.Setup(s => s.Users).Returns(usersRepoMock.Object);
        _storeMock.Setup(s => s.Departments).Returns(departmentsRepoMock.Object);
        _storeMock.Setup(s => s.Groups).Returns(groupsRepoMock.Object);
        _storeMock.Setup(s => s.Teachers).Returns(teachersRepoMock.Object);
        _storeMock.Setup(s => s.Classrooms).Returns(classroomsRepoMock.Object);
        _storeMock.Setup(s => s.Disciplines).Returns(disciplinesRepoMock.Object);
        _storeMock.Setup(s => s.ScheduleItems).Returns(scheduleItemsRepoMock.Object);
        _storeMock.Setup(s => s.CommitAsync()).Returns(Task.CompletedTask);

        // 5. Initialize target system
        _scheduleManager = new ScheduleManager(_storeMock.Object, _mapper);
    }

    #region Security / Role tests
    [Test]
    public async Task GetScheduleForGroup_AsStudent_Succeeds()
    {
        // Arrange
        int studentId = 5;
        int targetGroupId = 101;

        // Act
        var result = await _scheduleManager.GetScheduleForGroupAsync(studentId, targetGroupId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result.First().GroupName, Is.EqualTo("ПІ-221"));
    }

    [Test]
    public void GetScheduleForTeacher_AsStudent_ThrowsAccessDeniedException()
    {
        // Arrange
        int studentId = 5;
        int targetTeacherId = 201;

        // Act & Assert
        Assert.ThrowsAsync<AccessDeniedException>(async () =>
            await _scheduleManager.GetScheduleForTeacherAsync(studentId, targetTeacherId)
        );
    }

    [Test]
    public void GetScheduleForTeacher_AsTeacherViewingAnotherTeacher_ThrowsAccessDeniedException()
    {
        // Arrange
        int teacherUserId = 4;
        int targetTeacherId = 202;

        // Act & Assert
        Assert.ThrowsAsync<AccessDeniedException>(async () =>
            await _scheduleManager.GetScheduleForTeacherAsync(teacherUserId, targetTeacherId)
        );
    }

    [Test]
    public async Task GetScheduleForTeacher_AsTeacherViewingOwnSchedule_Succeeds()
    {
        // Arrange
        int teacherUserId = 4;
        int targetTeacherId = 201;

        // Act
        var result = await _scheduleManager.GetScheduleForTeacherAsync(teacherUserId, targetTeacherId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(1));
    }

    [Test]
    public void GetScheduleForClassroom_AsStudent_ThrowsAccessDeniedException()
    {
        // Arrange
        int studentUserId = 5;
        int targetClassroomId = 301;

        // Act & Assert
        Assert.ThrowsAsync<AccessDeniedException>(async () =>
            await _scheduleManager.GetScheduleForClassroomAsync(studentUserId, targetClassroomId)
        );
    }

    [Test]
    public async Task GetScheduleForClassroom_AsAdmin_Succeeds()
    {
        // Arrange
        int adminUserId = 1;
        int targetClassroomId = 301;

        // Act
        var result = await _scheduleManager.GetScheduleForClassroomAsync(adminUserId, targetClassroomId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(1));
    }
    #endregion

    #region Conflict checking tests
    [Test]
    public void CreateScheduleItem_WithTeacherConflict_ThrowsScheduleConflictException()
    {
        // Arrange
        int editorUserId = 2;
        int newGroupId = 102;
        int teacherId = 201;
        int classroomId = 302;
        int disciplineId = 402;

        // Act & Assert
        var ex = Assert.ThrowsAsync<ScheduleConflictException>(async () =>
            await _scheduleManager.CreateScheduleItemAsync(editorUserId, newGroupId, teacherId, classroomId, disciplineId, DayOfWeek.Monday, 1, "odd")
        );
        Assert.That(ex.Message, Does.Contain("Викладач"));
    }

    [Test]
    public void CreateScheduleItem_WithClassroomConflict_ThrowsScheduleConflictException()
    {
        // Arrange
        int editorUserId = 2;
        int newGroupId = 102;
        int teacherId = 202;
        int classroomId = 301;
        int disciplineId = 402;

        // Act & Assert
        var ex = Assert.ThrowsAsync<ScheduleConflictException>(async () =>
            await _scheduleManager.CreateScheduleItemAsync(editorUserId, newGroupId, teacherId, classroomId, disciplineId, DayOfWeek.Monday, 1, "both")
        );
        Assert.That(ex.Message, Does.Contain("Аудиторія"));
    }

    [Test]
    public void CreateScheduleItem_WithGroupConflict_ThrowsScheduleConflictException()
    {
        // Arrange
        int editorUserId = 2;
        int newGroupId = 101;
        int teacherId = 202;
        int classroomId = 302;
        int disciplineId = 402;

        // Act & Assert
        var ex = Assert.ThrowsAsync<ScheduleConflictException>(async () =>
            await _scheduleManager.CreateScheduleItemAsync(editorUserId, newGroupId, teacherId, classroomId, disciplineId, DayOfWeek.Monday, 1, "even")
        );
        Assert.That(ex.Message, Does.Contain("Група"));
    }

    [Test]
    public async Task CreateScheduleItem_NoConflictDifferentTime_Succeeds()
    {
        // Arrange
        int editorUserId = 2;
        int newGroupId = 101;
        int teacherId = 201;
        int classroomId = 301;
        int disciplineId = 401;

        // Act
        await _scheduleManager.CreateScheduleItemAsync(editorUserId, newGroupId, teacherId, classroomId, disciplineId, DayOfWeek.Monday, 2, "both");

        // Assert
        Assert.That(_scheduleItems.Count, Is.EqualTo(2));
        _storeMock.Verify(s => s.CommitAsync(), Times.Once);
    }

    [Test]
    public void CreateGroup_WithInvalidNameRegex_ThrowsArgumentException()
    {
        // Arrange
        int adminUserId = 1;
        string invalidGroupName = "InvalidGroup";

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            await _scheduleManager.CreateGroupAsync(adminUserId, invalidGroupName, 1)
        );
        Assert.That(ex.Message, Does.Contain("Назва групи має відповідати формату"));
    }

    [Test]
    public async Task CreateGroup_WithValidNameRegex_Succeeds()
    {
        // Arrange
        int adminUserId = 1;
        string validGroupName = "ПІ-223";

        // Act
        await _scheduleManager.CreateGroupAsync(adminUserId, validGroupName, 1);

        // Assert
        _storeMock.Verify(s => s.Groups.CreateAsync(It.Is<Group>(g => g.Name == validGroupName)), Times.Once);
    }

    [Test]
    public void CreateClassroom_WithInvalidBuildingRoomMismatch_ThrowsArgumentException()
    {
        // Arrange
        int adminUserId = 1;
        var room = BLL.Enums.ClassroomName.Room_201; // Належить до ScienceBuilding
        var building = BLL.Enums.ClassroomBuilding.MainBuilding;

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            await _scheduleManager.CreateClassroomAsync(adminUserId, room, building, 30)
        );
        Assert.That(ex.Message, Does.Contain("не належить навчальному корпусу"));
    }

    [Test]
    public async Task CreateClassroom_WithValidBuildingRoomCompliance_Succeeds()
    {
        // Arrange
        int adminUserId = 1;
        var room = BLL.Enums.ClassroomName.Room_101; // Належить до MainBuilding
        var building = BLL.Enums.ClassroomBuilding.MainBuilding;

        // Act
        await _scheduleManager.CreateClassroomAsync(adminUserId, room, building, 30);

        // Assert
        _storeMock.Verify(s => s.Classrooms.CreateAsync(It.Is<Classroom>(c => c.Name == "Room_101" && c.Building == "MainBuilding")), Times.Once);
    }
    #endregion
}
