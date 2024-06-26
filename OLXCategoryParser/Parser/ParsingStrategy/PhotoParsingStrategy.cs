using HtmlAgilityPack;
using Parser;

namespace OLXCategoryParser;

public class PhotoParsingStrategy : IParsingStrategy,IDataResolveStrategy
{
    public string StrategyTag => "photos";
    public void Parse(HtmlDocument page, PageData pageData)
    {
        Console.Write("Parsing photo ");
        
        HtmlNode photoWrapper = page.DocumentNode.SelectSingleNode(".//div[contains(@class, 'css-1uilkl7')]");
        var photoNodes = photoWrapper.SelectNodes(".//div[contains(@class, 'swiper-zoom-container')]//img");

        string[] photoUrls = new string[photoNodes.Count];

        for (int i = 0; i < photoNodes.Count; i++)
        {
            photoUrls[i] = photoNodes[i].GetAttributeValue("src", string.Empty);
        }

        pageData.Data.Add(StrategyTag , photoUrls);
    }
}