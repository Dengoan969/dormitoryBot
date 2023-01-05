using System;
using System.Collections.Generic;
using DormitoryBot.Domain.Marketplace;
using DormitoryBot.Infrastructure;
using Ninject;
using NUnit.Framework;

namespace TestProject;

public class AdvertTests
{
    private StandardKernel container => new();

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestAdvert_WhenAllOkay()
    {
        var c = container;
        c.Bind<IAdvertsRepository>().To<MockAdvertsRepository>()
            .WithConstructorArgument("adverts",
                new SortedSet<Advert>(new AdvertsComparator())
            );
        var mp = c.Get<MarketPlace>();
        mp.CreateAdvert(1, "ad", "100", TimeSpan.FromDays(1), "1");
        Assert.IsNotEmpty(mp.GetAdverts());
        var advert = mp.GetAdverts()[0];
        Assert.Contains(advert, mp.GetUserAdverts(1));
        mp.RemoveAdvert(advert);
        Assert.IsEmpty(mp.GetAdverts());
        Assert.IsEmpty(mp.GetUserAdverts(1));
    }

    [Test]
    public void TestAdvert_TestDoesNotContainsOtherUserAdverts()
    {
        var c = container;
        var adverts = new[]
        {
            new Advert(1, "ad", "100", TimeSpan.FromDays(1), "f"),
            new Advert(2, "ad", "100", TimeSpan.FromDays(1), "d"),
            new Advert(2, "ad", "100", TimeSpan.FromDays(1), "d")
        };
        c.Bind<IAdvertsRepository>().To<MockAdvertsRepository>().InSingletonScope()
            .WithConstructorArgument("adverts",
                new SortedSet<Advert>(adverts, new AdvertsComparator())
            );
        var mp = c.Get<MarketPlace>();
        Assert.Contains(adverts[0], mp.GetUserAdverts(1));
        Assert.AreEqual(1, mp.GetUserAdverts(1).Length);
        Assert.Contains(adverts[1], mp.GetUserAdverts(2));
        Assert.Contains(adverts[2], mp.GetUserAdverts(2));
        Assert.AreEqual(2, mp.GetUserAdverts(2).Length);
    }
}