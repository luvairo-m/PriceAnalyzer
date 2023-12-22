using FluentAssertions;
using NUnit.Framework;

namespace AvitoParser.Tests;

[TestFixture]
public class ParserHelperTests
{
    [TestCase("Москва, 1-й Кожевнический пер., 6с6", "Москва", "Москва")]
    [TestCase("Москва, Чистый пер.", "Москва", "Москва")]
    [TestCase("Брянск, улица Ленина", "Брянск", "Брянск")]
    [TestCase("Свердловская область, Екатеринбург", "Свердловская область", "Екатеринбург")]
    [TestCase("Курганская область, Курган, Климова 99", "Курганская область", "Курган")]
    public void GetLocationSummary_Should_ReturnCorrectValues(
        string location,
        string expectedRegion,
        string expectedCity)
    {
        var actual = ParserHelper.GetLocationSummary(location);
        actual.Should().Be((expectedRegion, expectedCity));
    }

    [TestCaseSource(nameof(CreateDateTimeFrom_TestData))]
    public void CreateDateTimeFrom_Should_ReturnCorrectValues(string dateString, DateTime expected)
    {
        var actual = ParserHelper.CombineDateTimeFrom(dateString);
        actual.Day.Should().Be(expected.Day);
        actual.Year.Should().Be(expected.Year);
        actual.Month.Should().Be(expected.Month);
    }

    public static IEnumerable<TestCaseData> CreateDateTimeFrom_TestData()
    {
        yield return new TestCaseData("25 марта 2004", new DateTime(2004, 3, 25));
        yield return new TestCaseData("1 января 1987", new DateTime(1987, 1, 1));
        yield return new TestCaseData("8 июля 2050", new DateTime(2050, 7, 8));
        yield return new TestCaseData("23 августа 2015", new DateTime(2015, 8, 23));
        yield return new TestCaseData("15 ноября 2019", new DateTime(2019, 11, 15));
        yield return new TestCaseData("1 минуту назад", DateTime.Now.AddMinutes(-1));
        yield return new TestCaseData("15 минут назад", DateTime.Now.AddMinutes(-15));
        yield return new TestCaseData("1 день назад", DateTime.Now.AddDays(-1));
        yield return new TestCaseData("3 дня назад", DateTime.Now.AddDays(-3));
        yield return new TestCaseData("25 дней назад", DateTime.Now.AddDays(-25));
        yield return new TestCaseData("1 час назад", DateTime.Now.AddHours(-1));
        yield return new TestCaseData("4 часа назад", DateTime.Now.AddHours(-4));
        yield return new TestCaseData("9 часов назад", DateTime.Now.AddHours(-9));
        yield return new TestCaseData("1 неделю назад", DateTime.Now.AddDays(-1 * 7));
        yield return new TestCaseData("5 недель назад", DateTime.Now.AddDays(-5 * 7));
    }
}