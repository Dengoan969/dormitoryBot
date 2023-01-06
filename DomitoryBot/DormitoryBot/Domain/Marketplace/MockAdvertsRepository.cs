using DormitoryBot.Domain.Marketplace;

namespace DormitoryBot.Domain.Marketplace;

public class MockAdvertsRepository : IAdvertsRepository
{
    private readonly SortedSet<Advert> adverts;

    public MockAdvertsRepository(SortedSet<Advert> adverts)
    {
        this.adverts = adverts;
    }

    public MockAdvertsRepository() : this(new SortedSet<Advert>()) { }

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