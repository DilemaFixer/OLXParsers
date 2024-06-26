using System.Numerics;
using OfficeOpenXml;
using OLXCategoryParser.Config;
using Parser;
using Tools.Console;

namespace OLXCategoryParser.DaraResolver;

public class ExelDataResolver : IDataResolver
{
    private readonly IDataResolveStrategy[] _parsingStrategies;

    public ExelDataResolver(IParserStrategyConfiguration configuration)
    {
        _parsingStrategies = configuration.GetResolveStrategies();
    }

    public async Task Resolve(IParser parser)
    {
        Console.WriteLine("В ведите url продовца :");
        string url = Console.ReadLine();
        
        List<PageData> parsedData = await parser.Handling(url);

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        string pathToFolder = ConsoleExtension.ReadLineLoop(input => Directory.Exists(input),
            "В ведите путь , где будет создан файл с данными");
        string fileName = ConsoleExtension.ReadLineLoop(
            input => (input.EndsWith(".xlsx") && !IsFileExist(pathToFolder, input)),
            "В ведите имя для файла , пример (example.xlsx) :",
            " Имя файла не заканчиваеться на .xlsx или такой файл уже присуцтвует , повторите попытку :");
        
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Parse");
                
                foreach (PageData pageData in parsedData)
                {
                    SetColumns(worksheet);
                    SetData(worksheet, parsedData);
                }

                FileInfo file = new FileInfo(fileName);
                package.SaveAs(Path.Combine(pathToFolder , fileName));
            }


            Console.WriteLine("Файл успешно создан и заполнен");
    }

    private void SetData(ExcelWorksheet worksheet, List<PageData> parsedData)
    {
        for (int j = 0; j < parsedData.Count; j++)
        {
            for(int i = 0; i < _parsingStrategies.Length; i++)
            {
                _parsingStrategies[i].SetToColum(worksheet , parsedData[j] , new Vector2(j + 2 , i + 1)); 
            }
        }
    }

    private bool IsFileExist(string path, string fileName)
    {
        string pathToFile = Path.Combine(path, fileName);
        return File.Exists(path);
    }

    private void SetColumns(ExcelWorksheet worksheet)
    {
        for (int i = 0; i < _parsingStrategies.Length; i++)
        {
            _parsingStrategies[i].SetColumName(worksheet, i + 1);
        }
    }
}