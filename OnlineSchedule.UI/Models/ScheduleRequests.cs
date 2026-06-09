using BLL.Enums;

namespace UI.Models;

public class CreateScheduleItemRequest
{
    public int GroupId { get; set; }
    public int TeacherId { get; set; }
    public int ClassroomId { get; set; }
    public int DisciplineId { get; set; }

    public DayOfWeek DayOfWeek { get; set; }

    public int LessonNumber { get; set; }

    public string WeekType { get; set; } = "Both";
}

public class UpdateScheduleItemRequest
{
    public int GroupId { get; set; }
    public int TeacherId { get; set; }
    public int ClassroomId { get; set; }
    public int DisciplineId { get; set; }

    public DayOfWeek DayOfWeek { get; set; }

    public int LessonNumber { get; set; }

    public string WeekType { get; set; } = "Both";
}

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