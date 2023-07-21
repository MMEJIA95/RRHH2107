using BE_GestionRRHH;
using BL_GestionRRHH;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace UI_GestionRRHH
{
    static class Program
    {
        internal static AppSesion Sesion;
        /// <summary>
        /// Punto de entrada principal para la aplicación..
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            /*  Comentar cuando pase a producción
             *  TU_SOLUCION: Colocar el nombre de la solucion; BACKOFFICE,SERVICIOS,PRODUCCION,LOTES,RRHH
             *  HNG: Es el Token de sesión; si no entra con sultar la tabla "scfma_solucion".
             *  ADMINISTRADOR: Usuario.
             *  Versión: 2022.0.7, es la versión de la APP, se saca del App.config
             */
            string __token = "G2wZdSXrV4Ymln33vm82cIWDKBPXYbyedg0Qxy1RecbBcfrZu2HZYtQ6FTPc"; //buscar de la DB
            string __key = "mee5z7pu7663b1h7a19xxw47ccloh70w"; //buscar de la DB
            if (new ConfigSesion().ObtenerAcceso(args) == null)
            {
                var __solucion = "RRHH";
                var __usuario = "ADMINISTRADOR";
                var __entorno = "LOCAL";
                args = new string[] { $"{__solucion},", $"{__token},", $"{__usuario},", $"{__key}", $",{__entorno}" };
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(DevExpress.LookAndFeel.Basic.DefaultSkin.PineLight);
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(DevExpress.LookAndFeel.SkinStyle.Basic);

            //Abrir Solución...
            new ConfigSesion(args, new frmSplashScreen());
        }
    }
}
