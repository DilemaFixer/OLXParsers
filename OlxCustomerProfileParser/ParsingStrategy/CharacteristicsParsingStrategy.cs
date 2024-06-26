using HtmlAgilityPack;
using Parser;

namespace OLXCategoryParser;

public class CharacteristicsParsingStrategy : IParsingStrategy , IDataResolveStrategy
{
    public string StrategyTag => "characteristics";

    public void Parse(HtmlDocument page, PageData pageData)
    {
        Console.Write("Parsing characteristic ");
        
        HtmlNode characteristicsWrapper = page.DocumentNode.SelectSingleNode("//ul[contains(@class, 'css-px7scb')]");

        if (characteristicsWrapper == null)
        {
            Console.WriteLine("Characteristics wrapper not found");
            return;
        }

        var characteristicNodes = characteristicsWrapper.SelectNodes(".//li[contains(@class, 'css-1r0si1e')]//p");

        if (characteristicNodes == null)
        {
            Console.WriteLine("Characteristic nodes not found");
            return;
        }

        string[] characteristics = new string[characteristicNodes.Count];

        for (int i = 0; i < characteristicNodes.Count; i++)
        {
            characteristics[i] = characteristicNodes[i].InnerText.Trim();
        }

        pageData.Data.Add("characteristics", characteristics);
    }
}