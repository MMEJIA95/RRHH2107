using BE_GestionRRHH;
using BL_GestionRRHH;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;

namespace UI_GestionRRHH
{
    internal class ConfigSesion
    {
        public ConfigSesion() { }
        public ConfigSesion(string[] args, Form mainForm)
        {
            var acceso = ObtenerAcceso(args);
            if (acceso == null)
                System.Windows.Forms.Application.Exit();

            /*  Asignar valores a la clase Sesión.
             *  Variable Global, se carga en el Splash.
             */

            Program.Sesion = new AppSesion(acceso);
            Program.Sesion.SetOpenAccess(new Acceso(solucion: acceso.Solucion,
                   token: acceso.Token,
                   user: acceso.User,
                   key: acceso.Key,
                   entorno: acceso.Entorno)
                );

            Cifrado(acceso.Key);
            var unit = new UnitOfWork();
            unit.Globales.Actualizar_appSettings(new eGlobales() { Entorno = acceso.Entorno });
            Program.Sesion.SetColor(unit.Globales.ObtenerColores());
            /* Obtener el token de la sesión: corresponde a cada solución.*/
            var sistema = unit.Sistema.ListarSolucion<eSolucion>(opcion: 2, dsc_solucion: acceso.Solucion);
            /*Primera validación: La consulta debe retornar algún valor.*/
            if (sistema == null && sistema.Count <= 0) { System.Windows.Forms.Application.Exit(); return; }
            /* Segunda validación: Si el Token enviado desde el login está registrado en la DB.*/
            if (sistema[0].dsc_token_sesion.ToString().Trim().Equals(acceso.Token.Trim()))
            {
                /*  Cargar información del usuario*/
                Program.Sesion.SetUser(unit.Usuario.ObtenerUsuario<eUsuario>(opcion: 2, cod_usuario: acceso.User));
                /*  Si el usuario no tiene info; se cierra la APP*/
                if (Program.Sesion.Usuario == null) { System.Windows.Forms.Application.Exit(); }
                /*  Cargar las empresas asociadas al usuario*/
                Program.Sesion.SetEmpresaList(unit.Proveedores.ListarEmpresasProveedor<eProveedor_Empresas>(
                    opcion: 11, cod_proveedor: "", cod_usuario: acceso.User));
                /*  Cargar Variables Globales.*/
                Program.Sesion.SetGlobal(Asignar_VariablesGlobales(unit, sistema[0].dsc_solucion, sistema[0].cod_solucion, sistema[0].dsc_ruta_descarga));
                /*  Asignar versión registrada en la DB->corresponde a la solución.*/
                Program.Sesion.Version = sistema[0].dsc_version;

                /*  Iniciar el SplasScreen*/
                unit.Dispose();
                //Application.Run(new Form1());
                System.Windows.Forms.Application.Run(mainForm);
                //Application.Run(new frmSplashScreen(sistema[0].dsc_version));
            }
            else
                System.Windows.Forms.Application.Exit();
        }

