namespace DAL.Entities;

public class Teacher
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Degree { get; set; } 

    public int DepartmentId { get; set; }
    public virtual Department? Department { get; set; }

    public int? UserId { get; set; }
    public virtual User? User { get; set; }

    public virtual ICollection<ScheduleItem> ScheduleItems { get; set; }
}
