using HtmlAgilityPack;
using Parser;

namespace OLXCategoryParser;

public class CustomerParsingStrategy : IParsingStrategy ,IDataResolveStrategy
{
    public string StrategyTag => "customer";

    public void Parse(HtmlDocument page, PageData pageData)
    {
        Console.Write("Parsing customer ");
        
        HtmlNode customerNameNode =
            page.DocumentNode.SelectSingleNode(".//h4[contains(@class, 'css-1lcz6o7 er34gjf0')]");

        pageData.Data.Add(StrategyTag, new[] { customerNameNode.InnerText });
    }
}