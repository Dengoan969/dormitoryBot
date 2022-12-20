namespace DomitoryBot.Domain
{
    public enum AdvertStatus
    {
        Active,
        Finished,
    }

    public class Advert //Record? (not class)
    {
        public readonly Guid Guid = Guid.NewGuid();
        public readonly DateTime CreationTime = DateTime.Now;
        public readonly AdvertStatus AdvertStatus = AdvertStatus.Active;
        public readonly Guid Author;
        public readonly string Price;
        public readonly string Text;
        public readonly TimeSpan Time = TimeSpan.FromDays(7);

        public Advert(Guid author, string text, string price, TimeSpan time)
        {
            Author = author;
            Text = text;
            Price = price;
            Time = time;
        }
    }

    public class MarketPlace
    {
        IAdvertsRepository repository = new AdvertMockRepository();
        public bool CreateAdvert(Guid author, string text, string price, TimeSpan time)
        {
            var advert = new Advert(author, text, price, time);
            repository.AddAdvert(advert);
            return true;
        }

        public Advert[] GetAdverts()
        {
            return repository.GetAdverts();
        }

        public Advert[] GetUserAdverts(Guid user)
        {
            return repository.GetUserAdverts(user);
        }

        public void RemoveAdvert(Guid advertGuid)
        {
            repository.RemoveAdvert(advertGuid);
        }
    }

    public interface IAdvertsRepository
    {
        void AddAdvert(Advert advert);
        void RemoveAdvert(Guid advertGuid);
        Advert[] GetAdverts();
        Advert[] GetUserAdverts(Guid user);
    }

    public class AdvertMockRepository : IAdvertsRepository
    {
        private readonly List<Advert> adverts = new List<Advert>();

        public void AddAdvert(Advert advert)
        {
            adverts.Add(advert);
        }

        public Advert[] GetAdverts()
        {
            return adverts.ToArray();
        }

        public Advert[] GetUserAdverts(Guid user)
        {
            return adverts.Where(x => x.Author == user).ToArray();
        }

        public void RemoveAdvert(Guid advertGuid)
        {
            adverts.Remove(adverts.FirstOrDefault(x => x.Guid == advertGuid));
        }
    }
}