﻿using DomitoryBot.Domain;
using DomitoryBot.Infrastructure;

namespace DomitoryBot.App;

public class MockAdvertsRepository : IAdvertsRepository
{
    private readonly List<Advert> adverts = new();

    public void AddAdvert(Advert advert)
    {
        adverts.Add(advert);
    }

    public Advert[] GetAdverts()
    {
        return adverts.ToArray();
    }

    public Advert[] GetUserAdverts(long user)
    {
        return adverts.Where(x => x.Author == user).ToArray();
    }

    public void RemoveAdvert(Advert advert)
    {
        adverts.Remove(advert);
    }
}