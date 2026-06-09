using System.Threading.Tasks;
using DAL.Repositories;
using DAL.Entities;
using DAL.Interfaces;

namespace DAL.EF;

public class DataStore : IDataStore
{
    private readonly ScheduleContext _ctx;
   
    private GenericRepo<User>? _userRepo;
    private GenericRepo<Department>? _departmentRepo;
    private GenericRepo<Teacher>? _teacherRepo;
    private GenericRepo<Group>? _groupRepo;
    private GenericRepo<Classroom>? _classroomRepo;
    private GenericRepo<Discipline>? _disciplineRepo;
    private ScheduleItemRepository? _scheduleItemRepo;

    public DataStore(ScheduleContext context)
    {
        _ctx = context;
    }

    public async Task CommitAsync()
    {
        await _ctx.SaveChangesAsync();
    }

    public void Dispose()
    {
        _ctx.Dispose();
    }

    public IRepository<User> Users
    {
        get
        {
            if (_userRepo == null)
            {
                _userRepo = new GenericRepo<User>(_ctx);
            }
            return _userRepo;
        }
    }

    public IRepository<Department> Departments
    {
        get
        {
            if (_departmentRepo == null)
            {
                _departmentRepo = new GenericRepo<Department>(_ctx);
            }
            return _departmentRepo;
        }
    }

    public IRepository<Teacher> Teachers
    {
        get
        {
            if (_teacherRepo == null)
            {
                _teacherRepo = new GenericRepo<Teacher>(_ctx);
            }
            return _teacherRepo;
        }
    }

    public IRepository<Group> Groups
    {
        get
        {
            if (_groupRepo == null)
            {
                _groupRepo = new GenericRepo<Group>(_ctx);
            }
            return _groupRepo;
        }
    }

    public IRepository<Classroom> Classrooms
    {
        get
        {
            if (_classroomRepo == null)
            {
                _classroomRepo = new GenericRepo<Classroom>(_ctx);
            }
            return _classroomRepo;
        }
    }

    public IRepository<Discipline> Disciplines
    {
        get
        {
            if (_disciplineRepo == null)
            {
                _disciplineRepo = new GenericRepo<Discipline>(_ctx);
            }
            return _disciplineRepo;
        }
    }

    public IRepository<ScheduleItem> ScheduleItems
    {
        get
        {
            if (_scheduleItemRepo == null)
            {
                _scheduleItemRepo = new ScheduleItemRepository(_ctx);
            }
            return _scheduleItemRepo;
        }
    }
}
