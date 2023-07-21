using BE_GestionRRHH;
using System.Collections.Generic;
internal class AppSesion
{
    private eUsuario _usuario;
    private eColor _colores;
    private eGlobales _global;
    private Acceso _acceso;
    private Acceso _solucionAbrir;
    private List<eProveedor_Empresas> _empresaList;

    public AppSesion(Acceso acceso) { _acceso = acceso; _global = new eGlobales() { Entorno = _acceso.Entorno }; }

    internal void SetOpenAccess(Acceso acceso) => _solucionAbrir = new Acceso(acceso.Solucion, acceso.Token, acceso.User, acceso.Key, acceso.Entorno);
    internal void SetUser(eUsuario usuario) => _usuario = usuario;
    internal void SetColor(eColor colores) => _colores = colores;
    internal void SetGlobal(eGlobales global) => _global = global;
    internal void SetEmpresaList(List<eProveedor_Empresas> empresaList) => _empresaList = empresaList;

    public eUsuario Usuario { get => _usuario; }// private set => _usuario = value; }
    public eColor Colores { get => _colores; }//private set => _colores = value; }
    public eGlobales Global { get => _global; }// private set => _global = value; }
    public Acceso Acceso { get => _acceso; }//private set => _acceso = value; }
    public Acceso SolucionAbrir { get => _solucionAbrir; }// private set => _solucionAbrir = value; }
    public List<eProveedor_Empresas> EmpresaList { get => _empresaList; }// private set => _empresaList = value; }
    public string Version { get; set; }
}
internal class Acceso
{
    private string _solucion;
    private string _token;
    private string _user;
    private string _key;
    private string _entorno;

    public Acceso(string solucion, string token, string user, string key, string entorno)
    { _solucion = solucion; _token = token; _user = user; _key = key; _entorno = entorno; }

    public string Solucion { get => _solucion; private set => _solucion = value; }
    public string Token { get => _token; private set => _token = value; }
    public string User { get => _user; private set => _user = value; }
    public string Key { get => _key; private set => _key = value; }
    public string Entorno { get => _entorno; private set => _entorno = value; }
}
