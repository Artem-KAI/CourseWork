using System;

namespace CONTRACT.DTO;

public class ScheduleItemInfo
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public int TeacherId { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public int ClassroomId { get; set; }
    public string ClassroomName { get; set; } = string.Empty;
    public int DisciplineId { get; set; }
    public string DisciplineName { get; set; } = string.Empty;
    public DayOfWeek DayOfWeek { get; set; }
    public int LessonNumber { get; set; }
    public string WeekType { get; set; } = string.Empty;

    public ScheduleItemInfo() { }

    public ScheduleItemInfo(
        int id,
        int groupId,
        string groupName,
        int teacherId,
        string teacherName,
        int classroomId,
        string classroomName,
        int disciplineId,
        string disciplineName,
        DayOfWeek dayOfWeek,
        int lessonNumber,
        string weekType)
    {
        Id = id;
        GroupId = groupId;
        GroupName = groupName;
        TeacherId = teacherId;
        TeacherName = teacherName;
        ClassroomId = classroomId;
        ClassroomName = classroomName;
        DisciplineId = disciplineId;
        DisciplineName = disciplineName;
        DayOfWeek = dayOfWeek;
        LessonNumber = lessonNumber;
        WeekType = weekType;
    }
}
