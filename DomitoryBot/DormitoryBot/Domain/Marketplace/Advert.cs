namespace DormitoryBot.Domain.Marketplace;

public class Advert //Record? (not class)
{
    public readonly AdvertStatus AdvertStatus = AdvertStatus.Active;
    public readonly long Author;
    public readonly DateTime CreationTime = DateTime.Now;
    public readonly Guid Guid = Guid.NewGuid();
    public readonly string Price;
    public readonly string Text;
    public readonly TimeSpan TimeToLive;
    public readonly string Username;

    public Advert(long author, string text, string price, TimeSpan timeToLive, string username)
    {
        Author = author;
        Text = text;
        Price = price;
        TimeToLive = timeToLive;
        Username = username;
    }
}