using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using DevExpress.Spreadsheet;
using Microsoft.Office.Interop.Excel;

namespace UI_GestionRRHH.Formularios.Personal.AccionesExcel
{
    public  class AccionCelda
    {
        public static Action<Workbook> MergeAndSplitCellsAction = MergeAndSplitCells;


        static void MergeAndSplitCells(Workbook workbook)
        {
            DevExpress.Spreadsheet.Worksheet worksheet = workbook.Worksheets[0];

            worksheet.Cells["A1"].FillColor = Color.LightGray;

            worksheet.Cells["B2"].Value = "B2";
            worksheet.Cells["B2"].FillColor = Color.LightGreen;

            worksheet.Cells["C3"].Value = "C3";
            worksheet.Cells["C3"].FillColor = Color.LightSalmon;

            #region #MergeCells
            // Merge cells contained in the "A1:C5" range.
            worksheet.MergeCells(worksheet.Range["A1:C5"]);
            #endregion #MergeCells
        }
    }
}
