namespace DormitoryBot.Domain.Marketplace
{
    public interface IAdvertsRepository
    {
        void AddAdvert(Advert advert);
        void RemoveAdvert(Advert advert);
        Advert[] Adverts { get; }
        Advert[] GetUserAdverts(long user);
    }
}