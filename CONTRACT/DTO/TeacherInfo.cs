namespace CONTRACT.DTO;

public class TeacherInfo
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Degree { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public int? UserId { get; set; }

    public TeacherInfo() { }

    public TeacherInfo(int id, string name, string degree, int departmentId, string departmentName, int? userId)
    {
        Id = id;
        Name = name;
        Degree = degree;
        DepartmentId = departmentId;
        DepartmentName = departmentName;
        UserId = userId;
    }
}
