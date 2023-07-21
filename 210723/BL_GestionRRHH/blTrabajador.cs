using DA_GestionRRHH;
using BE_GestionRRHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraEditors;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;

namespace BL_GestionRRHH
{
    public class blTrabajador
    {
        readonly daSQL _sql;
        public blTrabajador(daSQL sql) { _sql = sql; }


        public void CargaCombosLookUp(string nCombo, LookUpEdit combo, string campoValueMember, string campoDispleyMember, string campoSelectedValue = "", bool valorDefecto = false, string cod_empresa = "", string cod_sede_empresa = "", string cod_area = "", string cod_tipoContrato = "", string cod_segmento = "", string cod_grupo = "", string cod_actividad = "", string cod_pais = "", string cod_departamento = "", string cod_provincia = "", string cod_usuario = "", string cod_documento = "", string flg_varios = "", string cod_trabajador = "",string cod_ejecutivo="", string cod_ocupacional = "", string cod_tiporegimenacademico="", string cod_tipoinsitucion="", string cod_centroestudios="")
        {
            combo.Text = "";
            string procedure = "usp_ConsultasVarias_Trabajador";
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            DataTable tabla = new DataTable();

            try
            {
                switch (nCombo)
                {
                    case "Empresa":
                        dictionary.Add("opcion", 2);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "EstadoCivil":
                        dictionary.Add("opcion", 3);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "SistPension":
                        dictionary.Add("opcion", 4);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "NombreAFP":
                        dictionary.Add("opcion", 5);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "SedesEmpresa":
                        dictionary.Add("opcion", 6);
                        dictionary.Add("cod_empresa", cod_empresa);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "AreaEmpresa":
                        dictionary.Add("opcion", 7);
                        dictionary.Add("cod_empresa", cod_empresa);
                        dictionary.Add("cod_sede_empresa", cod_sede_empresa);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "CargoEmpresa":
                        dictionary.Add("opcion", 8);
                        dictionary.Add("cod_empresa", cod_empresa);
                        dictionary.Add("cod_sede_empresa", cod_sede_empresa);
                        dictionary.Add("cod_area", cod_area);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "TipoContrato":
                        dictionary.Add("opcion", 9);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "Modalidad":
                        dictionary.Add("opcion", 10);
                        dictionary.Add("cod_tipoContrato", cod_tipoContrato);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "Parentesco":
                        dictionary.Add("opcion", 11);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "GrupoSanguineo":
                        dictionary.Add("opcion", 15);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "EstadoSalud":
                        dictionary.Add("opcion", 16);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "TipoSeguro":
                        dictionary.Add("opcion", 17);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "PeriodoPrueba":
                        dictionary.Add("opcion", 18);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "NivelAcademico":
                        dictionary.Add("opcion", 19);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "TipoPropiedad":
                        dictionary.Add("opcion", 20);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "TipoVivienda":
                        dictionary.Add("opcion", 21);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "TipoComodidad":
                        dictionary.Add("opcion", 22);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "TallasUniforme":
                        dictionary.Add("opcion", 23);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "Segmento":
                        dictionary.Add("opcion", 24);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "Grupo":
                        dictionary.Add("opcion", 25);
                        dictionary.Add("cod_segmento", cod_segmento);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "Actividad":
                        dictionary.Add("opcion", 26);
                        dictionary.Add("cod_segmento", cod_segmento);
                        dictionary.Add("cod_grupo", cod_grupo);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "Usuarios":
                        procedure = "usp_Consulta_ListarUsuarios";
                        dictionary.Add("opcion", 1);
                        dictionary.Add("flg_activo", "SI");
                        dictionary.Add("cod_perfil", "");
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "UsuariosControlHoras":
                        dictionary.Add("opcion", 28);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "Areas":
                        dictionary.Add("opcion", 33);
                        dictionary.Add("cod_empresa", cod_empresa);
                        dictionary.Add("cod_sede_empresa", cod_sede_empresa);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "Nacionalidad":
                        dictionary.Add("opcion", 38);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "Sexo":
                        dictionary.Add("opcion", 39);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "Pais":
                        dictionary.Add("opcion", 40);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "Departamento":
                        dictionary.Add("opcion", 41);
                        dictionary.Add("cod_pais", cod_pais);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "Provincia":
                        dictionary.Add("opcion", 42);
                        dictionary.Add("cod_pais", cod_pais);
                        dictionary.Add("cod_departamento", cod_departamento);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "Distrito":
                        dictionary.Add("opcion", 43);
                        dictionary.Add("cod_pais", cod_pais);
                        dictionary.Add("cod_departamento", cod_departamento);
                        dictionary.Add("cod_provincia", cod_provincia);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "Tipovia":
                        dictionary.Add("opcion", 44);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "TipoZona":
                        dictionary.Add("opcion", 45);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "NivAcademico":
                        dictionary.Add("opcion", 46);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "SCRTSALUD":
                        dictionary.Add("opcion", 47);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "TrabajadorSalud":
                        dictionary.Add("opcion", 48);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "SCRTPension":
                        dictionary.Add("opcion", 49);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "SituacionTrabajador_salud":
                        dictionary.Add("opcion", 51);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "EntidadFinanciera":
                        dictionary.Add("opcion", 52);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "TipoCuenta":
                        dictionary.Add("opcion", 53);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "TipoComision":
                        dictionary.Add("opcion", 54);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "RegimenPension":
                        dictionary.Add("opcion", 55);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "Moneda":
                        dictionary.Add("opcion", 58);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "TipoDocumentoTrabajador":
                        dictionary.Add("opcion", 59);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "TipoDocumentoVista":
                        dictionary.Add("opcion", 35);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "MotivosDespido":
                        dictionary.Add("opcion", 60);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "PeriodicidadPago":
                        dictionary.Add("opcion", 61);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "SituacionTrabajador2":
                        dictionary.Add("opcion", 62);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "Sino":
                        dictionary.Add("opcion", 63);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "SituacionEspecial":
                        dictionary.Add("opcion", 64);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "TipoPago":
                        dictionary.Add("opcion", 65);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "Ocupacional":
                        dictionary.Add("opcion", 66);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "ConvenioTributacion":
                        dictionary.Add("opcion", 67);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "Categoria":
                        dictionary.Add("opcion", 68);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "TipoTrabajo":
                        dictionary.Add("opcion", 69);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "RegimenLaboral":
                        dictionary.Add("opcion", 70);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "tipocuentacts":
                        dictionary.Add("opcion", 71);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "regimensegurosalud":
                        dictionary.Add("opcion", 72);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "menupais":
                        dictionary.Add("opcion", 74);

                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "menudepartamento":
                        dictionary.Add("opcion", 75);
                        dictionary.Add("cod_pais", cod_pais);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "menuprovincia":
                        dictionary.Add("opcion", 76);
                        dictionary.Add("cod_departamento", cod_departamento);
                        dictionary.Add("cod_pais", cod_pais);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "menudistrito":
                        dictionary.Add("opcion", 77);
                        dictionary.Add("cod_provincia", cod_provincia);
                        dictionary.Add("cod_departamento", cod_departamento);
                        dictionary.Add("cod_pais", cod_pais);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "scrtcentroriesgo":
                        dictionary.Add("opcion", 80);
                        dictionary.Add("cod_empresa", cod_empresa);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "seguroley":
                        dictionary.Add("opcion", 81);
                        dictionary.Add("cod_empresa", cod_empresa);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "EmpresaUsuario":
                        dictionary.Add("opcion", 82);
                        dictionary.Add("cod_usuario", cod_usuario);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "TipoDocumentoEconomico":
                        dictionary.Add("opcion", 86);
                        dictionary.Add("cod_usuario", cod_usuario);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "CentrodeEstudios":
                        dictionary.Add("opcion", 88);
                        dictionary.Add("cod_tiporegimenacademico", cod_tiporegimenacademico);
                        dictionary.Add("cod_tipoinsitucion", cod_tipoinsitucion);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "CarreraProfesionales":
                        dictionary.Add("opcion", 89);
                        dictionary.Add("cod_tiporegimenacademico", cod_tiporegimenacademico);
                        dictionary.Add("cod_tipoinsitucion", cod_tipoinsitucion);
                        dictionary.Add("cod_centroestudios", cod_centroestudios);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "CIASegurovidaley":
                        dictionary.Add("opcion", 90);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "CentroRiesgo":
                        dictionary.Add("opcion", 91);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "TipoDocumento":
                        dictionary.Add("opcion", 106);
                        dictionary.Add("flg_varios", flg_varios);
                        dictionary.Add("cod_empresa", cod_empresa);

                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "TipoDocumentor":
                        dictionary.Add("opcion", 112);
                        dictionary.Add("flg_varios", flg_varios);
                        dictionary.Add("cod_trabajador", cod_trabajador);
                        dictionary.Add("cod_empresa", cod_empresa);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "TipoDocumentoTIPO":
                        dictionary.Add("opcion", 116);
                        dictionary.Add("cod_empresa", cod_empresa);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "Ocupacionesejecutivas":
                        dictionary.Add("opcion", 121);
                        dictionary.Add("cod_ocupacional", cod_ocupacional);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "entidadeps":
                        dictionary.Add("opcion", 122);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;

                    case "TipoRegimenAcademico":
                        dictionary.Add("opcion", 123);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "TipoInstitucion":
                        dictionary.Add("opcion", 124);
                        dictionary.Add("cod_tiporegimenacademico", cod_tiporegimenacademico);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "MotivosDespidointerno":
                        dictionary.Add("opcion", 128);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;



                }

                combo.Properties.DataSource = tabla;
                combo.Properties.ValueMember = campoValueMember;
                combo.Properties.DisplayMember = campoDispleyMember;
                if (campoSelectedValue == "") { combo.ItemIndex = -1; } else { combo.EditValue = campoSelectedValue; }
                if (tabla.Columns["flg_default"] != null) if (valorDefecto) combo.EditValue = tabla.Select("flg_default = 'SI'").Length == 0 ? null : (tabla.Select("flg_default = 'SI'"))[0].ItemArray[0];
            
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataTable ObtenerListadoGridLookup(string nCombo, string cod_condicion = "")
        {
            string procedure = "usp_ConsultasVarias_Trabajador";
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            DataTable tabla = new DataTable();

            try
            {
                switch (nCombo)
                {
                    case "TipoDocumento":
                        dictionary.Add("opcion", 1);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "TipoDistrito":
                        procedure = "usp_ConsultasVarias_Cliente";
                        dictionary.Add("opcion", 16);
                        dictionary.Add("cod_condicion", cod_condicion);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "Banco":
                        procedure = "usp_ConsultasVarias_Proveedor";
                        dictionary.Add("opcion", 6);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    //    vista_usuario_trabajador
                    case "TipoDocumentoVista":
                        dictionary.Add("opcion", 35);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;

                }
                return tabla;
            }
            catch (Exception ex)
            {
                return new DataTable();
                throw;
            }
        }

        public List<T> ListarOpcionesTrabajador<T>(int opcion, string cod_empresa = "") where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> oDictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion },
                { "cod_empresa", cod_empresa }
            };

