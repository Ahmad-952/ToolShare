namespace ToolShare.Services;

public class UserState
{
    public int MemberId { get; set; } = 2;   // 1 = Ali (owner), 2 = Sara (borrower)
    public string Name => MemberId == 1 ? "Ali Raza" : "Sara Khan";
}
