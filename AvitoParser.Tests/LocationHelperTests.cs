using AvitoParser.Helpers;
using NUnit.Framework;

namespace AvitoParser.Tests;

[TestFixture]
public class LocationHelperTests
{
    [TestCase("https://www.avito.ru/kurganskaya_oblast/moped", "Курганская область")]
    [TestCase("https://www.avito.ru/omsk/moped", "Омск")]
    [TestCase("https://www.avito.ru/gus-hrustalnyy/moped", "Гусь-Хрустальный")]
    [TestCase("https://www.avito.ru/balashiha/moped", "Балашиха")]
    [TestCase("https://www.avito.ru/dagestan/moped", "Дагестан")]
    public void GetCityFromUrl_Should_ReturnCorrectValue(string url, string expected)
    {
        var actual = LocationHelper.GetCityFromUrl(url);
        Assert.That(actual, Is.EqualTo(expected));
    }
}