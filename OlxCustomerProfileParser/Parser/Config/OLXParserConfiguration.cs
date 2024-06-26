namespace OLXCategoryParser.Config;

public class OLXParserConfiguration : IParserStrategyConfiguration
{
    private Sampling _sampling;

    public OLXParserConfiguration(Sampling sampling)
    {
        _sampling = sampling;
    }


    public IParsingStrategy[] GetStrategy()
    {
        List<IParsingStrategy> parsingStrategies = new();

        if (_sampling.Characteristics)
            parsingStrategies.Add(new CharacteristicsParsingStrategy());
        
        if(_sampling.Customer)
            parsingStrategies.Add(new CustomerParsingStrategy());
        
        if(_sampling.Description)
            parsingStrategies.Add(new DescriptionParsingStrategy());
        
        if(_sampling.Photo)
            parsingStrategies.Add(new PhotoParsingStrategy());
        
        if(_sampling.Title)
            parsingStrategies.Add(new TitleTitleStrategy());
        
        if(_sampling.Price)
            parsingStrategies.Add(new PriceParsingStrategy());

        if (parsingStrategies.Count == 0)
            throw new ArgumentException("Select data that need parse");

        return parsingStrategies.ToArray();
    }

    public IDataResolveStrategy[] GetResolveStrategies()
    {
        List<IDataResolveStrategy> parsingStrategies = new();

        if (_sampling.Characteristics)
            parsingStrategies.Add(new CharacteristicsParsingStrategy());
        
        if(_sampling.Customer)
            parsingStrategies.Add(new CustomerParsingStrategy());
        
        if(_sampling.Description)
            parsingStrategies.Add(new DescriptionParsingStrategy());
        
        if(_sampling.Photo)
            parsingStrategies.Add(new PhotoParsingStrategy());
        
        if(_sampling.Title)
            parsingStrategies.Add(new TitleTitleStrategy());
        
        if(_sampling.Price)
            parsingStrategies.Add(new PriceParsingStrategy());

        if (parsingStrategies.Count == 0)
            throw new ArgumentException("Select data that need parse");

        return parsingStrategies.ToArray();
    }
}