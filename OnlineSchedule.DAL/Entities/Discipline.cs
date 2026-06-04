namespace DAL.Entities;

public class Discipline
{
    public int Id { get; set; }
    public string Name { get; set; } 

    public virtual ICollection<ScheduleItem> ScheduleItems { get; set; }
}
