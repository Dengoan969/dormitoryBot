using DormitoryBot.Domain.Marketplace;
using DormitoryBot.Infrastructure;

namespace DormitoryBot.Domain.SubscribitionService;

public class MockAdvertsRepository : IAdvertsRepository
{
    private readonly SortedSet<Advert> adverts;

    public MockAdvertsRepository(SortedSet<Advert> adverts)
    {
        this.adverts = adverts;
    }

    public void AddAdvert(Advert advert)
    {
        adverts.Add(advert);
    }

    public Advert[] GetAdverts()
    {
        return adverts.ToArray();
    }

    public Advert[] GetUserAdverts(long user)
    {
        return adverts.Where(x => x.Author == user).ToArray();
    }

    public void RemoveAdvert(Advert advert)
    {
        adverts.Remove(advert);
    }
}