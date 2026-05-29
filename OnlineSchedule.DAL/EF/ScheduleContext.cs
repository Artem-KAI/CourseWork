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

        // Seed Users (SHA256 of "password123" is: 5e8837cd00ece283717503292153a338025a16f81585c329043f1144836b0b4c)
        string pwdHash = "5e8837cd00ece283717503292153a338025a16f81585c329043f1144836b0b4c";
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "admin", Email = "admin@schedule.com", PasswordHash = pwdHash, Role = "Admin" },
            new User { Id = 2, Username = "editor", Email = "editor@schedule.com", PasswordHash = pwdHash, Role = "Editor" },
            new User { Id = 3, Username = "director", Email = "director@schedule.com", PasswordHash = pwdHash, Role = "Management" },
            new User { Id = 4, Username = "dyshleva", Email = "teacher@schedule.com", PasswordHash = pwdHash, Role = "Teacher" },
            new User { Id = 5, Username = "ivanenko", Email = "student@schedule.com", PasswordHash = pwdHash, Role = "Student" }
        );

        // Seed Departments
        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "Кафедра ІПЗ" },
            new Department { Id = 2, Name = "Кафедра САПР" }
        );

        // Seed Teachers
        modelBuilder.Entity<Teacher>().HasData(
            new Teacher { Id = 1, Name = "Дишлевий О.П.", Degree = "доцент", DepartmentId = 1, UserId = 4 },
            new Teacher { Id = 2, Name = "Коваленко І.І.", Degree = "професор", DepartmentId = 2, UserId = null }
        );

        // Seed Groups
        modelBuilder.Entity<Group>().HasData(
            new Group { Id = 1, Name = "ПІ-221", DepartmentId = 1 },
            new Group { Id = 2, Name = "ПІ-222", DepartmentId = 1 },
            new Group { Id = 3, Name = "САПР-221", DepartmentId = 2 }
        );

        // Seed Classrooms
        modelBuilder.Entity<Classroom>().HasData(
            new Classroom { Id = 1, Name = "1-305", Building = "Корпус 1", Capacity = 30 },
            new Classroom { Id = 2, Name = "1-402", Building = "Корпус 1", Capacity = 50 },
            new Classroom { Id = 3, Name = "2-108", Building = "Корпус 2", Capacity = 100 }
        );

        // Seed Disciplines
        modelBuilder.Entity<Discipline>().HasData(
            new Discipline { Id = 1, Name = "Архітектура та проектування ПЗ" },
            new Discipline { Id = 2, Name = "Бази даних" },
            new Discipline { Id = 3, Name = "Алгоритми та структури даних" }
        );

        // Seed ScheduleItems
        modelBuilder.Entity<ScheduleItem>().HasData(
            new ScheduleItem 
            { 
                Id = 1, 
                GroupId = 1, 
                TeacherId = 1, 
                ClassroomId = 1, 
                DisciplineId = 1, 
                DayOfWeek = DayOfWeek.Monday, 
                LessonNumber = 1, 
                WeekType = WeekType.Both 
            },
            new ScheduleItem 
            { 
                Id = 2, 
                GroupId = 1, 
                TeacherId = 2, 
                ClassroomId = 2, 
                DisciplineId = 2, 
                DayOfWeek = DayOfWeek.Tuesday, 
                LessonNumber = 2, 
                WeekType = WeekType.Odd 
            },
            new ScheduleItem 
            { 
                Id = 3, 
                GroupId = 2, 
                TeacherId = 1, 
                ClassroomId = 1, 
                DisciplineId = 1, 
                DayOfWeek = DayOfWeek.Wednesday, 
                LessonNumber = 3, 
                WeekType = WeekType.Even 
            }
        );
    }
}
