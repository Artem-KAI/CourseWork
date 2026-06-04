using System.Collections.Generic;

namespace DAL.Entities;

public class Classroom
{
    public int Id { get; set; }
    public string Name { get; set; } 
    public string Building { get; set; } 
    public int Capacity { get; set; }

    public virtual ICollection<ScheduleItem> ScheduleItems { get; set; } 
}
