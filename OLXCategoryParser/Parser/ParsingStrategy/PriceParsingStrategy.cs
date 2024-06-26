using HtmlAgilityPack;
using Parser;

namespace OLXCategoryParser;

public class PriceParsingStrategy : IParsingStrategy,IDataResolveStrategy
{
    public string StrategyTag => "price";

    public void Parse(HtmlDocument page, PageData pageData)
    {
        Console.Write("Parsing price ");
        
        HtmlNode priceNode = page.DocumentNode.SelectSingleNode(".//h3[contains(@class, 'css-12vqlj3')]");

        pageData.Data.Add(StrategyTag, new[] { priceNode.InnerText });
    }
}