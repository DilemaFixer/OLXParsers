using HtmlAgilityPack;
using OLXCategoryParser.Config;
using Parser;
using Tools.Console;

namespace OLXCategoryParser;

public class OLXCategoryParser : IParser
{
    private const string BASE_OLX_URL = "https://www.olx.ua";
    private const string PAGE_REFIX = "?page=";
    
    private readonly HtmlWeb _htmlWeb = new();
    private readonly IParsingStrategy[] _parsingStrategies;

    public OLXCategoryParser(IParserStrategyConfiguration configuration) =>
        _parsingStrategies = configuration.GetStrategy();

    public async Task<List<PageData>?> Handling(string url)
    {
        int countOfParsingPages = 1;

        ConsoleExtension.ReadLineLoop(
            input => IsInputValid(input, out countOfParsingPages),
            "Введите количество страниц, которое нужно спарсить:"
        );

        var tasks = Enumerable.Range(1, countOfParsingPages)
            .Select(async i =>
            {
                Console.WriteLine($"Начал парсить страницу {i}");
                return await HandlingPage($"{url}{PAGE_REFIX}{i}");
            });

        var results = await Task.WhenAll(tasks);
        return results.SelectMany(pageData => pageData).ToList();
    }


    private bool IsInputValid(string input, out int countOfParsingPages) =>
        int.TryParse(input, out countOfParsingPages) && countOfParsingPages > 0;

    private async Task<List<PageData>?> HandlingPage(string url)
    {
        var cleanProductNodes = GetProductsNode(await ExtractCatalogPage(url));
        if (cleanProductNodes == null)
        {
            Console.WriteLine("Product nodes not found");
            return null;
        }

        var tasks = ExtractLinksFromNodes(cleanProductNodes).Select(link => ExtractInfo(link));
        var productsDataTable = await Task.WhenAll(tasks);

        return productsDataTable.ToList();
    }

    private async Task<HtmlDocument> ExtractCatalogPage(string url) => _htmlWeb.Load(url);

    private List<HtmlNode>? GetProductsNode(HtmlDocument htmlDocument) => 
        htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'css-1ut25fa')]")
            .Where((value, index) => index % 2 == 0)
            .ToList();
    private List<string> ExtractLinksFromNodes(List<HtmlNode> productNodes) =>
        productNodes.Select(node => node.SelectSingleNode(".//a[contains(@class, 'css-z3gu2d')]"))
            .Where(linkNode => linkNode != null)
            .Select(linkNode => linkNode.GetAttributeValue("href", string.Empty))
            .ToList();


    private async Task<PageData> ExtractInfo(string link)
    {
        Console.WriteLine($"Обработка страницы , url : {BASE_OLX_URL + link}");
        HtmlDocument doc = _htmlWeb.Load(BASE_OLX_URL + link);

        PageData pageData = new();

        foreach (IParsingStrategy strategy in _parsingStrategies)
            strategy.Parse(doc, pageData);
        Console.WriteLine(" ");
        return pageData;
    }
}