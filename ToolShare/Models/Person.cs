namespace ToolShare.Models;

// abstract base class -> Member inherits from it (inheritance)
public abstract class Person
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
}
