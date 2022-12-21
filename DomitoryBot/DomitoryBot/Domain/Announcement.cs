namespace DomitoryBot.Domain;

public class Announcement
{
    public readonly string Text;
    public readonly string Topic;
    public readonly string Type;

    public Announcement(string topic, string text, string type)
    {
        Topic = topic;
        Text = text;
        Type = type;
    }

    public override string ToString()
    {
        return $"{Topic}\n{Text}\n{Type}";
    }
}