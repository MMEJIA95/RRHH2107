using DevExpress.Utils.Drawing;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
//using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using DevExpress.Utils.Taskbar.Core;
using DevExpress.Utils.Taskbar;
using DevExpress.XtraTreeList;
using DevExpress.XtraNavBar;
using DevExpress.XtraTreeList.Nodes;
using System.Data;
using BE_GestionRRHH;
using DA_GestionRRHH;
using System.ComponentModel;
using DevExpress.XtraGrid;
using System.Reflection;
using System.Xml;
using System.IO;

namespace BL_GestionRRHH
{
    public class blGlobales
    {
        readonly blSistema blSis;
        private readonly blEncrypta blCrypt;
        private readonly eColor Colores;
        public blGlobales(daSQL sql, string key)
        {
            blCrypt = new blEncrypta(key);
            blSis = new blSistema(sql);
            Colores = ObtenerColores();
        }
        public eColor ObtenerColores() { return new eColor(GetColor("colorVerde"), GetColor("colorPlomo"), GetColor("colorEventRow"), GetColor("colorFocus")); }
        private Color GetColor(string argb)
        {

            var intArgb = blCrypt.Desencrypta(ConfigurationManager.AppSettings[blCrypt.Encrypta(argb)]).ToString().Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            if (intArgb == null) return new Color();
            return Color.FromArgb(intArgb[0], intArgb[1], intArgb[2]);
        }

