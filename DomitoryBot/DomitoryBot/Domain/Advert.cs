namespace DomitoryBot.Domain;

public class Advert //Record? (not class)
{
    public readonly AdvertStatus AdvertStatus = AdvertStatus.Active;
    public readonly long Author;
    public readonly DateTime CreationTime = DateTime.Now;
    public readonly Guid Guid = Guid.NewGuid();
    public readonly string Price;
    public readonly string Text;
    public readonly TimeSpan Time = TimeSpan.FromDays(7);

    public Advert(long author, string text, string price, TimeSpan time)
    {
        Author = author;
        Text = text;
        Price = price;
        Time = time;
    }
}