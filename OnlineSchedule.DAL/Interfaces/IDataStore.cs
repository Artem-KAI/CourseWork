using System;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Interfaces;

public interface IDataStore : IDisposable
{
    IRepository<User> Users { get; }
    IRepository<Department> Departments { get; }
    IRepository<Teacher> Teachers { get; }
    IRepository<Group> Groups { get; }
    IRepository<Classroom> Classrooms { get; }
    IRepository<Discipline> Disciplines { get; }
    IRepository<ScheduleItem> ScheduleItems { get; }

    Task CommitAsync();
}