        internal Acceso ObtenerAcceso(string[] args)
        {
            string values = "";
            args.ToList().ForEach(a => { values += a.ToString().Trim(); });

            // debería recibir 5 parámetros... (Agregar el valor si se requiere más parámetros).
            var array = values.Split(',');
            if (array.Count() < 5) return null;

            return new Acceso(solucion: array[0].ToString(), token: array[1].ToString(),
                user: array[2].ToString(), key: array[3].ToString(), entorno: array[4].ToString());
        }
        private void Cifrado(string key)
        {
            var blEncryp = new blEncrypta(key); // se puede declarar como  globar para llamar desde cualquier formulario...
            try { blEncryp.DesencryptaAppSettings(); } catch { }
            blEncryp.EncryptaAppSettings();
        }
        private eGlobales Asignar_VariablesGlobales(UnitOfWork unit, string dsc_solucion, string cod_solucion, string dsc_ruta_descarga)
        {
            var eGlobal = new eGlobales();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile.Replace(".vshost", "").Replace("Config", "config"));
            //xmlDoc.Load("C:\\SG5-Software\\kq_SG5_Controlador.exe.config");
            foreach (XmlElement element in xmlDoc.DocumentElement)
            {
                if (element.Name.Equals("appSettings"))
                {
                    foreach (XmlNode node in element.ChildNodes)
                    {
                        if (node.Attributes == null || node.Attributes.Count == 0) continue;
                        switch (unit.Encripta.Desencrypta(node.Attributes[0].Value))
                        {
                            case "conexion": eGlobal.Entorno = unit.Encripta.Desencrypta(node.Attributes[1].Value); break;
                            case "ServidorLOCAL": eGlobal.ServidorLOCAL = unit.Encripta.Desencrypta(node.Attributes[1].Value); break;
                            case "ServidorREMOTO": eGlobal.ServidorREMOTO = unit.Encripta.Desencrypta(node.Attributes[1].Value); break;
                            case "BBDD": eGlobal.BBDD = unit.Encripta.Desencrypta(node.Attributes[1].Value); break;
                            case "FormatoFecha": eGlobal.FormatoFecha = unit.Encripta.Desencrypta(node.Attributes[1].Value); break;
                            case "SeparadorListas": eGlobal.SeparadorListas = unit.Encripta.Desencrypta(node.Attributes[1].Value); break;
                            case "SeparadorDecimal": eGlobal.SeparadorDecimal = unit.Encripta.Desencrypta(node.Attributes[1].Value); break;
                            //case "UltimoLocalidad": eGlobal.UltimoLocalidad = blEncryp.Desencrypta(node.Attributes[1].Value); break;
                            case "UltimaEmpresa": eGlobal.UltimaEmpresa = unit.Encripta.Desencrypta(node.Attributes[1].Value); break;
                            case "UltimoUsuario": eGlobal.UltimoUsuario = unit.Encripta.Desencrypta(node.Attributes[1].Value); break;
                            case "VersionApp": eGlobal.VersionApp = unit.Encripta.Desencrypta(node.Attributes[1].Value); break;
                            case "UserID": eGlobal.UserSQL = unit.Encripta.Desencrypta(node.Attributes[1].Value); break;
                            case "Password": eGlobal.PasswordSQL = unit.Encripta.Desencrypta(node.Attributes[1].Value); break;
                            case "AppName": eGlobal.ApplicationName = unit.Encripta.Desencrypta(node.Attributes[1].Value); break;
                            case "RutaArchivosLocalExportar": eGlobal.RutaArchivosLocalExportar = unit.Encripta.Desencrypta(node.Attributes[1].Value); break;
                        }
                    }
                }
            }

            /* Obtener el token de la sesión: corresponde a cada solución.*/
            //Program.Sesion.Global = new eGlobales();

            eGlobal.Solucion = dsc_solucion;// Program.Sesion.Global.Solucion.dsc_solucion; //Program.Sesion.Global.Solucion;
            eGlobal.CodSolucion = cod_solucion;// Program.Sesion.Global.Solucion.cod_solucion;//. //Program.Sesion.Cod_solucion;
            eGlobal.Servidor = eGlobal.Entorno.Equals("LOCAL") ? eGlobal.ServidorLOCAL : eGlobal.ServidorREMOTO;
            eGlobal.RutaDescarga = dsc_ruta_descarga;
            //Program.Sesion.Global = eGlobal;
            //Program.Sesion.Global.Servidor = eGlobal.Entorno.Equals("LOCAL") ? eGlobal.ServidorLOCAL : eGlobal.ServidorREMOTO;

            return eGlobal;
        }


        #region Espacio para Soluciones
        public void CrearButtonModulos(List<eSolucionUsuario_Consulta> modulos, Control control)
        {
            control.Controls.Clear();
            var mods = modulos.Where(o => o.dsc_solucion != Program.Sesion.Global.Solucion).ToList();
            foreach (var mo in mods)
            {
                var icono = mo.dsc_icono;
                var btn = new SimpleButton()
                {
                    Cursor = Cursors.Hand,
                    PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light,
                    Dock = DockStyle.Top,
                    Font = new System.Drawing.Font("Tahoma", 11, FontStyle.Regular),
                    Size = new Size(260, 46),
                    Text = mo.dsc_texto,
                };
                btn.ImageOptions.Image = GetIcono(icono);
                btn.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                btn.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;

                btn.Click += (sender, args) =>
                AbrirModulos(mo.dsc_ruta_solucion, mo.dsc_solucion, mo.dsc_key_encrypted);
                control.Controls.Add(btn);
            }
            control.Height = (mods.Count() * 46) + 10;
        }

