using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL;
using BLL.Services;
using BLL.Exceptions;
using CONTRACT.DTO;
using DAL.Entities;
using DAL.Interfaces;
using Moq;
using NUnit.Framework;

namespace TESTS;

[TestFixture]
public class RoleValidationTests
{
    private Mock<IDataStore> _storeMock;
    private IMapper _mapper;
    private CredentialManager _credentialManager;
    private UserManager _userManager;
    private List<User> _users;

    [SetUp]
    public void SetUp()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BLLMappingProfile>());
        _mapper = config.CreateMapper();

        _users = new List<User>
        {
            new User { Id = 1, Username = "admin", Email = "admin@test.com", Role = "Admin", PasswordHash = "hashed" }
        };

        var usersRepoMock = new Mock<IRepository<User>>();
        usersRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(_users);
        usersRepoMock.Setup(r => r.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) => _users.FirstOrDefault(u => u.Id == id));
        usersRepoMock.Setup(r => r.CreateAsync(It.IsAny<User>())).Callback<User>(u => _users.Add(u)).Returns(Task.CompletedTask);

        _storeMock = new Mock<IDataStore>();
        _storeMock.Setup(s => s.Users).Returns(usersRepoMock.Object);
        _storeMock.Setup(s => s.CommitAsync()).Returns(Task.CompletedTask);

        _credentialManager = new CredentialManager(_storeMock.Object, _mapper);
        _userManager = new UserManager(_storeMock.Object, _mapper);
    }

    [Test]
    public async Task RegisterAccount_WithPermittedRoles_Succeeds(
        [Values("Student", "Teacher", "Admin", "Editor", "Management", "студент", "вчитель", "адміністратор", "редактор", "управлінський склад")] string role)
    {
        // Arrange
        string username = "testuser_" + Guid.NewGuid().ToString().Substring(0, 8);
        string email = username + "@test.com";

        // Act
        var result = await _credentialManager.RegisterAccountAsync(username, email, "password123", role);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Username, Is.EqualTo(username));
        // Verify mapped role matches the normalized English equivalent
        string expectedNormalizedRole = role.Trim().ToLower() switch
        {
            "admin" or "administrator" or "адміністратор" => "Admin",
            "editor" or "редактор" => "Editor",
            "management" or "управлінський склад" or "управлінський" => "Management",
            "teacher" or "вчитель" => "Teacher",
            _ => "Student"
        };
        Assert.That(result.Role, Is.EqualTo(expectedNormalizedRole));
    }

    [Test]
    public void RegisterAccount_WithForbiddenRole_ThrowsArgumentException()
    {
        // Arrange
        string forbiddenRole = "SuperUser";

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            await _credentialManager.RegisterAccountAsync("newuser", "new@test.com", "password", forbiddenRole)
        );
        Assert.That(ex.Message, Does.Contain("Неприпустима роль користувача"));
    }

    [Test]
    public void UpdateUser_WithForbiddenRole_ThrowsArgumentException()
    {
        // Arrange
        int userIdToUpdate = 1;
        string forbiddenRole = "Hacker";

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            await _userManager.UpdateUserAsync(userIdToUpdate, "updatedAdmin", "admin@test.com", forbiddenRole)
        );
        Assert.That(ex.Message, Does.Contain("Неприпустима роль користувача"));
    }
}
