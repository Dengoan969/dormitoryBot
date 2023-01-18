using DormitoryBot.Domain.Marketplace;
using DormitoryBot.Infrastructure;
using FakeItEasy;
using NUnit.Framework;

namespace TestProject
{
    public class MarketplaceTests
    {
        private IAdvertsRepository repository;
        private MarketPlace marketplace;

        [SetUp]
        public void Setup()
        {
            repository = A.Fake<IAdvertsRepository>();
            var dateTimeService = new DefaultDateTimeService();
            marketplace = new MarketPlace(repository, dateTimeService);
        }

        //[Test]
        //public void TestMarketplace_CreateAdvert_CallAddAdvert()
        //{
        //    var advert = A.Dummy<Advert>();
        //    marketplace.CreateAdvert(advert.Author, advert.Text, advert.Price, advert.TimeToLive, advert.Username);
        //    A.CallTo(() => repository.AddAdvert(advert)).MustHaveHappenedOnceExactly();
        //}

        public void TestMarketplace_RemoveAdvert_CallRemoveAdvert()
        {

        }
    }
}