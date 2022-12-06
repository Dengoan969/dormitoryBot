namespace DomitoryBot.Domain
{
    internal class MarketPlace
    {
    }


    public enum AdvertStatus
    {
    }

    public class Advert
    {
        public readonly AdvertStatus AdvertStatus;
        public readonly Guid Author;
        public readonly DateTime CreationTime;
        public readonly string Price;
        public readonly string Text;

        public Advert(Guid author, AdvertStatus advertStatus, DateTime creationTime, string text, string price)
        {
            Author = author;
            AdvertStatus = advertStatus;
            CreationTime = creationTime;
            Text = text;
            Price = price;
        }
    }
}