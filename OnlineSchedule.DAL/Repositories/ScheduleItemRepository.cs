using DAL.EF;
using DAL.Entities;

using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class ScheduleItemRepository : GenericRepo<ScheduleItem>
{
    private readonly ScheduleContext _ctx;

    public ScheduleItemRepository(ScheduleContext ctx) : base(ctx)
    {
        _ctx = ctx;
    }

    public override async Task<IEnumerable<ScheduleItem>> GetAllAsync()
    {
        return await _ctx.ScheduleItems
            .Include(x => x.Group)
            .Include(x => x.Teacher)
            .Include(x => x.Classroom)
            .Include(x => x.Discipline)
            .ToListAsync();
    }

    public override async Task<ScheduleItem?> GetAsync(int id)
    {
        return await _ctx.ScheduleItems
            .Include(x => x.Group)
            .Include(x => x.Teacher)
            .Include(x => x.Classroom)
            .Include(x => x.Discipline)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}