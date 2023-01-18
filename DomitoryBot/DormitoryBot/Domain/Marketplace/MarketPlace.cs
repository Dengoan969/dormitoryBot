using DormitoryBot.Infrastructure;
using DormitoryBot.Domain.Marketplace;

namespace DormitoryBot.Domain.Marketplace
{
    public class MarketPlace
    {
        private readonly IAdvertsRepository repository;
        private readonly IDateTimeService dateTimeService;
        private readonly Timer timer;

        public MarketPlace(IAdvertsRepository repository, IDateTimeService dateTimeService)
        {
            this.repository = repository;
            this.dateTimeService = dateTimeService;
            timer = new Timer(ClearExpiredAdverts, new object(), TimeSpan.FromDays(1), TimeSpan.FromHours(1));
        }

        public Advert[] Adverts => repository.Adverts;

        public void CreateAdvert(long author, string text, string price, TimeSpan time, string username)
        {
            var advert = new Advert(author, username, text, price, dateTimeService.Now, time);
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
                if (dateTimeService.Now >= Adverts[i].CreationTime + Adverts[i].TimeToLive)
                    RemoveAdvert(Adverts[i]);
                else
                    break;
        }
    }
}