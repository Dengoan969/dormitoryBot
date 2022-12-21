namespace DomitoryBot.Domain;

public interface IAdvertsRepository
{
    void AddAdvert(Advert advert);
    void RemoveAdvert(Guid advertGuid);
    Advert[] GetAdverts();
    Advert[] GetUserAdverts(long user);
}