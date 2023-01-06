using DormitoryBot.Domain.Marketplace;

namespace DormitoryBot.Domain.Marketplace;

public interface IAdvertsRepository
{
    void AddAdvert(Advert advert);
    void RemoveAdvert(Advert advert);
    Advert[] GetAdverts();
    Advert[] GetUserAdverts(long user);
}