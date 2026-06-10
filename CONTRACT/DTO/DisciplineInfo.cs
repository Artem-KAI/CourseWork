namespace CONTRACT.DTO;

public class DisciplineInfo
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DepartmentId { get; set; }

    public DisciplineInfo() { }

    public DisciplineInfo(int id, string name, int departmentId)
    {
        Id = id;
        Name = name;
        DepartmentId = departmentId;
    }
}
