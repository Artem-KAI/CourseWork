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