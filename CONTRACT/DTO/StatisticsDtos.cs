namespace CONTRACT.DTO;

public class TeacherWorkloadDto
{
    public int TeacherId { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public int TotalLessons { get; set; }

    public TeacherWorkloadDto() { }

    public TeacherWorkloadDto(int teacherId, string teacherName, int totalLessons)
    {
        TeacherId = teacherId;
        TeacherName = teacherName;
        TotalLessons = totalLessons;
    }
}

public class ClassroomUtilizationDto
{
    public int ClassroomId { get; set; }
    public string ClassroomName { get; set; } = string.Empty;
    public int OccupiedSlotsCount { get; set; }
    public double UtilizationPercentage { get; set; }

    public ClassroomUtilizationDto() { }

    public ClassroomUtilizationDto(int classroomId, string classroomName, int occupiedSlotsCount, double utilizationPercentage)
    {
        ClassroomId = classroomId;
        ClassroomName = classroomName;
        OccupiedSlotsCount = occupiedSlotsCount;
        UtilizationPercentage = utilizationPercentage;
    }
}

public class GroupLoadDto
{
    public int GroupId { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public int TotalLessons { get; set; }
    public double AverageLessonsPerDay { get; set; }

    public GroupLoadDto() { }

    public GroupLoadDto(int groupId, string groupName, int totalLessons, double averageLessonsPerDay)
    {
        GroupId = groupId;
        GroupName = groupName;
        TotalLessons = totalLessons;
        AverageLessonsPerDay = averageLessonsPerDay;
    }
}
