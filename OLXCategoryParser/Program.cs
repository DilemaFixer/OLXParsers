using OLXCategoryParser.Config;
using OLXCategoryParser.DaraResolver;
using Parser;
using SBot.Save;

namespace OLXCategoryParser
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            JsonDataSaver<Sampling> saver = new JsonDataSaver<Sampling>(AppDomain.CurrentDomain.BaseDirectory,
                "Sampling.json", () => new Sampling());

            Sampling sampling = saver.Load();

            OLXParserConfiguration olxParserConfiguration = new OLXParserConfiguration(sampling);
            
            IDataResolver dataResolver = new ExelDataResolver(olxParserConfiguration);

            IParser parser = new OLXCategoryParser(olxParserConfiguration);
            
           await dataResolver.Resolve(parser);
        }
    }
}