using DormitoryBot.Domain.SubscriptionService;
using FakeItEasy;
using NUnit.Framework;

namespace TestProject
{
    public class SubscriptionTests
    {
        private ISubscriptionRepository repository;
        private SubscriptionService subscriptionService;

        [SetUp]
        public void Setup()
        {
            repository = A.Fake<ISubscriptionRepository>();
            subscriptionService = new SubscriptionService(repository);
        }

        //[Test]
        //public void TestMarketplace_CreateAdvert_CallAddAdvert()
        //{
        //    var advert = A.Dummy<Advert>();
        //    marketplace.CreateAdvert(advert.Author, advert.Text, advert.Price, advert.TimeToLive, advert.Username);
        //    A.CallTo(() => repository.AddAdvert(advert)).MustHaveHappenedOnceExactly();
        //}

        //public void TestMarketplace_RemoveAdvert_CallRemoveAdvert()
        //{

        //}
    }
}