        private Random random = new Random();
        public string ObtenerTokenString()
        {
            const string chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
            return new string(Enumerable.Repeat(chars, random.Next(33, 77))
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public string ObtenerTokenString(int start, int end)
        {
            const string chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
            return new string(Enumerable.Repeat(chars, random.Next(start, end))
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public string ObtenerKeyString(int start, int end)
        {
            const string chars = "0123456789abcdefghijklmnopqrstuwxyz0123456789";
            return new string(Enumerable.Repeat(chars, random.Next(start, end))
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public string GetAppVariableValor(string appVariable)
        {
            var path = blCrypt.Desencrypta(ConfigurationManager.AppSettings[blCrypt.Encrypta(appVariable)]).ToString();
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }

        public void pKeyDown(TextEdit sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                sender.Text = "";
            }
        }

        public string pKeyPress(TextEdit sender, KeyPressEventArgs e)
        {
            string sAux = "";
            if ((e.KeyChar >= 65 && e.KeyChar <= 90) || (e.KeyChar >= 97 && e.KeyChar <= 122) || (e.KeyChar >= 48 && e.KeyChar <= 57) || (e.KeyChar == 45 || e.KeyChar == 32))
            {
                sAux = e.KeyChar.ToString();
                if (e.KeyChar == 45 || e.KeyChar == 32) { sAux = ""; }
                e.Handled = true;
            }
            return sAux;
        }

        public void keyPressOnlyNumber(KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
                e.Handled = false;
            else if (char.IsControl(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = true;
        }

        public void Pintar_CabeceraColumnas(ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column == null) return;
            System.Drawing.Rectangle rect = e.Bounds;
            rect.Inflate(-1, -1);
            e.Graphics.FillRectangle(new SolidBrush(Colores.Verde), rect); 
            //e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(210, 210, 210)), rect); 
            e.Appearance.DrawString(e.Cache, e.Info.Caption, e.Info.CaptionRect);
            foreach (DrawElementInfo info in e.Info.InnerElements)
            {
                if (!info.Visible) continue;
                ObjectPainter.DrawObject(e.Cache, info.ElementPainter, info.ElementInfo);
            }
            e.Handled = true;
        }

        public void Pintar_CabeceraColumnasBandHeader(BandHeaderCustomDrawEventArgs e)
        {
            if (e.Band == null) return;
            System.Drawing.Rectangle rect = e.Bounds;
            rect.Inflate(-1, -1);
            e.Graphics.FillRectangle(new SolidBrush(Colores.Verde), rect);
            e.Appearance.DrawString(e.Cache, e.Info.Caption, e.Info.CaptionRect);
            foreach (DrawElementInfo info in e.Info.InnerElements)
            {
                if (!info.Visible) continue;
                ObjectPainter.DrawObject(e.Cache, info.ElementPainter, info.ElementInfo);
            }
            e.Handled = true;
        }

        public void Pintar_EstiloGrilla(object sender, RowStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (view.Columns["flg_activo"] != null)
            {
                string estado = view.GetRowCellDisplayText(e.RowHandle, view.Columns["flg_activo"]);
                if (estado == "NO") e.Appearance.ForeColor = Color.Red;
            }
            view.Appearance.EvenRow.BackColor = Colores.EventRow;
            view.Appearance.FocusedRow.BackColor = Colores.Focus;
            view.Appearance.FocusedRow.FontStyleDelta = FontStyle.Bold; view.Appearance.FocusedRow.ForeColor = Color.Black;
            view.Appearance.FocusedCell.BackColor = Colores.Focus;
            view.Appearance.FocusedCell.FontStyleDelta = FontStyle.Bold; view.Appearance.FocusedCell.ForeColor = Color.Black;
            view.Appearance.HideSelectionRow.BackColor = Colores.Focus;
            view.Appearance.HideSelectionRow.FontStyleDelta = FontStyle.Bold; view.Appearance.HideSelectionRow.ForeColor = Color.Black;
            view.Appearance.SelectedRow.BackColor = Colores.Focus;
            view.Appearance.SelectedRow.FontStyleDelta = FontStyle.Bold; view.Appearance.SelectedRow.ForeColor = Color.Black;
        }


        public void ConfigurarGridView_TreeStyle(GridControl gc, GridView gv, bool editable = false, bool showGroupPanel = true)
        {
            gv.OptionsView.ShowGroupPanel = showGroupPanel;
            gv.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            gv.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            gv.OptionsView.EnableAppearanceEvenRow = true;
            gv.OptionsView.ShowAutoFilterRow = false;
            gv.OptionsSelection.ShowCheckBoxSelectorInGroupRow = DevExpress.Utils.DefaultBoolean.True;
            gv.OptionsSelection.MultiSelect = true;
            gv.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            gv.OptionsView.ShowColumnHeaders = false;
            //
            gv.OptionsFind.AllowFindPanel = true;
            gv.OptionsFind.AlwaysVisible = true;
            gv.OptionsBehavior.Editable = editable;
            //dgvTrabajador.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            gv.ColumnPanelRowHeight = 35;
            gv.Appearance.HeaderPanel.ForeColor = Color.White;
            gv.Appearance.HeaderPanel.Options.UseForeColor = true;
            gv.Appearance.HeaderPanel.Options.UseTextOptions = true;
            //gv.Appearance.GroupRow.BackColor = Color.White;
            //
            gc.UseEmbeddedNavigator = true;
            gc.EmbeddedNavigator.Buttons.Remove.Visible = false;
            gc.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            gc.EmbeddedNavigator.Buttons.Edit.Visible = false;
            gc.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            gc.EmbeddedNavigator.Buttons.Append.Visible = false;

            gv.ExpandAllGroups();
        }
        public void ConfigurarGridView_ClasicStyle(GridControl gc, GridView gv, bool editable = false, bool showGroupPanel = false, bool allowFindPanel = false, bool showAutoFilterRow = true)
        {
            gv.OptionsView.ShowGroupPanel = showGroupPanel;
            //gv.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            //gv.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            gv.OptionsView.EnableAppearanceEvenRow = true;
            gv.OptionsView.ShowAutoFilterRow = showAutoFilterRow;
            //gv.OptionsSelection.ShowCheckBoxSelectorInGroupRow = DevExpress.Utils.DefaultBoolean.True;
            gv.OptionsSelection.MultiSelect = true; ;
            //gv.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            gv.OptionsView.ShowColumnHeaders = true;
            //
            gv.OptionsFind.AllowFindPanel = allowFindPanel;
            gv.OptionsFind.AlwaysVisible = allowFindPanel;
            gv.OptionsBehavior.Editable = editable;
            //dgvTrabajador.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            gv.ColumnPanelRowHeight = 35;
            gv.Appearance.HeaderPanel.ForeColor = Color.White;
            gv.Appearance.HeaderPanel.Options.UseForeColor = true;
            gv.Appearance.HeaderPanel.Options.UseTextOptions = true;
            //gv.Appearance.GroupRow.BackColor = Color.White;
            //
            gc.UseEmbeddedNavigator = true;
            gc.EmbeddedNavigator.Buttons.Remove.Visible = false;
            gc.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            gc.EmbeddedNavigator.Buttons.Edit.Visible = false;
            gc.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            gc.EmbeddedNavigator.Buttons.Append.Visible = false;

            gv.ExpandAllGroups();

            gv.RowStyle += Gv_RowStyle;
            gv.CustomDrawColumnHeader += Gv_CustomDrawColumnHeader;
        }
        private void Gv_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e) { Pintar_CabeceraColumnas(e); }
        private void Gv_RowStyle(object sender, RowStyleEventArgs e) { Pintar_EstiloGrilla(sender, e); }

        public void Abrir_SplashScreenManager(Type splashFormType, string sTitulo, string sSubTitulo = "Cargando...")
        {
            foreach (Form splash in System.Windows.Forms.Application.OpenForms)
            {
                if (splash.Name.Equals("FrmSplashCarga"))
                {
                    //splash.Close();
                    SplashScreenManager.CloseForm();
                    break;
                }
            }



            SplashScreenManager.ShowForm(splashFormType);
            string[] oDatos = { sTitulo, sSubTitulo };
            SplashScreenManager.Default.SendCommand(SkinSplashScreenCommand.UpdateLoadingText, oDatos);
        }

        //public Boolean EnviarCorreoElectronico_Outlook(string mailDirection, string mailSubject, string mailContent)
        //{
        //    try
        //    {
        //        var oApp = new Microsoft.Office.Interop.Outlook.Application();
        //        Microsoft.Office.Interop.Outlook.NameSpace ns = oApp.GetNamespace("MAPI");
        //        var f = ns.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderInbox);
        //        System.Threading.Thread.Sleep(1000);

        //        var mailItem = (Microsoft.Office.Interop.Outlook.MailItem)oApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
        //        mailItem.Subject = mailSubject;
        //        mailItem.HTMLBody = mailContent;
        //        mailItem.To = mailDirection;
        //        mailItem.Send();
        //        MessageBox.Show("El correo fue enviado, revise su bandeja de entrada.", "Recuperación de clave", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        MessageBox.Show("El correo no pudo ser enviado.", "Recuperación de clave", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        return false;
        //    }
        //    finally
        //    {
        //    }
        //    return true;
        //}

        public Boolean EnviarCorreoElectronico_SMTP(string sDestinatario, string sAsunto,
            string sCuerpo, string RutasAdjunto = "")
        {
            List<BE_GestionRRHH.eSistema> eSist = blSis.Obtener_ParamterosSistema<BE_GestionRRHH.eSistema>(11);
            String sCredencialUsuario = "", sCredencialClave = "";
            if (eSist.Count > 0)
            {
                sCredencialUsuario = eSist[0].dsc_clave;
                sCredencialClave = eSist[0].dsc_valor;
            }

            System.Net.Mail.MailMessage correo = new System.Net.Mail.MailMessage();
            correo.To.Add(new System.Net.Mail.MailAddress(sDestinatario.Trim()));
            correo.From = new System.Net.Mail.MailAddress(sCredencialUsuario);
            correo.Subject = sAsunto;
            correo.Body = sCuerpo;
            correo.IsBodyHtml = false;

            using (var client = new System.Net.Mail.SmtpClient("smtp.office365.com", 587))
            {
                client.Credentials = new NetworkCredential(sCredencialUsuario, sCredencialClave);
                client.EnableSsl = true;
                try
                {
                    client.Send(correo);
                    MessageBox.Show("El correo fue enviado, revise su bandeja de entrada.", "Recuperación de clave", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"El correo no pudo ser enviado.\n{ex.Message}", "Recuperación de clave", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            return true;
        }

        public Boolean EnviarCorreoElectronico_SMTP(string sDestinatario, string sAsunto,
            string sCuerpo, List<BE_GestionRRHH.eSistema> credenciales,
            string RutasAdjunto = "")
        {

            String sCredencialUsuario = "", sCredencialClave = "";
            if (credenciales.Count > 0)
            {
                sCredencialUsuario = credenciales[0].dsc_clave;
                sCredencialClave = credenciales[0].dsc_valor;
            }

            System.Net.Mail.MailMessage correo = new System.Net.Mail.MailMessage();
            correo.To.Add(new System.Net.Mail.MailAddress(sDestinatario.Trim()));
            correo.From = new System.Net.Mail.MailAddress(sCredencialUsuario);
            correo.Subject = sAsunto;
            correo.Body = sCuerpo;
            correo.IsBodyHtml = false;

            //correo.Attachments ;
            string attachmentsPath = RutasAdjunto;// @"C:\Himasagar";
            if (Directory.Exists(attachmentsPath))
            {
                string[] files = Directory.GetFiles(attachmentsPath);
                foreach (string fileName in files)
                {
                    Attachment file = new Attachment(fileName);
                    correo.Attachments.Add(file);
                }
            }

            using (var client = new System.Net.Mail.SmtpClient("smtp.office365.com", 587))
            {
                client.Credentials = new NetworkCredential(sCredencialUsuario, sCredencialClave);
                client.EnableSsl = true;
                try
                {
                    client.Send(correo);
                    MessageBox.Show("El correo fue enviado, revise su bandeja de entrada.", "Recuperación de clave", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"El correo no pudo ser enviado.\n{ex.Message}", "Recuperación de clave", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            return true;
        }
        public Boolean EnviarCorreoElectronico_SMTP(string sDestinatario, string sAsunto,
            string sCuerpo, List<BE_GestionRRHH.eSistema> credenciales,
            string RutasAdjunto = "", string ArchivoAdjunto = "")
        {

            String sCredencialUsuario = "", sCredencialClave = "";
            if (credenciales.Count > 0)
            {
                sCredencialUsuario = credenciales[0].dsc_clave;
                sCredencialClave = credenciales[0].dsc_valor;
            }

            System.Net.Mail.MailMessage correo = new System.Net.Mail.MailMessage();
            correo.To.Add(new System.Net.Mail.MailAddress(sDestinatario.Trim()));
            correo.From = new System.Net.Mail.MailAddress(sCredencialUsuario);
            correo.Subject = sAsunto;
            correo.Body = sCuerpo;
            correo.IsBodyHtml = false;

            //correo.Attachments ;
            string attachmentsPath = RutasAdjunto;// @"C:\Himasagar";
            if (Directory.Exists(attachmentsPath))
            {
                string[] files = Directory.GetFiles(attachmentsPath);
                foreach (string fileName in files)
                {
                    if (fileName.Trim().Equals(ArchivoAdjunto.Trim()))
                    {
                        Attachment file = new Attachment(fileName);
                        correo.Attachments.Add(file);
                        break;
                    }
                }
            }

            using (var client = new System.Net.Mail.SmtpClient("smtp.office365.com", 587))
            {
                client.Credentials = new NetworkCredential(sCredencialUsuario, sCredencialClave);
                client.EnableSsl = true;
                try
                {
                    client.Send(correo);
                    //MessageBox.Show("El correo fue enviado, revise su bandeja de entrada.", "Recuperación de clave", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                catch (Exception)// ex)
                {
                    //  MessageBox.Show($"El correo no pudo ser enviado.\n{ex.Message}", "Recuperación de clave", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            //return true;
        }

        public Boolean EnviarCorreoElectronico_SMTP(string sDestinatario, string sCopia, string sAsunto,
            string sCuerpo, List<BE_GestionRRHH.eSistema> credenciales,
            string RutasAdjunto = "", string[] ArchivosAdjunto = null)
        {

            String sCredencialUsuario = "", sCredencialClave = "";
            if (credenciales.Count > 0)
            {
                sCredencialUsuario = credenciales[0].dsc_clave;
                sCredencialClave = credenciales[0].dsc_valor;
            }

            System.Net.Mail.MailMessage correo = new System.Net.Mail.MailMessage();
            if (sDestinatario.Length > 0)
            {
                if (sDestinatario.Contains(","))
                {
                    string[] des = sDestinatario.Split(',');
                    foreach (string d in des)
                    {
                        correo.To.Add(new System.Net.Mail.MailAddress(d));
                    }
                }
                else
                {
                    correo.To.Add(new System.Net.Mail.MailAddress(sDestinatario.Trim()));
                }
            }
           
            correo.From = new System.Net.Mail.MailAddress(sCredencialUsuario);

            if (sCopia.Length > 0)
            {
                if (sCopia.Contains(","))
                {
                    string[] cc = sCopia.Split(',');
                    foreach (string c in cc)
                    {
                        correo.CC.Add(new System.Net.Mail.MailAddress(c));
                    }
                }
                else
                {
                    correo.CC.Add(new System.Net.Mail.MailAddress(sCopia));
                }
            }



            correo.Subject = sAsunto;
            correo.Body = sCuerpo;
            correo.IsBodyHtml = false;

            //correo.Attachments ;
            string attachmentsPath = RutasAdjunto;// @"C:\Himasagar";
            if (Directory.Exists(attachmentsPath))
            {
                string[] files = Directory.GetFiles(attachmentsPath);

                foreach (string adjuntar in ArchivosAdjunto)
                {
                    foreach (string fileName in files)
                    {
                        var fname = Path.GetFileName(fileName);
                        if (fname.Trim().Equals(adjuntar))
                        {
                            Attachment file = new Attachment(fileName);
                            correo.Attachments.Add(file);
                            break;
                        }
                    }

                }
            }

            using (var client = new System.Net.Mail.SmtpClient("smtp.office365.com", 587))
            {
                client.Credentials = new NetworkCredential(sCredencialUsuario, sCredencialClave);
                client.EnableSsl = true;
                try
                {
                    client.Send(correo);
                    //MessageBox.Show("El correo fue enviado, revise su bandeja de entrada.", "Recuperación de clave", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                catch (Exception)// ex)
                {
                    //  MessageBox.Show($"El correo no pudo ser enviado.\n{ex.Message}", "Recuperación de clave", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            //return true;
        }
        public void Actualizar_appSettings(eGlobales globales)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile.Replace(".vshost", ""));
            foreach (XmlElement element in xmlDoc.DocumentElement)
            {
                if (element.Name.Equals("appSettings"))
                {
                    foreach (XmlNode node in element.ChildNodes)
                    {
                        if (node.Attributes == null || node.Attributes.Count == 0) continue;
                        switch (blCrypt.Desencrypta(node.Attributes[0].Value))
                        {
                            case "conexion": node.Attributes[1].Value = blCrypt.Encrypta(globales.Entorno); break;
                                //case "ServidorLOCAL": node.Attributes[1].Value = unit.Encripta.Encrypta(txtServidorLOCAL.Text); break;
                                //case "ServidorREMOTO": node.Attributes[1].Value = unit.Encripta.Encrypta(txtServidorREMOTO.Text); break;
                                //case "BBDD": node.Attributes[1].Value = unit.Encripta.Encrypta(txtBaseDatos.Text); break;
                                //case "FormatoFecha": node.Attributes[1].Value = unit.Encripta.Encrypta(grdbFormatoFecha.SelectedIndex == 0 ? "DD-MM-YYYY" : "YYYY-MM-DD"); break;
                                //case "UltimaEmpresa": node.Attributes[1].Value = blCrypt.Encrypta(lkpEmpresaInicio.EditValue.ToString()); break;
                                //case "SeparadorListas": node.Attributes[1].Value = unit.Encripta.Encrypta(CultureInfo.CurrentCulture.TextInfo.ListSeparator); break;
                        }
                    }
                }
            }
            xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile.Replace(".vshost", ""));
            ConfigurationManager.RefreshSection("appSettings");
        }

        public enum TipoMensaje
        {
            OK = 0,
            Error = 1,
            Alerta = 2,
            YesNo = 3,
        }
        public DialogResult Mensaje(TipoMensaje tipo, string mensaje = "", string titulo = "")
        {

            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icon = MessageBoxIcon.Information;
            switch (tipo)
            {
                case TipoMensaje.OK:
                    break;
                case TipoMensaje.Error:
                    {
                        icon = MessageBoxIcon.Error;
                        break;
                    }
                case TipoMensaje.Alerta:
                    {
                        icon = MessageBoxIcon.Warning;
                        break;
                    }
                case TipoMensaje.YesNo:
                    {
                        buttons = MessageBoxButtons.YesNo;
                        icon = MessageBoxIcon.Question;
                        break;
                    }
            }

            return XtraMessageBox.Show(mensaje, titulo, buttons, icon);
        }
        public DialogResult Mensaje(bool errorSuccess, string mensaje = "", string titulo = "")
        {

            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icon = MessageBoxIcon.Information;
            switch (errorSuccess)
            {
                case false:
                    {
                        icon = MessageBoxIcon.Error;
                        break;
                    }
            }

            return XtraMessageBox.Show(mensaje, titulo, buttons, icon);
        }

        public PropertyInfo[] GetPropertyInfoArray<T>()
        {
            PropertyInfo[] props = null;
            try
            {
                Type type = typeof(T);
                object obj = Activator.CreateInstance(type);
                props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            }
            catch (Exception)
            { }
            return props;
        }
    }
}
