namespace CONTRACT.DTO;

public class DepartmentInfo
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public DepartmentInfo() { }

    public DepartmentInfo(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
