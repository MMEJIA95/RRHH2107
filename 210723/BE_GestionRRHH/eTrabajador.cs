using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_GestionRRHH
{
    public class eTrabajador
    {
        public string cod_trabajador { get; set; }

        public int ctd_digitos { get; set; }

        public string cod_tregistro { get; set; }
        public string cod_usuario { get; set; }
        public string cod_empresa { get; set; }
        public string cod_sede_empresa { get; set; }
        public string dsc_sede_empresa { get; set; }
        public string dsc_empresa { get; set; }
        public string cod_fotocheck { get; set; }
        public string dsc_apellido_paterno { get; set; }
        public string dsc_apellido_materno { get; set; }
        public string dsc_nombres { get; set; }
        public string dsc_nombres_completos { get; set; }
        public string dsc_tipoContrato { get; set; }
        public string cod_area { get; set; }
        public string dsc_area { get; set; }
        public string cod_cargo { get; set; }
        public string dsc_cargo { get; set; }
        public string flg_sexo { get; set; }
        public string cod_estadocivil { get; set; }
        public string dsc_estadocivil { get; set; }
        public DateTime fch_nacimiento { get; set; }
        public string flg_vivo { get; set; }
        public string cod_tipo_documento { get; set; }
        public string dsc_tipo_documento { get; set; }
        public string dsc_documento { get; set; }
        public DateTime fch_vcto_documento { get; set; }
        public string nro_ubigeo_documento { get; set; }
        public string cod_banco { get; set; }
        public string dsc_direccion { get; set; }
        public string cod_distrito { get; set; }
        public string dsc_distrito { get; set; }
        public string cod_provincia { get; set; }
        public string dsc_provincia { get; set; }
        public string cod_departamento { get; set; }
        public string dsc_departamento { get; set; }
        public string cod_sunat { get; set; }
        public string cod_pais { get; set; }
        public string dsc_pais { get; set; }
        public string dsc_referencia { get; set; }
        public string cod_nacionalidad { get; set; }
        public string dsc_nacionalidad { get; set; }
        public string dsc_mail_1 { get; set; }
        public string dsc_mail_2 { get; set; }
        public string dsc_telefono { get; set; }
        public string dsc_celular { get; set; }
        public string cod_sist_pension { get; set; }
        public string dsc_sist_pension { get; set; }
        public string cod_APF { get; set; }
        public string dsc_APF { get; set; }
        public string cod_CUSPP { get; set; }
        public string flg_DNI { get; set; }
        public string flg_CV { get; set; }
        public string flg_AntPolicial { get; set; }
        public string flg_AntPenal { get; set; }
        public string flg_VerifDomiciliaria { get; set; }
        public string cod_TipoPersonal { get; set; }
        public DateTime fch_registro { get; set; }
        public string cod_usuario_registro { get; set; }
        public string dsc_usuario_registro { get; set; }
        public DateTime fch_cambio { get; set; }
        public string cod_usuario_cambio { get; set; }
        public string dsc_usuario_cambio { get; set; }
        public string flg_activo { get; set; }
        public int seleccionado { get; set; }

        public string idCarpeta_Trabajador { get; set; }
        public string id_DNI { get; set; }
        public string id_CV { get; set; }
        public string id_AntPolicial { get; set; }
        public string id_AntPenal { get; set; }
        public string id_VerifDomiciliaria { get; set; }

        public int dsc_edad { get; set; }
        public int dsc_anhos_ingreso { get; set; }
        public int dsc_meses_ingreso { get; set; }
        public int dsc_diasvencimiento { get; set; }

        public DateTime fch_ingreso { get; set; }
        public DateTime fch_vencimiento { get; set; }


        public string dsc_banco { get; set; }
        public string dsc_moneda { get; set; }
        public string dsc_tipo_cuenta { get; set; }
        public string dsc_cta_bancaria { get; set; }
        public string dsc_cta_interbancaria { get; set; }

        public string dsc_banco_CTS { get; set; }
        public string dsc_moneda_CTS { get; set; }
        public string dsc_cta_bancaria_CTS { get; set; }
        public string dsc_cta_interbancaria_CTS { get; set; }
        public int ctd_hijos_menores { get; set; }
        public string dsc_pref_ceco { get; set; }
        public DateTime fch_entrega_uniforme { get; set; }
        public DateTime fch_renovacion_uniforme { get; set; }
        public string dsc_nivel_educativo { get; set; }

        public string cod_ld { get; set; }
        public string cod_tipo_via { get; set; }
        public string dsc_tipo_via { get; set; }
        public string dsc_departamento_dir { get; set; }
        public string dsc_interior { get; set; }
        public string cod_tipozona { get; set; }


        public string dsc_nro_vivienda { get; set; }
        public string dsc_manzana { get; set; }
        public string cod_lote { get; set; }
        public string dsc_km { get; set; }
        public string dsc_block { get; set; }
        public string dsc_etapa { get; set; }
        public string dsc_tipo_zona { get; set; }
        public string dsc_nombre_zona { get; set; }
        public string dsc_ubigeo { get; set; }
        public string cod_centroasistencial { get; set; }

        public string cod_nivel_educativo { get; set; }
        public string FORMATOS { get; set; }
        //FORMATOE5
        public string FORMATOE5 { get; set; }

        public string flg_regimen_atipico { get; set; }
        public string flg_jornada_maxima { get; set; }
        public string flg_horario_nocturno { get; set; }
        public string flg_sindicato { get; set; }
        public string cod_situacion_trabajador_2 { get; set; }
        public string cod_exoneracion_5ta { get; set; }

        public string cod_parametro { get; set; }
        public string dsc_parametro { get; set; }
        public DateTime Fechabaja { get; set; }
        public string motivo_baja { get; set; }
        public string observaciones { get; set; }
        public DateTime fch_firma { get; set; }
        public string cant_hijos { get; set; }
        public string dsc_gruposanguineo { get; set; }

        public string cod_talla_polo { get; set; }
        public string cod_talla_camisa { get; set; }
        public string cod_talla_pantalon { get; set; }
        public string cod_talla_mameluco { get; set; }
        public string cod_talla_casaca { get; set; }
        public string cod_talla_chaleco { get; set; }
        public string cod_talla_calzado { get; set; }
        public string cod_talla_casco { get; set; }
        public string cod_talla_faja { get; set; }
        public string cod_sexo { get; set; }
        public string dsc_sexo { get; set; }
        public string cod_tipo_contrato { get; set; }
        public string dsc_tipo_contrato { get; set; }
        public string cod_moneda_CTS { get; set; }
        public decimal dsc_estatura { get; set; }
        public decimal dsc_peso { get; set; }
        public string flg_lentes { get; set; }
        public string cod_moneda { get; set; }

        public bool flg_escritura { get; set; }
        public string cod_tipoContrato { get; set; }
        public int ctd_meses { get; set; }

        public string campos { get; set; }
        public bool bloqueo { get; set; }
        public string dsc_cumpleanhos
        {
            get
            {
                return fch_nacimiento.ToString("dd/MM");
            }
            set
            {
                dsc_cumpleanhos = value;
            }
        }
        public string dsc_ruc { get; set; }
        public string CODIGO { get; set; }

        public string estado { get; set; }
        public string dsc_estado_civil { get; set; }
        public string cod_estado_civil { get; set; }

        public string cod_tipo_zona { get; set; }
        public DateTime fch_evaluacion { get; set; }
        public DateTime fch_evaluacion_obs { get; set; }

        public string dsc_observacion { get; set; }
        public string flg_observacion { get; set; }
        public string dsc_sino { get; set; }
        public string cod_sino { get; set; }
        public int cod_EMO { get; set; }
        public decimal rem_minimautil { get; set; }
        public decimal pcj_remuneracion { get; set; }
        public decimal total_rem { get; set; }
        public int cantidad_emo { get; set; }

        public string cod_CECO { get; set; }

        public string dsc_CECO { get; set; }
        public string tiempo_ingreso { get; set; }
        public string tiempo_contrato { get; set; }
        public string tiempo_actual { get; set; }
        public string dsc_kilometro_dir { get; set; }
        public string cod_reniec { get; set; }
        public string motivointerno { get; set; }
        public string blacklist { get; set; }
        public string estado_edad { get; set; }
        public class eArea : eTrabajador
        {
            public string cod_empresa { get; set; }
            public string dsc_empresa { get; set; }
            public string cod_sede_empresa { get; set; }
            public string dsc_sede_empresa { get; set; }
            public string cod_area { get; set; }
            public string dsc_area { get; set; }
            public string flg_activo { get; set; }
            public string registros { get; set; }
            public string remplazo { get; set; }


        }

        public class eCargos : eTrabajador
        {
            public string cod_empresa { get; set; }
            public string dsc_empresa { get; set; }
            public string cod_sede_empresa { get; set; }
            public string dsc_sede_empresa { get; set; }
            public string cod_area { get; set; }
            public string dsc_area { get; set; }
            public string cod_cargo { get; set; }
            public string dsc_cargo { get; set; }

            public string flg_activo { get; set; }

            public string flg_firma_documento { get; set; }
            public string registros { get; set; }
            public string remplazo { get; set; }


        }
        public class eContactoEmergencia_Trabajador : eTrabajador
        {
            public Int32 cod_contactemerg { get; set; }
            public string cod_parentesco { get; set; }

            public string dsc_parentesco { get; set; }
        }

        public class eInfoLaboral_Trabajador : eTrabajador
        {
            public Int32 cod_infolab { get; set; }
            public DateTime fch_ingreso { get; set; }
            public string cod_area { get; set; }
            public string dsc_area { get; set; }
            public string cod_cargo { get; set; }
            public string dsc_cargo { get; set; }
            public string dsc_pref_ceco { get; set; }

            public string cod_modalidad { get; set; }
            public string dsc_modalidad { get; set; }
            public string cod_moneda { get; set; }
            public string dsc_moneda { get; set; }
            public decimal imp_sueldo_base { get; set; }
            public decimal imp_asig_familiar { get; set; }
            public decimal imp_movilidad { get; set; }
            public decimal imp_alimentacion { get; set; }
            public decimal imp_escolaridad { get; set; }
            public decimal imp_bono { get; set; }
            public string cod_sede_empresa { get; set; }
            public string dsc_sede_empresa { get; set; }
            public string flg_Regimen_pension { get; set; }
            public string cod_ocupacional { get; set; }
            public string cod_conveniotributacion { get; set; }
            public string flg_horas_extras { get; set; }
            public string dsc_porcentajecomision { get; set; }
            public string dsc_porcentajequincena { get; set; }
            public string dsc_calificacion_puesto { get; set; }
            public string cod_categoria_trabajador { get; set; }
            public string cod_tipo_trabajador { get; set; }
            public DateTime fch_inicio_contrato { get; set; }
            public DateTime fch_fin_contrato { get; set; }
            public DateTime fch_puesto { get; set; }
            
            public string dsc_file_number { get; set; }
            public string flg_AfectoVidaLey { get; set; }
            public string flg_afectoSCTR { get; set; }
            public string dsc_cia_segurovida { get; set; }
            public string cod_regimen_laboral { get; set; }
            public string dsc_sctr_pension { get; set; }

            
            public string codsunat_scrtcentroriesgo { get; set; }
            public string codsunat_seguroley { get; set; }
            public string cod_situacion_especial { get; set; }
            public string flg_asignacionfamiliar { get; set; }
            public string dsc_tipoContrato { get; set; }

            public decimal dsc_estatura { get; set; }
            public decimal dsc_peso { get; set; }
            public string flg_lentes { get; set; }

            public string id_contrato { get; set; }
            public string flg_contrato { get; set; }
            public string correo_laboral { get; set; }
            public string cod_ocupaciones { get; set; }
            public string dsc_ocupacion { get; set; }

            public string comision_AFP { get; set; }
            

        }

      
       

        public class eTallaUniforme_Trabajador : eTrabajador
        {
            //public int cod_tallauniforme { get; set; }
          
            public int dsc_casillero { get; set; }
            public string flg_lentes { get; set; }
        }

        public class eInfoFamiliar_Trabajador : eTrabajador
        {
            public int cod_infofamiliar { get; set; }
            public string cod_parentesco { get; set; }
            public string dsc_parentesco { get; set; }
            public string dsc_mail { get; set; }
            public string dsc_profesion { get; set; }
            public string dsc_centrolaboral { get; set; }
            public string dsc_gradoinstruccion { get; set; }
            public string dsc_ocupacion { get; set; }
            public string dsc_discapacidad { get; set; }
            public string flg_enfermedad { get; set; }
            public string dsc_enfermedad { get; set; }
            public string flg_adiccion { get; set; }
            public string dsc_adiccion { get; set; }
            public string flg_dependenciaeconomica { get; set; }

            public string flg_DNI_documentofam { get; set; }
            public string id_DNI_documentofam { get; set; }
            public string flg_CERTIFICADOESTUDIOS_documentofam { get; set; }
            public string id_CERTIFICADOESTUDIOS_documentofam { get; set; }

            public string dsc_nombrescompletos_hijo { get; set; }
            public string dsc_notificación { get; set; }

        }

        public class eDocumentosFamiliar : eTrabajador
        {
            public string cod_trabajador { get; set; }
            public string cod_infofamiliar { get; set; }
            public string cod_documentofam { get; set; }
            public string dsc_parametro { get; set; }
            public string flg_documento { get; set; }
            public string id_documento { get; set; }
        }

        public class eInfoEconomica_Trabajador : eTrabajador
        {
            public int cod_infoeconomica { get; set; }
            public decimal imp_ingresomensual { get; set; }
            public decimal imp_gastomensual { get; set; }
            public decimal imp_totalactivos { get; set; }
            public decimal imp_totaldeudas { get; set; }
            public string cod_vivienda { get; set; }
            public string cod_tipovivienda { get; set; }
            public string imp_valorvivienda { get; set; }
            public string cod_monedavivienda { get; set; }
            public string dsc_direccion_vivienda { get; set; }
            public string cod_distrito_vivienda { get; set; }
            public string cod_provincia_vivienda { get; set; }
            public string cod_departamento_vivienda { get; set; }
            public string cod_pais_vivienda { get; set; }
            public string dsc_referencia_vivienda { get; set; }
            public string cod_tipovehiculo { get; set; }
            public string dsc_marcavehiculo { get; set; }
            public string dsc_modelovehiculo { get; set; }
            public string dsc_placavehiculo { get; set; }
            public string imp_valorvehiculo { get; set; }
            public string cod_monedavehiculo { get; set; }
            public string cod_tipoempresa { get; set; }
            public string dsc_participacion_empresa { get; set; }
            public string dsc_RUC_empresa { get; set; }
            public string dsc_giro_empresa { get; set; }
            public string dsc_direccion_empresa { get; set; }
            public string cod_distrito_empresa { get; set; }
            public string cod_provincia_empresa { get; set; }
            public string cod_departamento_empresa { get; set; }
            public string cod_pais_empresa { get; set; }
            public string dsc_referencia_empresa { get; set; }
        }

        public class eInfoAcademica_Trabajador : eTrabajador
        {
            public int cod_infoacademica { get; set; }
            public string cod_nivelacademico { get; set; }
            public string dsc_centroestudios { get; set; }
            public string dsc_lugar { get; set; }
            public string dsc_carrera_curso { get; set; }
            public string dsc_grado { get; set; }
            public int anho_inicio { get; set; }
            public int anho_fin { get; set; }
            public decimal imp_promedio { get; set; }
            public string flg_certificado { get; set; }
            public string id_certificado { get; set; }
            public string cod_centroestudios { get; set; }
            public string cod_carrera_profesional { get; set; }

            public string flg_EducComplPeru { get; set; }

            public string cod_tiporegimenacademico { get; set; }
            public string dsc_tiporegimenacademico { get; set; }

            public string cod_tipoinstitucion { get; set; }
            public string dsc_tipoinstitucion { get; set; }
        }

        public class eInfoProfesional_Trabajador : eTrabajador
        {
            public int cod_infoprofesional { get; set; }
            public string dsc_razon_social { get; set; }
            public string dsc_cargo { get; set; }
            public DateTime fch_inicio { get; set; }
            public DateTime fch_fin { get; set; }
            public string dsc_motivo_salida { get; set; }
            public string dsc_nombre_jefe { get; set; }
            public string dsc_cargo_jefe { get; set; }
            public string dsc_comentarios { get; set; }
            public string flg_certificado { get; set; }
            public string id_certificado { get; set; }
        }

        public class eInfoSalud_Trabajador : eTrabajador
        {
            public int cod_infosalud { get; set; }
            public string flg_alergias { get; set; }
            public string dsc_alergias { get; set; }
            public string flg_operaciones { get; set; }
            public string dsc_operaciones { get; set; }
            public string flg_discapacidad { get; set; }
            public string dsc_discapacidad { get; set; }
            public string flg_enfprexistente { get; set; }
            public string dsc_enfprexistente { get; set; }
            public string flg_tratprexistente { get; set; }
            public string dsc_tratprexistente { get; set; }
            public string flg_enfactual { get; set; }
            public string dsc_enfactual { get; set; }
            public string flg_tratactual { get; set; }
            public string dsc_tratactual { get; set; }
            public string flg_otros { get; set; }
            public string dsc_otros { get; set; }
         
            public string dsc_estadosalud { get; set; }
            public string dsc_tiposegurosalud { get; set; }
            public string dsc_segurosalud { get; set; }
            public string cod_tipo_trabajador_salud { get; set; }
            public string cod_sctr_salud { get; set; }
            public string cod_sctr_salud_pension { get; set; }
            public string cod_situacion_trabajador_salud { get; set; }
            public string cod_regimen_salud { get; set; }

            public string flg_AfectoVidaLey { get; set; }
            public string flg_Afecto_SCTR { get; set; }
            public string dsc_cia_segurovida { get; set; }
            public string cod_entidadeps { get; set; }
            public DateTime fch_scrtsalud { get; set; }
            public DateTime fch_scrtpension { get; set; }

        }

        public class eCertificadoEMO_Trabajador : eTrabajador
        {
            public Int32 cod_EMO { get; set; }
            public string dsc_descripcion { get; set; }
            public Int32 dsc_anho { get; set; }
            public string flg_certificado { get; set; }
            public string id_certificado { get; set; }
            public string dsc_nombrearchivo { get; set; }
        }

        public class eInfoVivienda_Trabajador : eTrabajador
        {
            public int cod_infovivienda { get; set; }
            public string cod_tipovivienda { get; set; }
            public string flg_puertas { get; set; }
            public string flg_ventanas { get; set; }
            public string flg_techo { get; set; }
            public string flg_telefono { get; set; }
            public string flg_celulares { get; set; }
            public string flg_internet_comunicacion { get; set; }
            public string cod_tipocomodidad { get; set; }
            public string flg_luz { get; set; }
            public string flg_agua { get; set; }
            public string flg_desague { get; set; }
            public string flg_gas { get; set; }
            public string flg_cable { get; set; }
            public string flg_internet_servicio { get; set; }
            public string dsc_viaacceso { get; set; }
            public string dsc_iluminacion { get; set; }
            public string dsc_entorno { get; set; }
            public string dsc_puestopolicial { get; set; }
            public string dsc_nombre_familiar { get; set; }
            public string dsc_horasencasa { get; set; }
            public string cod_parentesco { get; set; }
            public string dsc_mail { get; set; }
        }

        public class eInfoBancaria_Trabajador : eTrabajador
        {
            //public int cod_infobancaria { get; set; }
            public string cod_banco { get; set; }
            public string cod_moneda { get; set; }
            public string cod_tipo_cuenta { get; set; }
            public string dsc_cta_interbancaria { get; set; }
            public string cod_banco_CTS { get; set; }
            public string dsc_cta_interbancaria_CTS { get; set; }
            public string cod_sist_pension { get; set; }
            public string cod_APF { get; set; }
            public string cod_CUSPP { get; set; }
            public string cod_tipo_cuenta_CTS { get; set; }
            public string cod_tipo_comision_pension { get; set; }
            public string cod_periodicidad_pagos { get; set; }
            public string cod_tipo_pago { get; set; }
            
        }

        public class eEMO : eTrabajador
        {
            public DateTime fch_evaluacion { get; set; }
            public string dsc_observacion { get; set; }
            public DateTime fch_evaluacion_obs { get; set; }
            public string flg_observacion { get; set; }
            public Int32 cod_EMO { get; set; }
            public string dsc_descripcion { get; set; }
            public Int32 dsc_anho { get; set; }
            public string flg_certificado { get; set; }
            public string id_certificado { get; set; }

            public string estado { get; set; }
            public string cod_estado { get; set; }
            public string dsc_estado { get; set; }
            public string cod_documento { get; set; }
            public string tipo_doc { get; set; }
            public string nombre_archivoonedrive { get; set; }




        }

        public class eDetalleTrabajador : eTrabajador

        {
            public string cod_trabajador { get; set; }
            public int cod_infolab { get; set; }
            public string cod_empresa { get; set; }
            public string cod_sede_empresa { get; set; }
            public DateTime fch_ingreso { get; set; }
            public string cod_area { get; set; }
            public string cod_cargo { get; set; }
            public decimal imp_sueldo_base { get; set; }

            public DateTime fch_registro { get; set; }
            public string cod_usuario_registro { get; set; }
            public DateTime fch_cambio { get; set; }
            public string cod_usuario_cambio { get; set; }
        }

        public class eFormatos : eTrabajador
        {
            public string cod_formato { get; set; }
            public string flg_E4 { get; set; }
            public string flg_E5 { get; set; }
            public string flg_E11 { get; set; }
            public string flg_E17 { get; set; }
            public string flg_E29 { get; set; }
            public string flg_altaplacar { get; set; }

        }
        public class eDocumento : eTrabajador
        {
            public string cod_parametro { get; set; }
            public string dsc_parametro { get; set; }
            public string cod_sunat { get; set; }


        }

        public class eCaractFisicas_Trabajador : eTrabajador
        {
            //public int cod_caractfisica { get; set; }

            public decimal dsc_IMC { get; set; }
        }

        public class eDocumento_Trabajador : eTrabajador
        {
            //public int cod_caractfisica { get; set; }

            public int iddoc { get; set; }
            public string cod_documento { get; set; }
            public string dsc_documento { get; set; }
            public string flg_varios { get; set; }
            public string flg_activo { get; set; }
            public string dsc_modulo { get; set; }

            public string flg_resultados  { get; set; }

            public string dsc_archivo { get; set; }
            public string flg_documento { get; set; }
            public string id_documento { get; set; }
            public int cod_item { get; set; }
            public int flg_certificado { get; set; }

            public string remplazo { get; set; }
            public string dsc_abreviatura { get; set; }



        }

    }

    
    public class eTrabajador_EmpAreaCargo_Vista
    {
        public string cod_trabajador { get; set; }
        public string cod_empresa { get; set; }
        public string dsc_empresa { get; set; }
        public string cod_area { get; set; }
        public string dsc_area { get; set; }
        public string cod_cargo { get; set; }
        public string dsc_cargo { get; set; }
        public string dsc_trabajador { get; set; }
    }
    public class eTrabajador_EmpEmail_Vista
    {
        public string cod_trabajador { get; set; }
        public string cod_empresa { get; set; }
        public string dsc_empresa { get; set; }
        public string dsc_documento { get; set; }
        public string dsc_trabajador { get; set; }
        public string dsc_email { get; set; }
        public string dsc_pathfile { get; set; }
    }

    public class eTra
    {
        public string cod_trabajador { get; set; }
        public string cod_empresa { get; set; }
        public string dsc_empresa { get; set; }
        public string dsc_documento { get; set; }
        public string dsc_trabajador { get; set; }
        public string dsc_email { get; set; }
        public string dsc_pathfile { get; set; }
    }
    public class eRemuneracionMinimaUtil : eTrabajador
    {
        public int cod_mu { get; set; }
        public decimal rem_minimautil { get; set; }
        public decimal pcj_remuneracion { get; set; }
        public decimal total_rem { get; set; }
        public string flg_activo { get; set; }


    }

    //HOLA


}
