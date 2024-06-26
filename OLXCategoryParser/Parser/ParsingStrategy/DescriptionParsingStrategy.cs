using HtmlAgilityPack;
using Parser;

namespace OLXCategoryParser;

public class DescriptionParsingStrategy : IParsingStrategy,IDataResolveStrategy
{
    public string StrategyTag => "description";

    public void Parse(HtmlDocument page, PageData pageData)
    {
        Console.Write("Parsing description ");
        
        HtmlNode descriptionNode =
            page.DocumentNode.SelectSingleNode(".//div[contains(@class, 'css-1t507yq er34gjf0')]");

        pageData.Data.Add(StrategyTag, new[] { ExtractTextInOneLine(descriptionNode) });
    }

    private string ExtractTextInOneLine(HtmlNode node)
    {
        if (node == null) return string.Empty;

        string text = node.InnerText;

        text = text.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ");

        text = System.Text.RegularExpressions.Regex.Replace(text, @"\s+", " ");

        return text.Trim();
    }
}