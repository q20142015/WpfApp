using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;

namespace WpfApp
{
    internal class ExportDataToExcel
    {
        public static void export(ref SortedSet<Equipment> equipmentList)
        {
            ExcelPackage.License.SetNonCommercialOrganization("My Noncommercial organization");
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                worksheet.Cells["A1"].LoadFromCollection(equipmentList, true);
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                excelPackage.SaveAs(new FileInfo(DateTime.Now.ToString("dd.MM.yyyy hh.mm.ss") +".xlsx"));
            }
        }
    }
}
