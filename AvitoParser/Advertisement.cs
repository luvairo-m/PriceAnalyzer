using CsvHelper.Configuration.Attributes;

namespace AvitoParser;

public class Advertisement
{
    [Name("Название")] public string Title { get; set; }

    [Name("Ссылка")] public string Url { get; set; }

    [Name("Расположение")] public string Location { get; set; }

    [Name("Цена")] public int Price { get; set; }

    [Name("Дата публикации")] public DateTime PublicationDate { get; set; }

    [Name("Является выбросом")] public bool IsOutlier { get; set; }

    [Name("Отклонение от медианы, %")] public int PriceDeviation { get; set; }
}