namespace DormitoryBot.Domain.Marketplace;

public class AdvertsComparator : IComparer<Advert>
{
    public int Compare(Advert? x, Advert? y)
    {
        var firstTimeToExpire = x.CreationTime + x.TimeToLive;
        var secondTimeToExpire = y.CreationTime + y.TimeToLive;
        return DateTime.Compare(firstTimeToExpire, secondTimeToExpire);
    }
}