        public void CrearButtonModulos(List<eSolucionUsuario_Consulta> modulos, RibbonPageGroup control)
        {
            control.ItemLinks.Clear();
            var mods = modulos.Where(o => o.dsc_solucion != Program.Sesion.Global.Solucion).ToList();
            foreach (var mo in mods)
            {
                var icono = mo.dsc_icono;
                var btn = new BarButtonItem()
                {
                    Caption = mo.dsc_texto,
                    RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large,
                    //Font = new Font("Tahoma", 11, FontStyle.Regular),
                    //Size = new Size(260, 46),
                    //Text = mo.dsc_texto,
                    Enabled = mo.flg_acceso
                };
                btn.ImageOptions.Image = GetIcono(icono);
                //btn.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                //btn.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;

                btn.ItemClick += (sender, args) =>
                AbrirModulos(mo.dsc_ruta_solucion, mo.dsc_solucion, mo.dsc_key_encrypted);
                control.ItemLinks.Add(btn);
            }
            // control.Height = (mods.Count() * 46) + 10;
        }
        private Image GetIcono(string file)
        {
            var unit = new UnitOfWork();
            var pathFromApp = unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaIconoSolucion")]).ToString();
            if (!Directory.Exists(pathFromApp))
                Directory.CreateDirectory(pathFromApp);

            string path = $@"{pathFromApp}\{file}";
            if (!File.Exists(path))
                return null;

            return new Bitmap(path);
        }
        private string GetSolucionPath()
        {
            var unit = new UnitOfWork();
            var pathFromApp = unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaSoluciones")]).ToString();
            if (!Directory.Exists(pathFromApp))
                Directory.CreateDirectory(pathFromApp);

            return pathFromApp;
        }
        private void AbrirModulos(string solucionRuta, string dsc_solucion, string key) { EjecutarAplicacion(solucionRuta, dsc_solucion, key); }
        private void EjecutarAplicacion(string pathExe, string dsc_solucion, string key)
        {
            try
            {
                var ddd = Program.Sesion.Acceso.Key;
                var dddd = key;
                if (!RegistrarToken(dsc_solucion, out string token)) return;

                string path = $@"{GetSolucionPath()}\{pathExe}.exe";
                if (!File.Exists(path)) { throw (new Exception($"La solución {path}, no existe.")); }
                Process process = new Process();
                process.StartInfo.FileName = path;
                string parametro = $"{dsc_solucion},{token},{Program.Sesion.Usuario.cod_usuario},{key},{Program.Sesion.Global.Entorno}";
                /*-------*Guardar los datos de acceso de Solución*-------*/
                Program.Sesion.SetOpenAccess(new Acceso(solucion: dsc_solucion,
                    token: token,
                    user: Program.Sesion.Usuario.cod_usuario,
                    key: key,
                    entorno: Program.Sesion.Global.Entorno));


                //Se envía en ordern de 0 a más: 0:Solucion, 1:token_sesion, 2:cod_usuario,
                //process.StartInfo.Arguments = $"{dsc_solucion},{token},{Usuario.cod_usuario},{key}.{Global.Entorno}";
                Process.Start(path, parametro);
                //process.Start();



                //Application.Exit();
            }
            catch (Exception ex)
            {
                new UnitOfWork().Globales.Mensaje(false, $"{ex.Message}", "Abrir Solución");
            }
        }
        private bool RegistrarToken(string dsc_solucion, out string token)
        {
            token = new UnitOfWork().Globales.ObtenerTokenString();
            var result = new UnitOfWork().Sistema.ListarSolucion<eSqlMessage>
                (opcion: 3, dsc_token_sesion: token, cod_usuario: Program.Sesion.Usuario.cod_usuario,
                dsc_solucion: dsc_solucion);

            return result[0].IsSuccess;
        }
        #endregion
    }
}

