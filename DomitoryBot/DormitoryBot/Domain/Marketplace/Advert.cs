namespace DormitoryBot.Domain.Marketplace;

public record Advert
    (long Author, string Username, string Text, string Price, DateTime CreationTime, TimeSpan TimeToLive);
