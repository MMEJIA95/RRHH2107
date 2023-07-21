using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_GestionRRHH
{
    public class eEmailingBoleta
    {
        public string cod_emailingBoletas { get; set; }
        public string cod_empresa { get; set; }
        public string dsc_emailingBoletas { get; set; }
        public string dsc_documento_trabajador { get; set; }
        public string dsc_email_trabajador { get; set; }
        public string dsc_archivo_enviado { get; set; }
        public string flg_enviado { get; set; }
        public DateTime fch_envio { get; set; }
        public string cod_usuario_enviado { get; set; }
    }
    public class eEmpresaEmail
    {
        public string cod_empresa { get; set; }
        public string dsc_empresa { get; set; }
        public string dsc_RazonSocialBoletas { get; set; }
        public string dsc_UsuarioEmailBoletas { get; set; }
        public string dsc_ClaveEmailBoletas { get; set; }
    }
    public class eEmailFormato
    {
        public string cod_emailingFormato { get; set; }
        public string cod_empresa { get; set; }
        public string cod_tipo_formato { get; set; }
        public string dsc_emailingFormato { get; set; }
        public string dsc_asunto { get; set; }
        public string dsc_cuerpo { get; set; }
        public string dsc_rutaAdjunto { get; set; }
        public string flg_estado { get; set; }
    }
    public class eTrabajadorMatch
    {
        public string dsc_documento { get; set; }
        public string dsc_nombres { get; set; }
        public string dsc_mail_1 { get; set; }
        public string dsc_mail_2 { get; set; }
        public string flg_resultado { get; set; }
        public string flg_existe { get; set; }
    }
    public class pEmailingBoleta : pConsultaBase
    {
        public pEmailingBoleta()
        {
            _cod_empresaSplit = string.Empty;
            _dsc_emailingBoletaSplit = string.Empty;
            _cod_usuario_enviado = string.Empty;
            _dsc_email_trabajadorSplit = string.Empty;
            _dsc_archivo_enviadoSplit = string.Empty; 
            _flg_enviadoSplit = string.Empty;
            _dsc_documento_trabajadorSplit = string.Empty;
            _cod_emailingFormato = string.Empty;
            _cod_tipo_formato = string.Empty;
        }

        private string _cod_empresaSplit;
        private string _dsc_emailingBoletaSplit;
        private string _cod_usuario_enviado;
        private string _dsc_email_trabajadorSplit;
        private string _dsc_archivo_enviadoSplit;
        private string _flg_enviadoSplit;
        private string _dsc_documento_trabajadorSplit;
        private string _cod_emailingFormato;
        private string _cod_tipo_formato;

        public string Cod_empresaSplit { get => _cod_empresaSplit; set => _cod_empresaSplit = value; }
        public string Dsc_emailingBoletaSplit { get => _dsc_emailingBoletaSplit; set => _dsc_emailingBoletaSplit = value; }
        public string Cod_usuario_enviado { get => _cod_usuario_enviado; set => _cod_usuario_enviado = value; }
        public string Dsc_email_trabajadorSplit { get => _dsc_email_trabajadorSplit; set => _dsc_email_trabajadorSplit = value; }
        public string Dsc_archivo_enviadoSplit { get => _dsc_archivo_enviadoSplit; set => _dsc_archivo_enviadoSplit = value; }
        public string Flg_enviadoSplit { get => _flg_enviadoSplit; set => _flg_enviadoSplit = value; }
        public string Dsc_documento_trabajadorSplit { get => _dsc_documento_trabajadorSplit; set => _dsc_documento_trabajadorSplit = value; }
        public string Cod_emailingFormato { get => _cod_emailingFormato; set => _cod_emailingFormato = value; }
        public string Cod_tipo_formato { get => _cod_tipo_formato; set => _cod_tipo_formato = value; }
    }
}
