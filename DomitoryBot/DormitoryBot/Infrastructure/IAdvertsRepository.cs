using DormitoryBot.Domain.Marketplace;

namespace DormitoryBot.Infrastructure;

public interface IAdvertsRepository
{
    void AddAdvert(Advert advert);
    void RemoveAdvert(Advert advert);
    Advert[] GetAdverts();
    Advert[] GetUserAdverts(long user);
}