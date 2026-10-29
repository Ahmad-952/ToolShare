namespace ToolShare.Models;

// interface: anything that can be reserved/returned
public interface IBookable
{
    bool IsAvailable { get; }
    void MarkReserved();
    void MarkAvailable();
}
