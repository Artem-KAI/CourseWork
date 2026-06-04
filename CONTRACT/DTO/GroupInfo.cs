namespace CONTRACT.DTO;

public class GroupInfo
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;

    public GroupInfo() { }

    public GroupInfo(int id, string name, int departmentId, string departmentName)
    {
        Id = id;
        Name = name;
        DepartmentId = departmentId;
        DepartmentName = departmentName;
    }
}
