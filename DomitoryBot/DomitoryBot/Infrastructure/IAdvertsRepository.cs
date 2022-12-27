using DomitoryBot.Domain;

namespace DomitoryBot.Infrastructure;

public interface IAdvertsRepository
{
    void AddAdvert(Advert advert);
    void RemoveAdvert(Advert advert);
    Advert[] GetAdverts();
    Advert[] GetUserAdverts(long user);
}