using DormitoryBot.Infrastructure;

namespace DormitoryBot.Domain.Marketplace
{
    public class MarketPlace
    {
        private IAdvertsRepository repository;
        private Timer timer;

        public MarketPlace(IAdvertsRepository repository)
        {
            this.repository = repository;
            timer = new Timer(ClearExpiredAdverts, new object(), TimeSpan.FromDays(1), TimeSpan.FromHours(1));
        }

        public bool CreateAdvert(long author, string text, string price, TimeSpan time, string username)
        {
            var advert = new Advert(author, text, price, time, username);
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

        public void RemoveAdvert(Advert advert)
        {
            repository.RemoveAdvert(advert);
        }

        private void ClearExpiredAdverts(object obj)
        {
            var adverts = GetAdverts();
            for (var i = 0; i < adverts.Length; i++)
                if (DateTime.Now >= adverts[i].CreationTime + adverts[i].TimeToLive)
                    RemoveAdvert(adverts[i]);
                else
                    break;
        }
    }
}