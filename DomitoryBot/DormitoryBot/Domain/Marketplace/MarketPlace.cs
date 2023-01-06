using DormitoryBot.Domain.Marketplace;

namespace DormitoryBot.Domain.Marketplace
{
    public class MarketPlace
    {
        private readonly IAdvertsRepository repository;
        private readonly Timer timer;

        public MarketPlace(IAdvertsRepository repository)
        {
            this.repository = repository;
            timer = new Timer(ClearExpiredAdverts, new object(), TimeSpan.FromDays(1), TimeSpan.FromHours(1));
        }

        public Advert[] Adverts => repository.Adverts;

        public void CreateAdvert(long author, string text, string price, TimeSpan time, string username)
        {
            var advert = new Advert(author, text, price, time, username);
            repository.AddAdvert(advert);
        }

        public Advert[] GetUserAdverts(long user)
        {
            return repository.GetUserAdverts(user);
        }

        public void RemoveAdvert(Advert advert)
        {
            repository.RemoveAdvert(advert);
        }

        private void ClearExpiredAdverts(object stateInfo)
        {
            for (var i = 0; i < Adverts.Length; i++)
                if (DateTime.Now >= Adverts[i].CreationTime + Adverts[i].TimeToLive)
                    RemoveAdvert(Adverts[i]);
                else
                    break;
        }
    }
}