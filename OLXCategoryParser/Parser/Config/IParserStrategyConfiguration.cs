namespace OLXCategoryParser.Config;

public interface IParserStrategyConfiguration
{
    public IParsingStrategy[] GetStrategy();
    public IDataResolveStrategy[] GetResolveStrategies();
}