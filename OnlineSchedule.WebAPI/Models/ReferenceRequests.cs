namespace WebAPI.Models
{
    public class CreateDepartmentRequest
    {
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateDepartmentRequest
    {
        public string Name { get; set; } = string.Empty;
    }

    public class CreateGroupRequest
    {
        public string Name { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
    }

    public class UpdateGroupRequest
    {
        public string Name { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
    }

    public class CreateTeacherRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public int? UserId { get; set; }
    }

    public class UpdateTeacherRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public int? UserId { get; set; }
    }

    public class CreateClassroomRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Building { get; set; } = string.Empty;
        public int Capacity { get; set; }
    }

    public class UpdateClassroomRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Building { get; set; } = string.Empty;
        public int Capacity { get; set; }
    }

    public class CreateDisciplineRequest
    {
        public string Name { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
    }

    public class UpdateDisciplineRequest
    {
        public string Name { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
    }
}
