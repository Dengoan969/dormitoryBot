namespace DormitoryBot.Domain.Marketplace
{
    public class MockAdvertsRepository : IAdvertsRepository
    {
        private readonly SortedSet<Advert> adverts;

        public MockAdvertsRepository(SortedSet<Advert> adverts)
        {
            this.adverts = adverts;
        }

        public MockAdvertsRepository() : this(new SortedSet<Advert>()) { }

        public Advert[] Adverts => adverts.ToArray();

        public void AddAdvert(Advert advert)
        {
            adverts.Add(advert);
        }

        public void RemoveAdvert(Advert advert)
        {
            adverts.Remove(advert);
        }

        public Advert[] GetUserAdverts(long user)
        {
            return adverts.Where(x => x.Author == user).ToArray();
        }
    }
}