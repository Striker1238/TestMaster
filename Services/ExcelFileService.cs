using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMaster.Models.App;

namespace TestMaster.Services
{
    public class ExcelFileService
    {        
        public static void ExportResultToExcel(Result result, string filePath)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Результат");

            worksheet.Cell(1, 1).Value = "ФИО";
            worksheet.Cell(1, 2).Value = "Табельный номер";
            worksheet.Cell(1, 3).Value = "Название теста";
            worksheet.Cell(1, 4).Value = "Всего вопросов";
            worksheet.Cell(1, 5).Value = "Правильных ответов";

            worksheet.Cell(2, 1).Value = result.FullName;
            worksheet.Cell(2, 2).Value = result.PersonnelNumber;
            worksheet.Cell(2, 3).Value = result.ComplatedTest?.Title ?? "—";
            worksheet.Cell(2, 4).Value = result.CountQuestions;
            worksheet.Cell(2, 5).Value = result.CountCorrectAnswer;


            worksheet.Protect(SettingsService.GetString("ExportSettings:Key"));
            foreach (var cell in worksheet.CellsUsed())
            {
                cell.Style.Protection.SetLocked(true);
            }

            workbook.SaveAs(filePath);
        }
    }
}
