namespace DormitoryBot.Domain.Marketplace;

public record Advert
    (long Author, string Text, string Price, TimeSpan TimeToLive, string Username)
{
    public readonly AdvertStatus AdvertStatus = AdvertStatus.Active;
    public readonly DateTime CreationTime = DateTime.Now;
}