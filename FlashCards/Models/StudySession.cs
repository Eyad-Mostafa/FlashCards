namespace FlashCards.Models;

internal class StudySession
{
    public int StudySessionID { get; set; }
    public int StackID { get; set; }
    public DateTime SessionDate { get; set; }
    public int Score { get; set; }
}
