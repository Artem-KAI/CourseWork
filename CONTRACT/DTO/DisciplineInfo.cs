namespace CONTRACT.DTO;

public class DisciplineInfo
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public DisciplineInfo() { }

    public DisciplineInfo(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
