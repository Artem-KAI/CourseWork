using System.Text.RegularExpressions;

namespace DAL.Entities;

public class ScheduleItem
{
    public int Id { get; set; }

    public int GroupId { get; set; }
    public virtual Group? Group { get; set; }

    public int TeacherId { get; set; }
    public virtual Teacher? Teacher { get; set; }

    public int ClassroomId { get; set; }
    public virtual Classroom? Classroom { get; set; }

    public int DisciplineId { get; set; }
    public virtual Discipline? Discipline { get; set; }

    public DayOfWeek DayOfWeek { get; set; }

    // 1st pair, 2nd pair ...
    public int LessonNumber { get; set; }  

    // Both, Odd, Even
    public WeekType WeekType { get; set; } 
}
