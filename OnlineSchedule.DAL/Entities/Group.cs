namespace DAL.Entities;

public class Group
{
    public int Id { get; set; }
    public string Name { get; set; } 
    public int DepartmentId { get; set; }
    public virtual Department? Department { get; set; }

    public virtual ICollection<ScheduleItem> ScheduleItems { get; set; } 
}
