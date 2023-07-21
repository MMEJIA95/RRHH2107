using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI_GestionRRHH.Formularios.Correspondencia
{
    internal class ReadFiles
    {
        private static string pathFile(string extension)
        {
            string dsc = "All files |*.*";
            switch (extension)
            {
                case "pdf":
                    dsc = "PDF (pdf) |*.pdf";
                    break;
                case "docx":
                    dsc = "Word Old(doc) |*.doc|Word New(docx) |*.docx";
                    break;
                case "xlsx":
                    dsc = "Excel Old(xls) |*.xls|Excel New(xlsx) |*.xlsx";
                    break;
            }
            string path = "";
            var oFile = new System.Windows.Forms.OpenFileDialog();
            //oFile.InitialDirectory = System.Windows.Forms.Application.StartupPath + @"\Excel";
            oFile.Filter = dsc;
            oFile.Title = "Open File";
            oFile.ShowDialog();
            if (oFile.FileName != "")
                path = Path.GetFullPath(oFile.FileName);
            return path;
        }
        internal static string pathFolder(string path = "")
        {
            #region heeelp
            // var _of = new OpenFileDialog();
            //_of.Multiselect = false;
            //if (_of.ShowDialog() == DialogResult.OK)
            //{
            //    string fileName = Path.GetFileName(_of.FileName);
            //    string filePath = _of.FileName;
            //    MessageBox.Show(fileName + " - " + filePath);
            //}

            //string message = "";
            //_of.Multiselect = true;
            //if (_of.ShowDialog() == DialogResult.OK)
            //{
            //    foreach (string file in _of.FileNames)
            //    {
            //        message += Path.GetFileName(file) + " - " + file + Environment.NewLine;
            //    }
            //    MessageBox.Show(message);
            //}
            #endregion

            var folder = new System.Windows.Forms.FolderBrowserDialog();
            //folder.RootFolder = Environment.SpecialFolder.MyComputer;//Environment.SpecialFolder.LocalApplicationData;// @"C:\";
            //folder.SelectedPath = path.Length>0?path;

            if (path.Length > 0) folder.SelectedPath = path;
            else folder.RootFolder = Environment.SpecialFolder.MyComputer; ;

            if (folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                // shows the path to the selected folder in the folder dialog
                // MessageBox.Show(fbd.SelectedPath);
                return $@"{folder.SelectedPath}\";
            else return "";
        }
    }
}
