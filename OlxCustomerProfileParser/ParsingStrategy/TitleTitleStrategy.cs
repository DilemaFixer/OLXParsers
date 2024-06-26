using HtmlAgilityPack;
using Parser;

namespace OLXCategoryParser;

public class TitleTitleStrategy : IParsingStrategy,IDataResolveStrategy
{
    public string StrategyTag => "title";

    public void Parse(HtmlDocument page, PageData pageData)
    {
        Console.Write("Parsing title ");
        
        HtmlNode titleNode = page.DocumentNode.SelectSingleNode(".//h4[contains(@class, 'css-1juynto')]");

        pageData.Data.Add(StrategyTag, new[] { titleNode.InnerText });
    }
}