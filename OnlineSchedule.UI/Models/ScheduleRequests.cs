using System;

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