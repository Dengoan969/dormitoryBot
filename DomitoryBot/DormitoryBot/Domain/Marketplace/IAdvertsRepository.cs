using DormitoryBot.Domain.Marketplace;

namespace DomitoryBot.Domain.Marketplace;

public interface IAdvertsRepository
{
    void AddAdvert(Advert advert);
    void RemoveAdvert(Advert advert);
    Advert[] GetAdverts();
    Advert[] GetUserAdverts(long user);
}