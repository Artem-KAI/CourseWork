namespace CONTRACT.DTO;

public class ClassroomInfo
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Building { get; set; } = string.Empty;
    public int Capacity { get; set; }

    public ClassroomInfo() { }

    public ClassroomInfo(int id, string name, string building, int capacity)
    {
        Id = id;
        Name = name;
        Building = building;
        Capacity = capacity;
    }
}
