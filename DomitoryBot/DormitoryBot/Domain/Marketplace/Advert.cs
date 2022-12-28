namespace DormitoryBot.Domain.Marketplace;

public record Advert
    (long Author, string Text, string Price, TimeSpan TimeToLive, string Username)
{
    public readonly AdvertStatus AdvertStatus = AdvertStatus.Active;
    public readonly long Author = Author;
    public readonly DateTime CreationTime = DateTime.Now;
    public readonly Guid Guid = Guid.NewGuid();
    public readonly string Price = Price;
    public readonly string Text = Text;
    public readonly TimeSpan TimeToLive = TimeToLive;
    public readonly string Username = Username;
}