using DomitoryBot.Domain;
using DomitoryBot.Infrastructure;

namespace DomitoryBot.App
{
    public class MarketPlace
    {
        private IAdvertsRepository repository;

        public MarketPlace(IAdvertsRepository repository)
        {
            this.repository = repository;
        }

        public bool CreateAdvert(long author, string text, string price, TimeSpan time)
        {
            var advert = new Advert(author, text, price, time);
            repository.AddAdvert(advert);
            return true;
        }

        public Advert[] GetAdverts()
        {
            return repository.GetAdverts();
        }

        public Advert[] GetUserAdverts(long user)
        {
            return repository.GetUserAdverts(user);
        }

        public void RemoveAdvert(Guid advertGuid)
        {
            repository.RemoveAdvert(advertGuid);
        }
    }
}