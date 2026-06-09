using BLL.Enums;

namespace WebAPI.Models
{
    public class CreateDepartmentRequest
    {
        public DepartmentName Name { get; set; }
    }

    public class CreateGroupRequest
    {
        public string Name { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
    }

    public class CreateTeacherRequest
    {
        public string Name { get; set; } = string.Empty;
        public TeacherDegree Degree { get; set; }
        public int DepartmentId { get; set; }
        public int? UserId { get; set; }
    }

    public class CreateClassroomRequest
    {
        public ClassroomName Name { get; set; }
        public ClassroomBuilding Building { get; set; }
        public int Capacity { get; set; }
    }

    public class CreateDisciplineRequest
    {
        public string Name { get; set; } = string.Empty;
    }
}
