namespace DomitoryBot.Domain
{
    public enum AdvertStatus
    {
        Active,
        Finished,
    }

    public class Advert //Record? (not class)
    {
        public readonly AdvertStatus AdvertStatus;
        public readonly Guid Author;
        public readonly DateTime CreationTime;
        public readonly Guid Guid;
        public readonly string Price;
        public readonly string Text;
        public readonly TimeSpan time = TimeSpan.FromDays(7);

        public Advert(Guid author, AdvertStatus advertStatus, DateTime creationTime, string text, string price,
            TimeSpan time)
        {
            Guid = Guid.NewGuid();
            Author = author;
            AdvertStatus = advertStatus;
            CreationTime = creationTime;
            Text = text;
            Price = price;
        }
    }

    public class MarketPlace
    {
        IAdvertsRepository repository = new AdvertMockRepository();
        public bool CreateAdvert()
        {
            throw new NotImplementedException();
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