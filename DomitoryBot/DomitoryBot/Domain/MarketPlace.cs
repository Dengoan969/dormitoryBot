namespace DomitoryBot.Domain
{
    public class MarketPlace
    {
        public bool CreateAdvert()
        {
            throw new NotImplementedException();
        }

        public Advert[] GetAdverts()
        {
            throw new NotImplementedException();
        }

        public Advert[] GetUserAdverts(Guid user)
        {
            throw new NotImplementedException();
        }

        public void RemoveAdvert(Guid advertGuid)
        {
            throw new NotImplementedException();
        }
    }


    public enum AdvertStatus
    {
        Active,
        Finished,
    }

    public class Advert
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

    public interface IAdvertsRepository
    {
        public void AddAdvert(Advert advert);
        public void RemoveAdvert(Advert advert);
    }

    public class AdvertMockRepository : IAdvertsRepository
    {
        public void AddAdvert(Advert advert)
        {
            throw new NotImplementedException();
        }

        public void RemoveAdvert(Advert advert)
        {
            throw new NotImplementedException();
        }
    }
}