using HtmlAgilityPack;
using OLXCategoryParser;
using OLXCategoryParser.Config;
using Parser;
using PuppeteerSharp;
using Tools.Console;

namespace OlxCustomerProfileParser.Parser;

public class OlxCustomerProfileParser : IParser 
{
    private const string BASE_OLX_URL = "https://www.olx.ua";
    private const string PAGE_REFIX = "?page=";
    
    private readonly IParsingStrategy[] _parsingStrategies;
    private readonly HtmlWeb _web = new();
    private IBrowser _browser;

    public OlxCustomerProfileParser(IParserStrategyConfiguration configuration) =>
       _parsingStrategies  = configuration.GetStrategy();

    public async Task<List<PageData>?> Handling(string url)
    {
       await LoadingBrowser();
        int countOfParsingPages = 1;

        ConsoleExtension.ReadLineLoop(
            input => IsInputValid(input, out countOfParsingPages),
            "Введите количество страниц, которое нужно спарсить:"
        );

        List<PageData> pageDates = new();

        for (int i = 0; i < countOfParsingPages; i++)
        {
            Console.WriteLine($"Начал парсить страницу {i+1}");
            var result = await HandlingPage($"{url}{PAGE_REFIX}{i+1}");
            pageDates.AddRange(result);
        }

        await _browser.CloseAsync();
        _browser = null;
        
        return pageDates;
    }

    private async Task LoadingBrowser()
    {
        var _browserFetcher = new BrowserFetcher();
        _browserFetcher.DownloadAsync().GetAwaiter().GetResult();
        _browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true});
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

        var links = ExtractLinksFromNodes(cleanProductNodes);
        List<PageData> pageDates = new();
        
        foreach (string link in links)
            pageDates.Add(await ExtractInfo(link));

        return pageDates;
    }

    private async Task<HtmlDocument> ExtractCatalogPage(string url)
    {
        HtmlDocument htmlDocument = new();

        using (var page = await _browser.NewPageAsync())
        {
            await page.GoToAsync(url);

            await page.WaitForSelectorAsync(".css-gdx2ks");

            var content = await page.GetContentAsync();
            
            htmlDocument.LoadHtml(content);
        }

        return htmlDocument;
    }

    private List<HtmlNode>? GetProductsNode(HtmlDocument htmlDocument) =>
        htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'css-1sw7q4x')]").ToList();

    private List<string> ExtractLinksFromNodes(List<HtmlNode> productNodes)
    {
        var nodes = productNodes.Select(node => node.SelectSingleNode(".//a[contains(@class, 'css-z3gu2d')]"));
        
        return nodes.Where(linkNode => linkNode != null)
            .Select(linkNode => linkNode.GetAttributeValue("href", string.Empty))
            .ToList();
    }


    private async Task<PageData> ExtractInfo(string link)
    {
        link = FormatLink(link);
        
        Console.WriteLine($"Обработка страницы , url : {link}");
        HtmlDocument doc = _web.Load(link);

        PageData pageData = new();

        foreach (IParsingStrategy strategy in _parsingStrategies)
            strategy.Parse(doc, pageData);
        
        Console.WriteLine(" ");
        return pageData;
    }

    private string FormatLink(string link) =>
        TruncateStringFromEnd( BASE_OLX_URL + link , 22);
        
    private string TruncateStringFromEnd(string input, int numberOfCharactersToRemove) =>
        input.Length < numberOfCharactersToRemove ? string.Empty : input.Substring(0, input.Length - numberOfCharactersToRemove);
   
}