            myList = _sql.ListaconSP<T>("usp_ConsultasVarias_Trabajador", oDictionary);
            return myList;
        }

        public T ObtenerDatosOneDrive<T>(int opcion, string cod_empresa = "", string dsc_Carpeta = "", string cod_trabajador = "") where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> oDictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion },
                { "cod_empresa", cod_empresa },
                { "dsc_Carpeta", dsc_Carpeta },
                { "cod_trabajador", cod_trabajador }
            };

            obj = _sql.ConsultarEntidad<T>("usp_ConsultasVarias_Trabajador", oDictionary);
            return obj;
        }

        public string ActualizarInformacionDocumentos(string flg_Adjuntos, int opcionDoc, eTrabajador objTrab)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("cod_trabajador", objTrab.cod_trabajador);
            dictionary.Add("cod_empresa", objTrab.cod_empresa);
            dictionary.Add("flg_Adjuntos", flg_Adjuntos);
            dictionary.Add("opcionDoc", opcionDoc);
            dictionary.Add("idCarpeta_Trabajador", objTrab.idCarpeta_Trabajador);
            dictionary.Add("id_DNI", objTrab.id_DNI);
            dictionary.Add("id_CV", objTrab.id_CV);
            dictionary.Add("id_AntPolicial", objTrab.id_AntPolicial);
            dictionary.Add("id_AntPenal", objTrab.id_AntPenal);
            dictionary.Add("id_VerifDomiciliaria", objTrab.id_VerifDomiciliaria);

            string result;
            result = _sql.ExecuteScalarWithParams("usp_Insertar_Actualizar_Trabajador", dictionary);

            return result;
        }

        public string EliminarInactivar_DatosTrabajador(int opcion, string cod_trabajador = "", string cod_usuario_registro = "", int cod_contactemerg = 0, int cod_infolab = 0, int cod_infofamiliar = 0, int cod_infoacademica = 0, int cod_infoprofesional = 0, int cod_infoeconomica = 0, string cod_empresa = "", string cod_sede_empresa = "", string cod_area = "", string cod_cargo = "", string cod_carrera_profesional = "", string cod_centroestudios = "")
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("opcion", opcion);
            dictionary.Add("cod_trabajador", cod_trabajador);
            dictionary.Add("cod_contactemerg", cod_contactemerg);
            dictionary.Add("cod_infolab", cod_infolab);
            dictionary.Add("cod_infofamiliar", cod_infofamiliar);
            dictionary.Add("cod_infoacademica", cod_infoacademica);
            dictionary.Add("cod_infoprofesional", cod_infoprofesional);
            dictionary.Add("cod_infoeconomica", cod_infoeconomica);
            dictionary.Add("cod_usuario_registro", cod_usuario_registro);
            dictionary.Add("cod_empresa", cod_empresa);
            dictionary.Add("cod_area", cod_area);
            dictionary.Add("cod_sede_empresa", cod_sede_empresa);
            dictionary.Add("cod_cargo", cod_cargo);

            string result;
            result = _sql.ExecuteScalarWithParams("usp_Eliminar_Inactivar_DatosTrabajador", dictionary);

            return result;
        }

        public List<T> ListarTrabajadores<T>(int opcion, string cod_trabajador, string cod_empresa = "") where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> oDictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion },
                { "cod_trabajador", cod_trabajador },
                { "cod_empresa", cod_empresa },


            };

            myList = _sql.ListaconSP<T>("usp_Consulta_ListarTrabajador", oDictionary);
            return myList;
        }

        public T Obtener_Trabajador<T>(int opcion, string cod_trabajador = "", string cod_empresa = "", string cod_infolab = "", string cod_usuario = "", string dsc_documento = "") where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion },
                { "cod_trabajador", cod_trabajador },
                { "cod_empresa", cod_empresa },
                 { "cod_infolab", cod_infolab },
                { "cod_usuario", cod_usuario },
                { "dsc_documento", dsc_documento },
            };

            obj = _sql.ConsultarEntidad<T>("usp_Consulta_ListarTrabajador", dictionary);
            return obj;
        }

        public T Obtener_costo_usuario<T>(string usuario) where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "opcion", 27 },
                { "cod_usuario", usuario }
            };

            obj = _sql.ConsultarEntidad<T>("usp_ConsultasVarias_Trabajador", dictionary);
            return obj;
        }

        public T InsertarActualizar_Trabajador<T>(eTrabajador eTrab) where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "cod_trabajador", eTrab.cod_trabajador },
                { "cod_usuario", eTrab.cod_usuario },
                { "cod_empresa", eTrab.cod_empresa },
                { "dsc_apellido_paterno", eTrab.dsc_apellido_paterno },
                { "dsc_apellido_materno", eTrab.dsc_apellido_materno },
                { "dsc_nombres", eTrab.dsc_nombres },
                { "cod_estadocivil", eTrab.cod_estadocivil },
                { "fch_nacimiento", eTrab.fch_nacimiento },
                { "flg_sexo", eTrab.flg_sexo },
                { "cod_tipo_documento", eTrab.cod_tipo_documento },
                { "dsc_documento", eTrab.dsc_documento },
				//{ "fch_vcto_documento", eTrab.fch_vcto_documento },
				{ "nro_ubigeo_documento", eTrab.nro_ubigeo_documento },
                { "cod_nacionalidad", eTrab.cod_nacionalidad },
                { "dsc_direccion", eTrab.dsc_direccion },
                { "cod_distrito", eTrab.cod_distrito },
                { "cod_provincia", eTrab.cod_provincia },
                { "cod_departamento", eTrab.cod_departamento },
                { "cod_pais", eTrab.cod_pais },
                {"dsc_lote",eTrab.cod_lote},
                {"cod_tipo_via",eTrab.cod_tipo_via },
                {"dsc_tipo_via",eTrab.dsc_tipo_via},
                {"dsc_nro_vivienda",eTrab.dsc_nro_vivienda },
                {"dsc_departamento_dir",eTrab.dsc_departamento_dir},
                {"dsc_interior",eTrab.dsc_interior},
                {"dsc_manzana",eTrab.dsc_manzana},
                {"dsc_etapa",eTrab.dsc_etapa },
                {"dsc_block",eTrab.dsc_block},
                {"dsc_kilometro_dir",eTrab.dsc_km},
                {"dsc_tipo_zona",eTrab.dsc_tipo_zona},
                {"dsc_nombre_zona",eTrab.dsc_nombre_zona},
                { "dsc_referencia", eTrab.dsc_referencia },
                { "dsc_mail_1", eTrab.dsc_mail_1 },
                { "dsc_mail_2", eTrab.dsc_mail_2 },
                { "dsc_telefono", eTrab.dsc_telefono },
                { "dsc_celular", eTrab.dsc_celular },
                { "cod_sist_pension", eTrab.cod_sist_pension },
                { "cod_APF", eTrab.cod_APF },
                { "cod_CUSPP", eTrab.cod_CUSPP },
                { "flg_DNI", eTrab.flg_DNI },
                { "flg_CV", eTrab.flg_CV },
                { "flg_AntPolicial", eTrab.flg_AntPolicial },
                { "flg_AntPenal", eTrab.flg_AntPenal },
                { "flg_VerifDomiciliaria", eTrab.flg_VerifDomiciliaria },
                { "cod_TipoPersonal", eTrab.cod_TipoPersonal },
                { "flg_activo", eTrab.flg_activo },
                { "cod_usuario_registro", eTrab.cod_usuario_registro },
            };
            if (eTrab.fch_vcto_documento.ToString().Contains("0001")) { dictionary.Add("fch_vcto_documento", DBNull.Value); } else { dictionary.Add("fch_vcto_documento", eTrab.fch_vcto_documento); }
            if (eTrab.fch_entrega_uniforme.ToString().Contains("0001")) { dictionary.Add("fch_entrega_uniforme", DBNull.Value); } else { dictionary.Add("fch_entrega_uniforme", eTrab.fch_entrega_uniforme); }
            if (eTrab.fch_renovacion_uniforme.ToString().Contains("0001")) { dictionary.Add("fch_renovacion_uniforme", DBNull.Value); } else { dictionary.Add("fch_renovacion_uniforme", eTrab.fch_renovacion_uniforme); }

            obj = _sql.ConsultarEntidad<T>("usp_Insertar_Actualizar_Trabajador", dictionary);
            return obj;
        }

        public T InsertarActualizar_ContactoTrabajador<T>(eTrabajador.eContactoEmergencia_Trabajador eTrab) where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "cod_trabajador", eTrab.cod_trabajador },
                { "cod_empresa", eTrab.cod_empresa },
                { "cod_contactemerg", eTrab.cod_contactemerg },
                { "dsc_apellido_paterno", eTrab.dsc_apellido_paterno },
                { "dsc_apellido_materno", eTrab.dsc_apellido_materno },
                { "dsc_nombres", eTrab.dsc_nombres },
                { "cod_parentesco", eTrab.cod_parentesco },
                { "cod_tipo_documento", eTrab.cod_tipo_documento },
                { "dsc_documento", eTrab.dsc_documento },
                //{ "fch_nacimiento", eTrab.fch_nacimiento },
                { "dsc_telefono", eTrab.dsc_telefono },
                { "dsc_celular", eTrab.dsc_celular },
                { "flg_activo", eTrab.flg_activo },
                { "cod_usuario_registro", eTrab.cod_usuario_registro },
            };
            if (eTrab.fch_nacimiento.ToString().Contains("0001")) { dictionary.Add("fch_nacimiento", DBNull.Value); } else { dictionary.Add("fch_nacimiento", eTrab.fch_nacimiento); }

            obj = _sql.ConsultarEntidad<T>("usp_Insertar_Actualizar_ContactEmergenciaTrabajador", dictionary);
            return obj;
        }

        public T InsertarActualizar_InfoLaboralTrabajador<T>(eTrabajador.eInfoLaboral_Trabajador eTrab, string flg_documento = "NO") where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "cod_trabajador", eTrab.cod_trabajador },
                { "cod_infolab", eTrab.cod_infolab },
                { "fch_ingreso", eTrab.fch_ingreso },
                { "cod_empresa", eTrab.cod_empresa },
                { "cod_sede_empresa", eTrab.cod_sede_empresa },
                { "cod_area", eTrab.cod_area },
                { "cod_cargo", eTrab.cod_cargo },
                { "dsc_pref_ceco", eTrab.dsc_pref_ceco },
                { "cod_tipo_contrato", eTrab.cod_tipo_contrato },
                { "fch_firma", eTrab.fch_firma }, // INICIOCONTRATO
                { "imp_sueldo_base", eTrab.imp_sueldo_base },
                { "imp_asig_familiar", eTrab.imp_asig_familiar },
                { "imp_movilidad", eTrab.imp_movilidad },
                { "imp_alimentacion", eTrab.imp_alimentacion },
                { "imp_escolaridad", eTrab.imp_escolaridad },
                { "imp_bono", eTrab.imp_bono },
                { "flg_regimen_atipico", eTrab.flg_regimen_atipico },
                { "flg_jornada_maxima", eTrab.flg_jornada_maxima },
                { "flg_horario_nocturno", eTrab.flg_horario_nocturno },
                { "flg_sindicato", eTrab.flg_sindicato },
                { "cod_situacion_trabajador_2", eTrab.cod_situacion_trabajador_2 },
                { "cod_categoria_trabajador", eTrab.cod_categoria_trabajador },
                { "cod_regimen_laboral", eTrab.cod_regimen_laboral },
                { "cod_exoneracion_5ta", eTrab.cod_exoneracion_5ta },
                { "cod_ocupacional", eTrab.cod_ocupacional },
                { "flg_activo", eTrab.flg_activo },
                { "cod_usuario_registro", eTrab.cod_usuario_registro },
                { "codsunat_scrtcentroriesgo", eTrab.codsunat_scrtcentroriesgo },
                { "codsunat_seguroley", eTrab.codsunat_seguroley },
                { "flg_afectoSCTR", eTrab.flg_afectoSCTR },
                { "flg_AfectoVidaLey", eTrab.flg_AfectoVidaLey },
                { "flg_horas_extras", eTrab.flg_horas_extras },
                { "cod_tipo_trabajador", eTrab.cod_tipo_trabajador },
                { "cod_conveniotributacion", eTrab.cod_conveniotributacion },
                { "flg_asignacionfamiliar", eTrab.flg_asignacionfamiliar },
                { "flg_Regimen_pension", eTrab.flg_Regimen_pension },
                { "cod_situacion_especial", eTrab.cod_situacion_especial },
                { "cant_hijos", eTrab.cant_hijos },
                { "id_contrato", eTrab.id_contrato },
                { "flg_documento", flg_documento },
                { "cod_modalidad", eTrab.cod_modalidad },
                { "dsc_porcentajecomision", eTrab.dsc_porcentajecomision },
                { "dsc_porcentajequincena", eTrab.dsc_porcentajequincena },
                { "correo_laboral", eTrab.correo_laboral },
                { "cod_ocupaciones", eTrab.cod_ocupaciones }

            };

            if (eTrab.fch_vencimiento.ToString().Contains("0001")) { dictionary.Add("fch_vencimiento", DBNull.Value); } else { dictionary.Add("fch_vencimiento", eTrab.fch_vencimiento); }

            obj = _sql.ConsultarEntidad<T>("usp_Insertar_Actualizar_InfoLaboralTrabajador", dictionary);
            return obj;
        }


        public T InsertarActualizar_CaractFisicasTrabajador<T>(eTrabajador.eCaractFisicas_Trabajador eTrab) where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "cod_trabajador", eTrab.cod_trabajador },
                { "cod_empresa", eTrab.cod_empresa },
                //{ "cod_caractfisica", eTrab.cod_caractfisica },
                { "dsc_estatura", eTrab.dsc_estatura },
                { "dsc_peso", eTrab.dsc_peso },
                { "flg_lentes", eTrab.flg_lentes },
                { "dsc_IMC", eTrab.dsc_IMC },
                { "flg_activo", eTrab.flg_activo },
                { "cod_usuario_registro", eTrab.cod_usuario_registro },
            };

            obj = _sql.ConsultarEntidad<T>("usp_Insertar_Actualizar_CaractFisicasTrabajador", dictionary);
            return obj;
        }

        public T InsertarActualizar_TallaUniformesTrabajador<T>(eTrabajador.eTallaUniforme_Trabajador eTrab) where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "cod_trabajador", eTrab.cod_trabajador },
                { "cod_empresa", eTrab.cod_empresa },
                //{ "cod_tallauniforme", eTrab.cod_tallauniforme },
                { "cod_talla_polo", eTrab.cod_talla_polo },
                { "cod_talla_camisa", eTrab.cod_talla_camisa },
                { "cod_talla_pantalon", eTrab.cod_talla_pantalon },
                { "cod_talla_mameluco", eTrab.cod_talla_mameluco },
                { "cod_talla_casaca", eTrab.cod_talla_casaca },
                { "cod_talla_chaleco", eTrab.cod_talla_chaleco },
                { "cod_talla_calzado", eTrab.cod_talla_calzado },
                 { "cod_talla_casco", eTrab.cod_talla_casco },
                  { "cod_talla_faja", eTrab.cod_talla_faja },
                { "flg_activo", eTrab.flg_activo },
                { "cod_usuario_registro", eTrab.cod_usuario_registro },
            };

            obj = _sql.ConsultarEntidad<T>("usp_Insertar_Actualizar_TallaUniformesTrabajador", dictionary);
            return obj;
        }

        public T InsertarActualizar_InfoFamiliarTrabajador<T>(eTrabajador.eInfoFamiliar_Trabajador eTrab) where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "cod_trabajador", eTrab.cod_trabajador },
                { "cod_empresa", eTrab.cod_empresa },
                { "cod_infofamiliar", eTrab.cod_infofamiliar },
                { "cod_parentesco", eTrab.cod_parentesco },
                { "dsc_apellido_paterno", eTrab.dsc_apellido_paterno },
                { "dsc_apellido_materno", eTrab.dsc_apellido_materno },
                { "dsc_nombres", eTrab.dsc_nombres },
                { "fch_nacimiento", eTrab.fch_nacimiento },
                { "flg_vivo", eTrab.flg_vivo },
                { "cod_tipo_documento", eTrab.cod_tipo_documento },
                { "dsc_documento", eTrab.dsc_documento },
                { "dsc_mail", eTrab.dsc_mail },
                { "dsc_telefono", eTrab.dsc_telefono },
                { "dsc_celular", eTrab.dsc_celular },
                { "dsc_profesion", eTrab.dsc_profesion },
                { "dsc_centrolaboral", eTrab.dsc_centrolaboral },
                { "dsc_gradoinstruccion", eTrab.dsc_gradoinstruccion },
                { "dsc_ocupacion", eTrab.dsc_ocupacion },
                { "dsc_discapacidad", eTrab.dsc_discapacidad },
                { "dsc_direccion", eTrab.dsc_direccion },
                { "cod_distrito", eTrab.cod_distrito },
                { "cod_provincia", eTrab.cod_provincia },
                { "cod_departamento", eTrab.cod_departamento },
                { "cod_pais", eTrab.cod_pais },
                { "dsc_referencia", eTrab.dsc_referencia },
                { "flg_enfermedad", eTrab.flg_enfermedad },
                { "dsc_enfermedad", eTrab.dsc_enfermedad },
                { "flg_adiccion", eTrab.flg_adiccion },
                { "dsc_adiccion", eTrab.dsc_adiccion },
                { "flg_dependenciaeconomica", eTrab.flg_dependenciaeconomica },
                { "flg_activo", eTrab.flg_activo },
                { "cod_usuario_registro", eTrab.cod_usuario_registro },
            };
            //if (eTrab.fch_nacimiento.ToString().Contains("0001")) { dictionary.Add("fch_nacimiento", DBNull.Value); } else { dictionary.Add("fch_nacimiento", eTrab.fch_nacimiento); }

            obj = _sql.ConsultarEntidad<T>("usp_Insertar_Actualizar_InfoFamiliarTrabajador", dictionary);
            return obj;
        }

        public T InsertarActualizar_InfoEconomicaTrabajador<T>(eTrabajador.eInfoEconomica_Trabajador eTrab) where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "cod_trabajador", eTrab.cod_trabajador },
                { "cod_empresa", eTrab.cod_empresa },
                { "cod_infoeconomica", eTrab.cod_infoeconomica },
                { "imp_ingresomensual", eTrab.imp_ingresomensual },
                { "imp_gastomensual", eTrab.imp_gastomensual },
                { "imp_totalactivos", eTrab.imp_totalactivos },
                { "imp_totaldeudas", eTrab.imp_totaldeudas },
                { "cod_vivienda", eTrab.cod_vivienda },
                { "cod_tipovivienda", eTrab.cod_tipovivienda },
                { "imp_valorvivienda", eTrab.imp_valorvivienda },
                { "cod_monedavivienda", eTrab.cod_monedavivienda },
                { "dsc_direccion_vivienda", eTrab.dsc_direccion_vivienda },
                { "cod_distrito_vivienda", eTrab.cod_distrito_vivienda },
                { "cod_provincia_vivienda", eTrab.cod_provincia_vivienda },
                { "cod_departamento_vivienda", eTrab.cod_departamento_vivienda },
                { "cod_pais_vivienda", eTrab.cod_pais_vivienda },
                { "dsc_referencia_vivienda", eTrab.dsc_referencia_vivienda },
                { "cod_tipovehiculo", eTrab.cod_tipovehiculo },
                { "dsc_marcavehiculo", eTrab.dsc_marcavehiculo },
                { "dsc_modelovehiculo", eTrab.dsc_modelovehiculo },
                { "dsc_placavehiculo", eTrab.dsc_placavehiculo },
                { "imp_valorvehiculo", eTrab.imp_valorvehiculo },
                { "cod_monedavehiculo", eTrab.cod_monedavehiculo },
                { "cod_tipoempresa", eTrab.cod_tipoempresa },
                { "dsc_participacion_empresa", eTrab.dsc_participacion_empresa },
                { "dsc_RUC_empresa", eTrab.dsc_RUC_empresa },
                { "dsc_giro_empresa", eTrab.dsc_giro_empresa },
                { "dsc_direccion_empresa", eTrab.dsc_direccion_empresa },
                { "cod_distrito_empresa", eTrab.cod_distrito_empresa },
                { "cod_provincia_empresa", eTrab.cod_provincia_empresa },
                { "cod_departamento_empresa", eTrab.cod_departamento_empresa },
                { "cod_pais_empresa", eTrab.cod_pais_empresa },
                { "dsc_referencia_empresa", eTrab.dsc_referencia_empresa },
                { "flg_activo", eTrab.flg_activo },
                { "cod_usuario_registro", eTrab.cod_usuario_registro },
            };

            obj = _sql.ConsultarEntidad<T>("usp_Insertar_Actualizar_InfoEconomicaTrabajador", dictionary);
            return obj;
        }

        public T InsertarActualizar_InfoAcademicaTrabajador<T>(eTrabajador.eInfoAcademica_Trabajador eTrab, string flg_documento = "NO") where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "cod_trabajador", eTrab.cod_trabajador },
                { "cod_empresa", eTrab.cod_empresa },
                { "cod_infoacademica", eTrab.cod_infoacademica },
                { "cod_nivelacademico", eTrab.cod_nivelacademico },
                { "dsc_centroestudios", eTrab.dsc_centroestudios },
                { "dsc_lugar", eTrab.dsc_lugar },
                { "dsc_carrera_curso", eTrab.dsc_carrera_curso },
                { "dsc_grado", eTrab.dsc_grado },
                { "anho_inicio", eTrab.anho_inicio },
                { "anho_fin", eTrab.anho_fin },
                { "imp_promedio", eTrab.imp_promedio },
                { "flg_activo", eTrab.flg_activo },
                { "cod_usuario_registro", eTrab.cod_usuario_registro },
                { "id_certificado", eTrab.id_certificado },
                { "flg_documento", flg_documento },
                { "cod_centroestudios", eTrab.cod_centroestudios },
                { "cod_carrera_profesional", eTrab.cod_carrera_profesional },
                { "cod_pais", eTrab.cod_pais },
                { "flg_EducComplPeru", eTrab.flg_EducComplPeru },
                { "cod_tiporegimenacademico", eTrab.cod_tiporegimenacademico },
                { "cod_tipoinstitucion", eTrab.cod_tipoinstitucion }
            };

            obj = _sql.ConsultarEntidad<T>("usp_Insertar_Actualizar_InfoAcademicaTrabajador", dictionary);
            return obj;
        }

        public T InsertarActualizar_InfoProfesionalTrabajador<T>(eTrabajador.eInfoProfesional_Trabajador eTrab, string flg_documento = "NO") where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "cod_trabajador", eTrab.cod_trabajador },
                { "cod_empresa", eTrab.cod_empresa },
                { "cod_infoprofesional", eTrab.cod_infoprofesional },
                { "dsc_razon_social", eTrab.dsc_razon_social },
                { "dsc_cargo", eTrab.dsc_cargo },
                //{ "fch_inicio", eTrab.fch_inicio },
                //{ "fch_fin", eTrab.fch_fin },
                { "dsc_motivo_salida", eTrab.dsc_motivo_salida },
                { "dsc_nombre_jefe", eTrab.dsc_nombre_jefe },
                { "dsc_cargo_jefe", eTrab.dsc_cargo_jefe },
                { "dsc_celular", eTrab.dsc_celular },
                { "dsc_comentarios", eTrab.dsc_comentarios },
                { "flg_activo", eTrab.flg_activo },
                { "cod_usuario_registro", eTrab.cod_usuario_registro },
                { "id_certificado", eTrab.id_certificado },
                { "flg_documento", flg_documento },
            };
            if (eTrab.fch_inicio.ToString().Contains("0001")) { dictionary.Add("fch_inicio", DBNull.Value); } else { dictionary.Add("fch_inicio", eTrab.fch_inicio); }
            if (eTrab.fch_fin.ToString().Contains("0001")) { dictionary.Add("fch_fin", DBNull.Value); } else { dictionary.Add("fch_fin", eTrab.fch_fin); }

            obj = _sql.ConsultarEntidad<T>("usp_Insertar_Actualizar_InfoProfesionalTrabajador", dictionary);
            return obj;
        }

        public T InsertarActualizar_InfoBancariaTrabajador<T>(eTrabajador.eInfoBancaria_Trabajador eTrab) where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "cod_trabajador", eTrab.cod_trabajador },
                { "cod_empresa", eTrab.cod_empresa },
                { "cod_banco", eTrab.cod_banco },
                { "cod_moneda", eTrab.cod_moneda },
                { "cod_tipo_cuenta", eTrab.cod_tipo_cuenta },
                { "dsc_cta_bancaria", eTrab.dsc_cta_bancaria },
                { "dsc_cta_interbancaria", eTrab.dsc_cta_interbancaria },
                { "cod_banco_CTS", eTrab.cod_banco_CTS },
                { "cod_moneda_CTS", eTrab.cod_moneda_CTS },
                { "dsc_cta_bancaria_CTS", eTrab.dsc_cta_bancaria_CTS },
                { "dsc_cta_interbancaria_CTS", eTrab.dsc_cta_interbancaria_CTS },
                { "cod_sist_pension", eTrab.cod_sist_pension },
                { "cod_APF", eTrab.cod_APF },
                { "cod_CUSPP", eTrab.cod_CUSPP },
                { "flg_activo", eTrab.flg_activo },
                { "cod_usuario_registro", eTrab.cod_usuario_registro },
                { "cod_tipo_cuenta_CTS", eTrab.cod_tipo_cuenta_CTS },
                { "cod_tipo_comision_pension", eTrab.cod_tipo_comision_pension },
                { "cod_tipo_pago", eTrab.cod_tipo_pago },
                {"cod_periodicidad_pagos",eTrab.cod_periodicidad_pagos}




            };

            obj = _sql.ConsultarEntidad<T>("usp_Insertar_Actualizar_InfoBancariaTrabajador", dictionary);
            return obj;
        }
        public T InsertarActualizar_InfoSaludTrabajador<T>(eTrabajador.eInfoSalud_Trabajador eTrab, string flg_documento = "NO") where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "cod_trabajador", eTrab.cod_trabajador },
                { "cod_empresa", eTrab.cod_empresa },
                //{ "cod_infosalud", eTrab.cod_infosalud },
                { "flg_alergias", eTrab.flg_alergias },
                { "dsc_alergias", eTrab.dsc_alergias },
                { "flg_operaciones", eTrab.flg_operaciones },
                { "dsc_operaciones", eTrab.dsc_operaciones },
                { "flg_discapacidad", eTrab.flg_discapacidad },
                { "dsc_discapacidad", eTrab.dsc_discapacidad },
                { "flg_enfprexistente", eTrab.flg_enfprexistente },
                { "dsc_enfprexistente", eTrab.dsc_enfprexistente },
                { "flg_tratprexistente", eTrab.flg_tratprexistente },
                { "dsc_tratprexistente", eTrab.dsc_tratprexistente },
                { "flg_enfactual", eTrab.flg_enfactual },
                { "dsc_enfactual", eTrab.dsc_enfactual },
                { "flg_tratactual", eTrab.flg_tratactual },
                { "dsc_tratactual", eTrab.dsc_tratactual },
                { "flg_otros", eTrab.flg_otros },
                { "dsc_otros", eTrab.dsc_otros },
                { "dsc_gruposanguineo", eTrab.dsc_gruposanguineo },
                { "dsc_estadosalud", eTrab.dsc_estadosalud },
                { "dsc_tiposegurosalud", eTrab.dsc_tiposegurosalud },
                { "dsc_segurosalud", eTrab.dsc_segurosalud },
                { "cod_usuario_registro", eTrab.cod_usuario_registro },
                { "cod_tipo_trabajador_salud", eTrab.cod_tipo_trabajador_salud },
                { "cod_sctr_salud", eTrab.cod_sctr_salud },
                { "cod_sctr_salud_pension", eTrab.cod_sctr_salud_pension },
                { "cod_situacion_trabajador_salud", eTrab.cod_situacion_trabajador_salud },
                { "cod_regimen_salud", eTrab.cod_regimen_salud },
                { "cod_entidadeps", eTrab.cod_entidadeps }

            };
            if (eTrab.fch_scrtsalud.ToString().Contains("1/01/0001")) { dictionary.Add("fch_scrtsalud", DBNull.Value); } else { dictionary.Add("fch_scrtsalud", eTrab.fch_scrtsalud); }
            if (eTrab.fch_scrtpension.ToString().Contains("1/01/0001")) { dictionary.Add("fch_scrtpension", DBNull.Value); } else { dictionary.Add("fch_scrtpension", eTrab.fch_scrtpension); }

            obj = _sql.ConsultarEntidad<T>("usp_Insertar_Actualizar_InfoSaludTrabajador", dictionary);
            return obj;
        }


        public T InsertarActualizar_CertificadoEMOTrabajador<T>(eTrabajador.eCertificadoEMO_Trabajador eTrab) where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "cod_trabajador", eTrab.cod_trabajador },
                { "cod_empresa", eTrab.cod_empresa },
                { "cod_EMO", eTrab.cod_EMO },
                { "dsc_descripcion", eTrab.dsc_descripcion },
                { "fch_registro", eTrab.fch_registro },
                { "dsc_anho", eTrab.dsc_anho },
                { "flg_certificado", eTrab.flg_certificado },
                { "id_certificado", eTrab.id_certificado },
            };

            obj = _sql.ConsultarEntidad<T>("usp_Insertar_Actualizar_CertificadoEMOTrabajador", dictionary);
            return obj;
        }

        public T InsertarActualizar_InfoViviendaTrabajador<T>(eTrabajador.eInfoVivienda_Trabajador eTrab) where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "cod_trabajador", eTrab.cod_trabajador },
                { "cod_empresa", eTrab.cod_empresa },
                { "cod_infovivienda", eTrab.cod_infovivienda },
                { "flg_puertas", eTrab.flg_puertas },
                { "cod_tipovivienda", eTrab.cod_tipovivienda },
                { "flg_ventanas", eTrab.flg_ventanas },
                { "flg_techo", eTrab.flg_techo },
                { "flg_telefono", eTrab.flg_telefono },
                { "flg_celulares", eTrab.flg_celulares },
                { "flg_internet_comunicacion", eTrab.flg_internet_comunicacion },
                { "cod_tipocomodidad", eTrab.cod_tipocomodidad },
                { "flg_luz", eTrab.flg_luz },
                { "flg_agua", eTrab.flg_agua },
                { "flg_desague", eTrab.flg_desague },
                { "flg_gas", eTrab.flg_gas },
                { "flg_cable", eTrab.flg_cable },
                { "flg_internet_servicio", eTrab.flg_internet_servicio },
                { "dsc_viaacceso", eTrab.dsc_viaacceso },
                { "dsc_iluminacion", eTrab.dsc_iluminacion },
                { "dsc_entorno", eTrab.dsc_entorno },
                { "dsc_puestopolicial", eTrab.dsc_puestopolicial },
                { "dsc_nombre_familiar", eTrab.dsc_nombre_familiar },
                { "dsc_horasencasa", eTrab.dsc_horasencasa },
                { "cod_parentesco", eTrab.cod_parentesco },
                { "dsc_celular", eTrab.dsc_celular },
                { "dsc_mail", eTrab.dsc_mail },
                { "cod_usuario_registro", eTrab.cod_usuario_registro },
            };

            obj = _sql.ConsultarEntidad<T>("usp_Insertar_Actualizar_InfoViviendaTrabajador", dictionary);
            return obj;
        }
        public List<T> ListarControlHoras<T>(int opcion, string @cod_usuario = "", string @cod_empresa = "", string @fechaInicio = "", string @fechaFin = "", string @cod_empresa_multiple = "") where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> oDictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion },
                { "cod_usuario", @cod_usuario },
                { "fechaInicio", @fechaInicio },
                { "fechaFin", @fechaFin },
                { "cod_empresa", @cod_empresa },
                { "cod_empresa_multiple", @cod_empresa_multiple }
            };

            myList = _sql.ListaconSP<T>("usp_Consulta_ListarControlHoras", oDictionary);
            return myList;
        }

        public T Insertar_Actualizar_AreaEmpresa<T>(eTrabajador.eArea eTrab) where T : class, new()
        {

            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            {
                dictionary.Add("cod_empresa", eTrab.cod_empresa);
                dictionary.Add("cod_sede_empresa", eTrab.cod_sede_empresa);
                dictionary.Add("cod_area", eTrab.cod_area);
                dictionary.Add("dsc_area", eTrab.dsc_area);
                dictionary.Add("flg_activo", eTrab.flg_activo);
            }
            obj = _sql.ConsultarEntidad<T>("usp_rhu_Insertar_Actualizar_Areas", dictionary);
            return obj;
        }

        public List<T> ListarAreas<T>(int opcion, string cod_empresa = "", string cod_sede_empresa = "", string cod_area = "", string cod_cargo = "") where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> oDictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion },
                { "cod_empresa", cod_empresa },
                { "cod_sede_empresa", cod_sede_empresa },
                { "cod_area", cod_area },
                { "cod_cargo", cod_cargo },

            };

            myList = _sql.ListaconSP<T>("usp_ConsultasVarias_Trabajador", oDictionary);
            return myList;
        }

        public List<T> ListarArea<T>(int opcion, string cod_empresa = "", string cod_sede_empresa = "", string flg_varios = "", string cod_documento = "") where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> oDictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion },
                 { "cod_empresa", cod_empresa },
                  { "cod_sede_empresa", cod_sede_empresa },
                {"flg_varios",flg_varios },
                {"cod_documento",cod_documento }


            };

            myList = _sql.ListaconSP<T>("usp_ConsultasVarias_Trabajador", oDictionary);
            return myList;
        }


        public T InsertarActualizar_EMOTrabajador<T>(eTrabajador.eEMO eTrab, int opcion = 1, string flg_observacion = "APTO") where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "opcion",opcion },
                { "cod_trabajador", eTrab.cod_trabajador },
                { "cod_empresa", eTrab.cod_empresa },
                { "cod_EMO", eTrab.cod_EMO },
                //{ "cod_infosalud", eTrab.cod_infosalud },
                {"dsc_observacion", eTrab.dsc_observacion },
                {"flg_observacion", eTrab.flg_observacion },
                {"dsc_anho", eTrab.dsc_anho },
                //{"fch_evaluacion_obs", eTrab.fch_evaluacion_obs },
                {"fch_evaluacion", eTrab.fch_evaluacion },
                {"fch_registro",eTrab.fch_registro },
                {"cod_usuario_registro",eTrab.cod_usuario_registro },
                {"dsc_descripcion",eTrab.dsc_descripcion },
                { "flg_certificado", eTrab.flg_certificado },
                { "id_certificado", eTrab.id_certificado },
                { "cod_documento", eTrab.cod_documento },
                { "nombre_archivoonedrive", eTrab.nombre_archivoonedrive }
            };
            if (eTrab.fch_evaluacion_obs.ToString().Contains("1/01/0001")) { dictionary.Add("fch_evaluacion_obs", DBNull.Value); } else { dictionary.Add("fch_evaluacion_obs", eTrab.fch_evaluacion_obs); }

            obj = _sql.ConsultarEntidad<T>("usp_Insertar_Actualizar_EMOtrabajador", dictionary);
            return obj;
        }

        public T InsertarActualizar_EMO<T>(eTrabajador.eEMO eTrab) where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "cod_trabajador", eTrab.cod_trabajador },
                { "cod_empresa", eTrab.cod_empresa },
                { "cod_EMO", eTrab.cod_EMO },
                { "dsc_descripcion", eTrab.dsc_descripcion },
                { "fch_registro", eTrab.fch_registro },
                { "dsc_anho", eTrab.dsc_anho },
                { "flg_certificado", eTrab.flg_certificado },
                { "id_certificado", eTrab.id_certificado }
            };

            obj = _sql.ConsultarEntidad<T>("usp_Insertar_Actualizar_EMOtrabajador", dictionary);
            return obj;
        }

        public T ObtenerCargos<T>(int opcion) where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion }
            };

            obj = _sql.ConsultarEntidad<T>("usp_ConsultasVarias_Cargos", dictionary);
            return obj;
        }

        public T ObtenerUltimoCargo<T>(int opcion, string cod_trabajador, string cod_empresa, string cod_cargo) where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion },
                { "cod_trabajador", cod_trabajador },
                { "cod_empresa", cod_empresa },
                {"cod_cargo", cod_cargo }
            };

            obj = _sql.ConsultarEntidad<T>("usp_rhu_Consulta_ListarTrabajador", dictionary);
            return obj;
        }

        public List<T> ListarTrabajadoresvista<T>(int opcion, string cod_trabajador, string cod_empresa, string cod_empresa_multiple = "", string cod_sede_multiple = "", string flg_activo = "") where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> oDictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion },
                { "cod_trabajador", cod_trabajador },
                { "cod_empresa", cod_empresa },
                { "cod_empresa_multiple", cod_empresa_multiple },
                { "cod_sede_multiple", cod_sede_multiple },
                {"flg_activo",flg_activo }

            };

            myList = _sql.ListaconSP<T>("usp_rhu_Consulta_ListarTrabajador", oDictionary);
            return myList;
        }

        public T Insertar_Actualizar_Cargos<T>(eTrabajador.eCargos eTrab) where T : class, new()
        {

            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            {
                dictionary.Add("cod_empresa", eTrab.cod_empresa);
                dictionary.Add("cod_sede_empresa", eTrab.cod_sede_empresa);
                dictionary.Add("cod_cargo", eTrab.cod_cargo);
                dictionary.Add("cod_area", eTrab.cod_area);
                dictionary.Add("dsc_cargo", eTrab.dsc_cargo);
                dictionary.Add("flg_activo", eTrab.flg_activo);
                dictionary.Add("flg_firma_documento", eTrab.flg_firma_documento);
                dictionary.Add("remplazo", eTrab.remplazo);
            }
            obj = _sql.ConsultarEntidad<T>("usp_rhu_Insertar_Actualizar_Cargos", dictionary);
            return obj;
        }

        public List<T> ListarFormatos<T>(int opcion, string cod_empresa, string cod_trabajador_multiple = "") where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> oDictionary = new Dictionary<string, object>()
            {
                {"opcion", opcion },
                {"cod_empresa", cod_empresa},
                {"cod_trabajador_multiple", cod_trabajador_multiple}
            };

            myList = _sql.ListaconSP<T>("usp_rhu_Consulta_ListarFormatosTrabajador", oDictionary);
            return myList;
        }

        public T Validar_NroDocumento<T>(int opcion, string num_documento = "", string cod_tipo_documento = "") where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                {"opcion", opcion }, {"num_documento", num_documento },
                {"cod_tipo_documento", cod_tipo_documento }
            };

            obj = _sql.ConsultarEntidad<T>("usp_ConsultasVarias_Trabajador", dictionary);
            return obj;
        }

        public T Obtener_cod_trabajador<T>(int opcion, string cod_empresa = "", string dsc_documento = "", string cod_trabajador = "", string cod_sede_empresa = "", string cod_area = "", string cod_cargo = "") where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion },
                { "cod_empresa", cod_empresa },
                { "dsc_documento", dsc_documento },
                { "cod_trabajador", cod_trabajador },
                { "cod_sede_empresa", cod_sede_empresa },
                { "cod_area", cod_area },
                { "cod_cargo", cod_cargo },


            };

            obj = _sql.ConsultarEntidad<T>("usp_ConsultasVarias_Trabajador", dictionary);
            return obj;
        }

        public T Obtener_cantidad_doc<T>(int opcion, string cod_tipo_documento) where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion },
                { "cod_tipo_documento", cod_tipo_documento }
            };

            obj = _sql.ConsultarEntidad<T>("usp_ConsultasVarias_Trabajador", dictionary);
            return obj;
        }

        public List<T> ListarEMO<T>(int opcion, string cod_trabajador = "", string cod_empresa = "", string dsc_documento = "") where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> oDictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion },
                { "cod_trabajador", cod_trabajador },
                { "cod_empresa", cod_empresa },
                { "dsc_documento", dsc_documento },


            };

            myList = _sql.ListaconSP<T>("usp_Consulta_ListarTrabajador", oDictionary);
            return myList;
        }


        public string EliminarInactivarEMOtrabajador(int opcion, string id_certificado = "", string dsc_descripcion = "", string cod_trabajador = "", string cod_empresa = "", int cod_EMO = 0, string Fechabaja = "", string motivobaja = "", string observaciones = "", string cod_documento = "")
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("opcion", opcion);
            dictionary.Add("cod_trabajador", cod_trabajador);
            dictionary.Add("cod_empresa", cod_empresa);
            dictionary.Add("cod_EMO", cod_EMO);
            dictionary.Add("Fechabaja", Fechabaja);
            dictionary.Add("motivo_baja", motivobaja);
            dictionary.Add("observaciones", observaciones);
            dictionary.Add("cod_documento", cod_documento);
            dictionary.Add("id_certificado", id_certificado);
            dictionary.Add("dsc_descripcion", dsc_descripcion);


            string result;
            result = _sql.ExecuteScalarWithParams("usp_Eliminar_EMOtrabajador", dictionary);

            return result;
        }

        //public T ActualizarEMO<T>(int opcion,eTrabajador eTrab , string cod_empresa="", string cod_trabajador="") where T : class, new()
        //{

        //    T obj = new T();
        //    Dictionary<string, object> dictionary = new Dictionary<string, object>();
        //    {
        //        dictionary.Add("opcion", opcion);
        //        dictionary.Add("Fechabaja", eTrab.Fechabaja);
        //        dictionary.Add("motivobaja", eTrab.motivobaja);
        //        dictionary.Add("observaciones", eTrab.observaciones);
        //        dictionary.Add("cod_trabajador_multiple", eTrab.cod_trabajador);
        //        dictionary.Add("cod_empresa", eTrab.cod_empresa);

        //    }
        //    obj = _sql.ConsultarEntidad<T>("usp_Eliminar_EMOtrabajador", dictionary);
        //    return obj;
        //}

        public string ActualizarBaja(int opcion, DateTime Fechabaja, string motivo_baja = "", string observaciones = "", string cod_empresa = "", string cod_trabajador = "", string flg_activo = "", string motivo_bajainterna="",string black_list="")
        {


            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            {
                dictionary.Add("opcion", opcion);
                dictionary.Add("motivo_baja", motivo_baja);
                dictionary.Add("observaciones", observaciones);
                dictionary.Add("cod_trabajador_multiple", cod_trabajador);
                dictionary.Add("cod_empresa", cod_empresa);
                dictionary.Add("flg_activo", flg_activo);
                dictionary.Add("motivo_bajainterna", motivo_bajainterna);
                dictionary.Add("black_list", black_list);

                if (Fechabaja.ToString().Contains("0001")) { dictionary.Add("Fechabaja", DBNull.Value); } else { dictionary.Add("Fechabaja", Fechabaja); }
                string result;

                result = _sql.ExecuteScalarWithParams("usp_Eliminar_EMOtrabajador", dictionary);

                return result;
            }

        }



        public T Validardni<T>(int opcion, string dsc_documento = "") where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion },
                { "dsc_documento", dsc_documento }
            };

            obj = _sql.ConsultarEntidad<T>("usp_Consulta_ListarTrabajador", dictionary);
            return obj;
        }

        public T InsertarMasivo<T>(int opcion, eTrabajador eTrab) where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion},
                { "cod_trabajador", eTrab.cod_trabajador },
                { "cod_empresa", eTrab.cod_empresa },
                { "dsc_documento", eTrab.dsc_documento },
                { "dsc_nombres", eTrab.dsc_nombres },

            };

            obj = _sql.ConsultarEntidad<T>("Usp_rhu_Insertar_RegistroPlacar", dictionary);
            return obj;
        }
        public List<T> DocumentoTabla<T>(int opcion, string cod_empresa = "") where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> oDictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion },
                { "cod_empresa", cod_empresa }
            };

            myList = _sql.ListaconSP<T>("usp_ConsultasVarias_Trabajador", oDictionary);
            return myList;


        }
        public List<T> ListaDocuemtos<T>(int opcion, string cod_empresa = "", string cod_trabajador = "") where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> oDictionary = new Dictionary<string, object>() {
                { "opcion", opcion}, { "cod_empresa", cod_empresa}, { "cod_trabajador", cod_trabajador}
            };
            myList = _sql.ListaconSP<T>("usp_rhu_Consulta_ListarTrabajador", oDictionary);
            return myList;
        }

        public T Obtener_Familiar<T>(int opcion, string cod_trabajador, string cod_empresa, int cod_infofamiliar) where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion },
                { "cod_trabajador", cod_trabajador },
                { "cod_empresa", cod_empresa },
                { "cod_infofamiliar", cod_infofamiliar }
            };

            obj = _sql.ConsultarEntidad<T>("usp_Consulta_ListarTrabajador", dictionary);
            return obj;
        }
        public string ActualizarInformacionDocumentosFamilia(string flg_Adjuntos, int opcionDoc, eTrabajador.eInfoFamiliar_Trabajador objTrab)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("cod_trabajador", objTrab.cod_trabajador);
            dictionary.Add("cod_empresa", objTrab.cod_empresa);
            dictionary.Add("cod_infofamiliar", objTrab.cod_infofamiliar);
            dictionary.Add("flg_Adjuntos", flg_Adjuntos);
            dictionary.Add("opcionDoc", opcionDoc);
            dictionary.Add("idCarpeta_Trabajador", objTrab.idCarpeta_Trabajador);
            dictionary.Add("id_DNI_documentofam", objTrab.id_DNI_documentofam);
            dictionary.Add("flg_DNI_documentofam", flg_Adjuntos);
            dictionary.Add("flg_CERTIFICADOESTUDIOS_documentofam", flg_Adjuntos);
            dictionary.Add("id_CERTIFICADOESTUDIOS_documentofam", objTrab.id_CERTIFICADOESTUDIOS_documentofam);


            string result;
            result = _sql.ExecuteScalarWithParams("usp_Insertar_Actualizar_InfoFamiliarTrabajador", dictionary);

            return result;
        }
        public List<T> ListarUbigedo<T>(int opcion, string cod_departamento = "", string cod_empresa = "", string cod_tipo_via = "", string cod_distrito = "", string cod_provincia = "", string dsc_documento = "", string cod_sede_empresa = "", string cod_area = "", string cod_sunat = "", string cod_usuario="") where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> oDictionary = new Dictionary<string, object>()
            {
                {"opcion", opcion },
                {"cod_tipo_via", cod_tipo_via },
                {"cod_departamento", cod_departamento },
                {"cod_provincia", cod_provincia },
                {"cod_distrito", cod_distrito },
                {"dsc_documento", dsc_documento },
                {"cod_empresa", cod_empresa },
                {"cod_sede_empresa", cod_sede_empresa },
                {"cod_area", cod_area },
                {"cod_sunat", cod_sunat },
                {"cod_usuario", cod_usuario }

            };

            myList = _sql.ListaconSP<T>("usp_ConsultasVarias_Trabajador", oDictionary);
            return myList;
        }
        public T ObtenerDatos<T>(int opcion, string cod_tipo_via = "", string cod_tipo_zona = "", string cod_pais="",string cod_departamento = "", string cod_provincia = "", string cod_distrito = "") where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion },
                {"cod_tipo_via", cod_tipo_via },
                {"cod_tipo_zona", cod_tipo_zona },
                {"cod_pais", cod_pais },
                {"cod_departamento", cod_departamento },
                {"cod_provincia", cod_provincia },
                {"cod_distrito", cod_distrito }


            };

            obj = _sql.ConsultarEntidad<T>("usp_ConsultasVarias_Trabajador", dictionary);
            return obj;
        }


        public List<T> CombosEnGridControl<T>(string nCombo, string dato = "", string cod_tipo_documento = "", string cod_empresa = "", string cod_departamento = "", string cod_pais = "", string cod_usuario = "", string cod_provincia = "", string cod_distrito = "", string flg_varios = "",string cod_sede_empresa="",string cod_area="") where T : class, new()
        {
            List<T> myList = new List<T>();
            string procedure = "usp_ConsultasVarias_Trabajador";
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            //dictionary.Add("dato", dato);

            switch (nCombo)
            {
                case "TipoDocumento":
                    dictionary.Add("opcion", 59);
                    break;
                case "EmpresaUsuario":
                    dictionary.Add("opcion", 82);
                    dictionary.Add("cod_usuario", cod_usuario);
                    break;
                case "Nacionalidad":
                    dictionary.Add("opcion", 38);
                    break;
                case "Sexo":
                    dictionary.Add("opcion", 39);
                    break;
                case "Pais":
                    dictionary.Add("opcion", 40);
                    break;
                case "Departamento":
                    dictionary.Add("opcion", 41);
                    dictionary.Add("cod_pais", "00001");
                    break;
                case "Provincia":
                    dictionary.Add("opcion", 42);
                    dictionary.Add("cod_pais", "00001");
                    dictionary.Add("cod_departamento", cod_departamento);
                    break;
                case "Distrito":
                    dictionary.Add("opcion", 43);
                    dictionary.Add("cod_pais", cod_pais);
                    dictionary.Add("cod_departamento", cod_departamento);
                    dictionary.Add("cod_provincia", cod_provincia);
                    break;
                case "EstadoCivil":
                    dictionary.Add("opcion", 37);
                    break;
                case "Tipovia":
                    dictionary.Add("opcion", 44);
                    break;
                case "TipoZona":
                    dictionary.Add("opcion", 45);
                    break;
                case "EstadoEMO":
                    dictionary.Add("opcion", 105);
                    break;
                case "TipoDoc":
                    dictionary.Add("opcion", 106);
                    dictionary.Add("flg_varios", flg_varios);
                    dictionary.Add("cod_empresa", cod_empresa);
                    break;
                case "sedeempregrid":
                    dictionary.Add("opcion", 6);
                    dictionary.Add("cod_empresa", cod_empresa);
                    break;
                case "AreaEmpresa":
                    dictionary.Add("opcion", 7);
                    dictionary.Add("cod_empresa", cod_empresa);
                    dictionary.Add("cod_sede_empresa", cod_sede_empresa);
                    break;
                case "CargoEmpresa":
                    dictionary.Add("opcion", 8);
                    dictionary.Add("cod_empresa", cod_empresa);
                    dictionary.Add("cod_sede_empresa", cod_sede_empresa);
                    dictionary.Add("cod_area", cod_area);
                    break;
                case "Sino":
                    dictionary.Add("opcion", 63);
                    break;
                case "DistribucionCECO":
                    procedure = "usp_ConsultasVarias_FacturasProveedor";
                    dictionary.Add("opcion", 32);
                    dictionary.Add("cod_empresa", cod_empresa);
                    break;
            }
            myList = _sql.ListaconSP<T>(procedure, dictionary);
            return myList;
        }

        public T Validar_Perfil<T>(int opcion, string num_documento = "", string cod_tipo_documento = "") where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                {"opcion", opcion }, {"num_documento", num_documento },
                {"cod_tipo_documento", cod_tipo_documento }
            };

            obj = _sql.ConsultarEntidad<T>("usp_ConsultasVarias_Trabajador", dictionary);
            return obj;
        }

        public T Obtener_meses<T>(int opcion, string cod_tipoContrato, string cod_ModContrato) where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion },
                { "cod_tipoContrato", cod_tipoContrato },
                 { "cod_ModContrato", cod_ModContrato },
            };
            obj = _sql.ConsultarEntidad<T>("usp_ConsultasVarias_Trabajador", dictionary);
            return obj;
        }
        public string ActualizarDocumentoInfolab(int opcion, string id_contrato = "", int cod_infolab = 0, string cod_empresa = "", string cod_trabajador = "", string flg_contrato = "")
        {

            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            {
                dictionary.Add("opcion", opcion);
                dictionary.Add("flg_contrato", flg_contrato);
                dictionary.Add("id_contrato", id_contrato);
                dictionary.Add("cod_infolab", cod_infolab);
                dictionary.Add("cod_empresa", cod_empresa);
                dictionary.Add("cod_trabajador", cod_trabajador);
                string result;
                result = _sql.ExecuteScalarWithParams("usp_Eliminar_EMOtrabajador", dictionary);

                return result;
            }

        }
        public void pDatosAExcel(string Coneccion, Excel.Application xls, string cadSql, string Nombre, string celda, bool AjustarColumnas = false, bool NoCab = false)
        {
            {
                var withBlock = xls.ActiveSheet.QueryTables.Add(Connection: Coneccion
                  , Destination: xls.Range[celda]);
                withBlock.CommandText = cadSql;
                withBlock.Name = Nombre;
                withBlock.FieldNames = !(NoCab); // True
                withBlock.RowNumbers = false;
                withBlock.FillAdjacentFormulas = false;
                withBlock.PreserveFormatting = true;
                withBlock.RefreshOnFileOpen = false;
                withBlock.BackgroundQuery = true;
                withBlock.SavePassword = true;
                withBlock.SaveData = true;
                if (AjustarColumnas == true)
                    withBlock.AdjustColumnWidth = true;
                withBlock.RefreshPeriod = 0;
                withBlock.PreserveColumnInfo = true;
                withBlock.Refresh(BackgroundQuery: false);
            }
        }

        public List<T> LISTARCECO<T>(int opcion, string cod_empresa = "") where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> oDictionary = new Dictionary<string, object>()
            {
                {"opcion", opcion },
                {"cod_empresa", cod_empresa },

            };

            myList = _sql.ListaconSP<T>("usp_ConsultasVarias_FacturasProveedor", oDictionary);
            return myList;
        }

        public T InsertarActualizar_DocumentoIngreso<T>(eTrabajador.eDocumento_Trabajador eTrab, string flg_observacion = "APTO") where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "cod_item", eTrab.cod_item },
                { "cod_trabajador", eTrab.cod_trabajador },
                { "cod_empresa", eTrab.cod_empresa },
                { "flg_resultados", eTrab.flg_resultados },
                { "flg_documento", eTrab.flg_documento },
                { "id_documento", eTrab.id_documento },
                { "dsc_archivo", eTrab.dsc_archivo },
                 { "cod_documento", eTrab.cod_documento },
                { "fch_registro", eTrab.fch_registro },
                { "cod_usuario_registro", eTrab.cod_usuario_registro },

            };
            obj = _sql.ConsultarEntidad<T>("usp_Insertar_Actualizar_DocumentosIngresos", dictionary);
            return obj;
        }


        public T Insertar_ActualizarDocumentos<T>(eTrabajador.eDocumento_Trabajador eTrab) where T : class, new()
        {

            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            {
                dictionary.Add("iddoc", eTrab.iddoc);
                dictionary.Add("dsc_documento", eTrab.dsc_documento);
                dictionary.Add("flg_activo", eTrab.flg_activo);
                dictionary.Add("flg_varios", eTrab.flg_varios);
                dictionary.Add("remplazo", eTrab.remplazo);
                dictionary.Add("dsc_abreviatura", eTrab.dsc_abreviatura);
                dictionary.Add("cod_empresa", eTrab.cod_empresa);
            }
            obj = _sql.ConsultarEntidad<T>("usp_rhu_Insertar_Actualizar_DocumentosIngresos", dictionary);
            return obj;
        }

        public T Obtener_emo<T>(int opcion, string cod_trabajador = "", string cod_empresa = "", int cod_EMO = 0, int cod_item = 0, string cod_documento = "", string id_certificado = "") where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion },
                { "cod_trabajador", cod_trabajador },
                { "cod_empresa", cod_empresa },
                { "cod_documento", cod_documento },
                { "cod_EMO", cod_EMO }
            };
            obj = _sql.ConsultarEntidad<T>("usp_ConsultasVarias_Trabajador", dictionary);
            return obj;
        }

        public List<T> ListarTrabajadorMasivo<T>(int opcion, string cod_trabajador = "", string cod_empresa = "", string dsc_documento = "",string cod_trabajador_multiple="") where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> oDictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion },
                { "cod_trabajador", cod_trabajador },
                { "cod_empresa", cod_empresa },
                { "dsc_documento", dsc_documento },
                {"cod_trabajador_multiple", cod_trabajador_multiple}

            };

            myList = _sql.ListaconSP<T>("usp_ConsultasVarias_Trabajador", oDictionary);
            return myList;
        }
        public T ObtenerRemuneracionminimavital<T>(int opcion) where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion }
            };
            obj = _sql.ConsultarEntidad<T>("usp_ConsultasVarias_Trabajador", dictionary);
            return obj;
        }

        public List<T> ListarMenuFormatoContable<T>() where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> oDictionary = new Dictionary<string, object>()
            {
               

            };

            myList = _sql.ListaconSP<T>("usp_rhu_Formato_ArchivosContables_Listado", oDictionary);
            return myList;
        }
        public T InsertarActualizarMasivo_Trabajador<T>(eTrabajadorMasivo eTrab) where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "cod_trabajador", eTrab.cod_trabajador },
                { "cod_usuario", eTrab.dsc_usuario },
                { "cod_empresa", eTrab.dsc_empresa },
                { "dsc_apellido_paterno", eTrab.dsc_apellido_paterno },
                { "dsc_apellido_materno", eTrab.dsc_apellido_materno },
                { "dsc_nombres", eTrab.dsc_nombres },
                { "cod_estadocivil", eTrab.cod_estado_civil },
                { "fch_nacimiento", eTrab.fch_nacimiento },
                { "flg_sexo", eTrab.flg_sexo },
                { "cod_tipo_documento", eTrab.cod_tipo_documento },
                { "dsc_documento", eTrab.dsc_documento },
                { "cod_nacionalidad", eTrab.cod_nacionalidad },
                { "cod_distrito", eTrab.cod_distrito },
                { "cod_provincia", eTrab.cod_provincia },
                { "cod_departamento", eTrab.cod_departamento },
                {"dsc_lote",eTrab.cod_lote},
                {"cod_tipo_via",eTrab.cod_tipo_via },
                {"dsc_tipo_via",eTrab.dsc_tipo_via},
                {"dsc_nro_vivienda",eTrab.dsc_nro_vivienda },
                {"dsc_departamento_dir",eTrab.dsc_departamento_dir},
                {"dsc_interior",eTrab.dsc_interior},
                {"dsc_manzana",eTrab.dsc_manzana},
                {"dsc_etapa",eTrab.dsc_etapa },
                {"dsc_block",eTrab.dsc_block},
                {"dsc_kilometro_dir",eTrab.dsc_km},
                {"dsc_tipo_zona",eTrab.cod_tipozona},
                {"dsc_nombre_zona",eTrab.dsc_nombre_zona},
                { "dsc_mail_1", eTrab.dsc_mail_1 },
                { "dsc_telefono", eTrab.dsc_telefono },
                { "dsc_celular", eTrab.dsc_celular },
                { "cod_sist_pension", eTrab.cod_sist_pension },
                { "cod_APF", eTrab.cod_APF },
                { "cod_CUSPP", eTrab.cod_CUSPP },

            };

            obj = _sql.ConsultarEntidad<T>("usp_Insertar_Actualizar_Trabajador", dictionary);
            return obj;
        }

        public T Obtener_Trabajador_academico<T>(int opcion, string cod_trabajador = "", string cod_empresa = "") where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion },
                { "cod_trabajador", cod_trabajador },
                { "cod_empresa", cod_empresa },
            };

            obj = _sql.ConsultarEntidad<T>("usp_Consulta_ListarTrabajador", dictionary);
            return obj;
        }
    }
}

