using System.Text.RegularExpressions;

namespace DAL.Entities;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } 

    public virtual ICollection<Teacher> Teachers { get; set; } 
    public virtual ICollection<Group> Groups { get; set; } 
}
