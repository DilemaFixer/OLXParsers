using System.Numerics;
using OfficeOpenXml;
using Parser;

namespace OLXCategoryParser;

public interface IDataResolveStrategy
{
    public string StrategyTag { get; }
    
    public void SetToColum(ExcelWorksheet worksheet, PageData pageData, Vector2 columPosition) =>
        worksheet.Cells[(int)columPosition.X, (int)columPosition.Y].Value = pageData.Data[StrategyTag].ArrLineUp();

    public void SetColumName(ExcelWorksheet worksheet, int columnId) =>
        worksheet.Cells[1, columnId].Value = StrategyTag;
}