using System;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF;

public class ScheduleContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<Classroom> Classrooms => Set<Classroom>();
    public DbSet<Discipline> Disciplines => Set<Discipline>();
    public DbSet<ScheduleItem> ScheduleItems => Set<ScheduleItem>();

    public ScheduleContext(DbContextOptions<ScheduleContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure ScheduleItem relationships
        modelBuilder.Entity<ScheduleItem>()
            .HasOne(s => s.Group)
            .WithMany(g => g.ScheduleItems)
            .HasForeignKey(s => s.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ScheduleItem>()
            .HasOne(s => s.Teacher)
            .WithMany(t => t.ScheduleItems)
            .HasForeignKey(s => s.TeacherId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ScheduleItem>()
            .HasOne(s => s.Classroom)
            .WithMany(c => c.ScheduleItems)
            .HasForeignKey(s => s.ClassroomId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ScheduleItem>()
            .HasOne(s => s.Discipline)
            .WithMany(d => d.ScheduleItems)
            .HasForeignKey(s => s.DisciplineId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure Teacher-Department
        modelBuilder.Entity<Teacher>()
            .HasOne(t => t.Department)
            .WithMany(d => d.Teachers)
            .HasForeignKey(t => t.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Group-Department
        modelBuilder.Entity<Group>()
            .HasOne(g => g.Department)
            .WithMany(d => d.Groups)
            .HasForeignKey(g => g.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Teacher-User
        modelBuilder.Entity<Teacher>()
            .HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        // Configure Discipline-Department
        modelBuilder.Entity<Discipline>()
            .HasOne(d => d.Department)
            .WithMany()
            .HasForeignKey(d => d.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed Users (SHA256 of "password123" is: ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f)
        string pwdHash = "ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f";
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "Admin", Email = "admin@schedule.com", PasswordHash = pwdHash, Role = "Admin" },
            new User { Id = 2, Username = "Editor", Email = "editor@schedule.com", PasswordHash = pwdHash, Role = "Editor" },
            new User { Id = 3, Username = "Director", Email = "director@schedule.com", PasswordHash = pwdHash, Role = "Management" },
            new User { Id = 4, Username = "Дишлевий О.П.", Email = "teacher1@schedule.com", PasswordHash = pwdHash, Role = "Teacher" },
            new User { Id = 5, Username = "Іваненко", Email = "student@schedule.com", PasswordHash = pwdHash, Role = "Student" }
        );

        // Seed Departments
        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "Кафедра ІПЗ" },
            new Department { Id = 2, Name = "Кафедра САПР" },
            new Department { Id = 3, Name = "Кафедра Комп'ютерних наук" }
        );

        // Seed Teachers
        modelBuilder.Entity<Teacher>().HasData(
            new Teacher { Id = 1, Name = "Дишлевий О.П.", Degree = "доцент", DepartmentId = 1, UserId = 4 },
            new Teacher { Id = 2, Name = "Коваленко І.І.", Degree = "професор", DepartmentId = 2, UserId = null },
            new Teacher { Id = 3, Name = "Шевченко А.М.", Degree = "ст. викладач", DepartmentId = 1, UserId = null },
            new Teacher { Id = 4, Name = "Петренко В.О.", Degree = "доцент", DepartmentId = 3, UserId = null },
            new Teacher { Id = 5, Name = "Сидоренко О.В.", Degree = "професор", DepartmentId = 1, UserId = null },
            new Teacher { Id = 6, Name = "Мельник Ю.М.", Degree = "доцент", DepartmentId = 2, UserId = null },
            new Teacher { Id = 7, Name = "Лисенко К.А.", Degree = "асистент", DepartmentId = 3, UserId = null }
        );

        // Seed Groups
        modelBuilder.Entity<Group>().HasData(
            new Group { Id = 1, Name = "ПІ-221", DepartmentId = 1 },
            new Group { Id = 2, Name = "ПІ-222", DepartmentId = 1 },
            new Group { Id = 3, Name = "САПР-221", DepartmentId = 2 },
            new Group { Id = 4, Name = "САПР-222", DepartmentId = 2 },
            new Group { Id = 5, Name = "КН-221", DepartmentId = 3 },
            new Group { Id = 6, Name = "КН-222", DepartmentId = 3 }
        );

        // Seed Classrooms
        modelBuilder.Entity<Classroom>().HasData(
            new Classroom { Id = 1, Name = "1-305", Building = "Корпус 1", Capacity = 30 },
            new Classroom { Id = 2, Name = "1-402", Building = "Корпус 1", Capacity = 50 },
            new Classroom { Id = 3, Name = "2-108", Building = "Корпус 2", Capacity = 100 },
            new Classroom { Id = 4, Name = "2-205", Building = "Корпус 2", Capacity = 40 },
            new Classroom { Id = 5, Name = "3-12", Building = "Корпус 3", Capacity = 60 },
            new Classroom { Id = 6, Name = "3-45", Building = "Корпус 3", Capacity = 25 }
        );

        // Seed Disciplines
        modelBuilder.Entity<Discipline>().HasData(
            new Discipline { Id = 1, Name = "Архітектура та проектування ПЗ", DepartmentId = 1 },
            new Discipline { Id = 2, Name = "Бази даних", DepartmentId = 1 },
            new Discipline { Id = 3, Name = "Алгоритми та структури даних", DepartmentId = 2 },
            new Discipline { Id = 4, Name = "Об'єктно-орієнтоване програмування", DepartmentId = 1 },
            new Discipline { Id = 5, Name = "Комп'ютерні мережі", DepartmentId = 3 },
            new Discipline { Id = 6, Name = "Операційні системи", DepartmentId = 3 },
            new Discipline { Id = 7, Name = "Штучний інтелект", DepartmentId = 2 },
            new Discipline { Id = 8, Name = "Веб-технології", DepartmentId = 1 }
        );

        // Seed ScheduleItems
        modelBuilder.Entity<ScheduleItem>().HasData(
            new ScheduleItem { Id = 1, GroupId = 1, TeacherId = 1, ClassroomId = 1, DisciplineId = 1, DayOfWeek = DayOfWeek.Monday, LessonNumber = 1, WeekType = WeekType.Both },
            new ScheduleItem { Id = 2, GroupId = 1, TeacherId = 2, ClassroomId = 2, DisciplineId = 2, DayOfWeek = DayOfWeek.Tuesday, LessonNumber = 2, WeekType = WeekType.Odd },
            new ScheduleItem { Id = 3, GroupId = 2, TeacherId = 1, ClassroomId = 1, DisciplineId = 1, DayOfWeek = DayOfWeek.Wednesday, LessonNumber = 3, WeekType = WeekType.Even },
            new ScheduleItem { Id = 4, GroupId = 3, TeacherId = 3, ClassroomId = 3, DisciplineId = 3, DayOfWeek = DayOfWeek.Monday, LessonNumber = 2, WeekType = WeekType.Both },
            new ScheduleItem { Id = 5, GroupId = 4, TeacherId = 4, ClassroomId = 4, DisciplineId = 5, DayOfWeek = DayOfWeek.Thursday, LessonNumber = 1, WeekType = WeekType.Odd },
            new ScheduleItem { Id = 6, GroupId = 5, TeacherId = 5, ClassroomId = 5, DisciplineId = 6, DayOfWeek = DayOfWeek.Friday, LessonNumber = 4, WeekType = WeekType.Both },
            new ScheduleItem { Id = 7, GroupId = 6, TeacherId = 6, ClassroomId = 6, DisciplineId = 7, DayOfWeek = DayOfWeek.Wednesday, LessonNumber = 2, WeekType = WeekType.Even },
            new ScheduleItem { Id = 8, GroupId = 1, TeacherId = 3, ClassroomId = 1, DisciplineId = 8, DayOfWeek = DayOfWeek.Monday, LessonNumber = 3, WeekType = WeekType.Even },
            new ScheduleItem { Id = 9, GroupId = 2, TeacherId = 5, ClassroomId = 2, DisciplineId = 4, DayOfWeek = DayOfWeek.Tuesday, LessonNumber = 1, WeekType = WeekType.Both },
            new ScheduleItem { Id = 10, GroupId = 3, TeacherId = 2, ClassroomId = 3, DisciplineId = 3, DayOfWeek = DayOfWeek.Wednesday, LessonNumber = 4, WeekType = WeekType.Odd },
            new ScheduleItem { Id = 11, GroupId = 4, TeacherId = 6, ClassroomId = 4, DisciplineId = 7, DayOfWeek = DayOfWeek.Thursday, LessonNumber = 3, WeekType = WeekType.Both },
            new ScheduleItem { Id = 12, GroupId = 5, TeacherId = 7, ClassroomId = 5, DisciplineId = 5, DayOfWeek = DayOfWeek.Friday, LessonNumber = 2, WeekType = WeekType.Odd },
            new ScheduleItem { Id = 13, GroupId = 6, TeacherId = 4, ClassroomId = 6, DisciplineId = 6, DayOfWeek = DayOfWeek.Monday, LessonNumber = 4, WeekType = WeekType.Both },
            new ScheduleItem { Id = 14, GroupId = 1, TeacherId = 1, ClassroomId = 3, DisciplineId = 1, DayOfWeek = DayOfWeek.Wednesday, LessonNumber = 1, WeekType = WeekType.Odd },
            new ScheduleItem { Id = 15, GroupId = 2, TeacherId = 3, ClassroomId = 2, DisciplineId = 8, DayOfWeek = DayOfWeek.Thursday, LessonNumber = 2, WeekType = WeekType.Both },
            new ScheduleItem { Id = 16, GroupId = 3, TeacherId = 6, ClassroomId = 4, DisciplineId = 7, DayOfWeek = DayOfWeek.Friday, LessonNumber = 1, WeekType = WeekType.Even },
            new ScheduleItem { Id = 17, GroupId = 5, TeacherId = 7, ClassroomId = 5, DisciplineId = 6, DayOfWeek = DayOfWeek.Tuesday, LessonNumber = 3, WeekType = WeekType.Both },
            new ScheduleItem { Id = 18, GroupId = 1, TeacherId = 5, ClassroomId = 1, DisciplineId = 4, DayOfWeek = DayOfWeek.Thursday, LessonNumber = 4, WeekType = WeekType.Even },
            new ScheduleItem { Id = 19, GroupId = 2, TeacherId = 2, ClassroomId = 3, DisciplineId = 2, DayOfWeek = DayOfWeek.Monday, LessonNumber = 2, WeekType = WeekType.Odd },
            new ScheduleItem { Id = 20, GroupId = 4, TeacherId = 1, ClassroomId = 1, DisciplineId = 1, DayOfWeek = DayOfWeek.Tuesday, LessonNumber = 4, WeekType = WeekType.Both },
            new ScheduleItem { Id = 21, GroupId = 3, TeacherId = 3, ClassroomId = 2, DisciplineId = 8, DayOfWeek = DayOfWeek.Wednesday, LessonNumber = 2, WeekType = WeekType.Both },
            new ScheduleItem { Id = 22, GroupId = 6, TeacherId = 5, ClassroomId = 5, DisciplineId = 4, DayOfWeek = DayOfWeek.Friday, LessonNumber = 3, WeekType = WeekType.Odd },
            new ScheduleItem { Id = 23, GroupId = 1, TeacherId = 6, ClassroomId = 3, DisciplineId = 7, DayOfWeek = DayOfWeek.Saturday, LessonNumber = 1, WeekType = WeekType.Both },
            new ScheduleItem { Id = 24, GroupId = 2, TeacherId = 4, ClassroomId = 4, DisciplineId = 5, DayOfWeek = DayOfWeek.Saturday, LessonNumber = 2, WeekType = WeekType.Odd },
            new ScheduleItem { Id = 25, GroupId = 5, TeacherId = 1, ClassroomId = 1, DisciplineId = 1, DayOfWeek = DayOfWeek.Monday, LessonNumber = 5, WeekType = WeekType.Both },
            new ScheduleItem { Id = 26, GroupId = 3, TeacherId = 7, ClassroomId = 6, DisciplineId = 6, DayOfWeek = DayOfWeek.Tuesday, LessonNumber = 5, WeekType = WeekType.Even },
            new ScheduleItem { Id = 27, GroupId = 4, TeacherId = 2, ClassroomId = 2, DisciplineId = 3, DayOfWeek = DayOfWeek.Thursday, LessonNumber = 5, WeekType = WeekType.Odd },
            new ScheduleItem { Id = 28, GroupId = 1, TeacherId = 4, ClassroomId = 3, DisciplineId = 5, DayOfWeek = DayOfWeek.Friday, LessonNumber = 5, WeekType = WeekType.Both }
        );
    }
}
