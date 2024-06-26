using System.Numerics;
using HtmlAgilityPack;
using Parser;

namespace OLXCategoryParser;

public interface IParsingStrategy
{
    public string StrategyTag { get; }
    void Parse(HtmlDocument page, PageData pageData);
    
}