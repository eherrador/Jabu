using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.IO;
using System.Text;

namespace WebAppMvcJabu.Models
{
    public class JabuConnectionString
    {
        public string JabuCnnString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["BusinessConnection"].ToString();
            }
        }
    }

    public class Estado
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public List<Estado> ObtenerEstados()
        {
            try
            {
                List<Estado> listaEstados = new List<Estado>();

                JabuConnectionString jdbConnString = new JabuConnectionString();

                using (SqlConnection cnn = new SqlConnection(jdbConnString.JabuCnnString))
                {
                    SqlCommand cmd = cnn.CreateCommand();
                    cmd.CommandText = "usp_Busqueda_Get_Estados";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cnn.Open();

                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        Estado estado = new Estado();
                        estado.Id = dr.GetInt32(dr.GetOrdinal("Estado_Id"));
                        estado.Nombre = dr.GetString(dr.GetOrdinal("Estado_Nombre"));

                        listaEstados.Add(estado);
                    }
                }

                Utils.WriteLogMessage(HttpContext.Current.User.Identity.Name, "Acceso a Base de Datos. Se obtiene lista de Estados.");
                return listaEstados;
            }
            catch (Exception ex)
            {
                Utils.WriteLogMessage(HttpContext.Current.User.Identity.Name, ex.Message);
                return null;
            }
        }
    }

    public class Demarcacion
    {
        public int Id { get; set; }

        public int Estado { get; set; }

        public string Nombre { get; set; }

        public List<Demarcacion> ObtenerDemarcaciones(int id_estado)
        {
            try
            {
                List<Demarcacion> listaDemarcaciones = new List<Demarcacion>();

                JabuConnectionString jdbConnString = new JabuConnectionString();

                using (SqlConnection cnn = new SqlConnection(jdbConnString.JabuCnnString))
                {
                    SqlCommand cmd = cnn.CreateCommand();
                    cmd.CommandText = "usp_Busqueda_Get_Demarcaciones";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id_Estado", id_estado));

                    cnn.Open();

                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        Demarcacion demarcacion = new Demarcacion();
                        demarcacion.Id = dr.GetInt32(dr.GetOrdinal("Demarcacion_Id"));
                        demarcacion.Estado = dr.GetInt32(dr.GetOrdinal("Demarcacion_Estado"));
                        demarcacion.Nombre = dr.GetString(dr.GetOrdinal("Demarcacion_Nombre"));

                        listaDemarcaciones.Add(demarcacion);
                    }
                }

                Utils.WriteLogMessage(HttpContext.Current.User.Identity.Name, "Acceso a Base de Datos. Se obtiene lista de Demarcaciones.");
                return listaDemarcaciones;
            }
            catch (Exception ex)
            {
                Utils.WriteLogMessage(HttpContext.Current.User.Identity.Name, ex.Message);
                return null;
            }
        }
    }

    public class Segmento
    {
        public int Id { get; set; }

        public string Descripcion { get; set; }

        public decimal MinimoEnPesos { get; set; }

        public decimal MaximoEnPesos { get; set; }

        public List<Segmento> ObtenerSegmentos()
        {
            try
            {
                List<Segmento> listaSegmentos = new List<Segmento>();

                JabuConnectionString jdbConnString = new JabuConnectionString();

                using (SqlConnection cnn = new SqlConnection(jdbConnString.JabuCnnString))
                {
                    SqlCommand cmd = cnn.CreateCommand();
                    cmd.CommandText = "usp_Busqueda_Get_Segmentos";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ClasificacionAnio", DateTime.Now.Year));

                    cnn.Open();

                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        Segmento segmento = new Segmento();
                        segmento.Id = dr.GetInt32(dr.GetOrdinal("Clasificacion_Id"));
                        segmento.Descripcion = dr.GetString(dr.GetOrdinal("Clasificacion_Descripcion"));
                        segmento.MinimoEnPesos = dr.GetDecimal(dr.GetOrdinal("ClasificacionCosto_EnPesos_Min"));
                        segmento.MaximoEnPesos = dr.GetDecimal(dr.GetOrdinal("ClasificacionCosto_EnPesos_Max"));

                        listaSegmentos.Add(segmento);
                    }
                }

                Utils.WriteLogMessage(HttpContext.Current.User.Identity.Name, "Acceso a Base de Datos. Se obtiene lista de Segmentos.");
                return listaSegmentos;
            }
            catch (Exception ex)
            {
                Utils.WriteLogMessage(HttpContext.Current.User.Identity.Name, ex.Message);
                return null;
            }
        }
    }

    public class TipoConstruccion
    {
        public int Id { get; set; }

        public string Descripcion { get; set; }

        public List<TipoConstruccion> ObtenerTiposConstruccion()
        {
            try
            {
                List<TipoConstruccion> listaTiposConstruccion = new List<TipoConstruccion>();

                JabuConnectionString jdbConnString = new JabuConnectionString();

                using (SqlConnection cnn = new SqlConnection(jdbConnString.JabuCnnString))
                {
                    SqlCommand cmd = cnn.CreateCommand();
                    cmd.CommandText = "usp_Busqueda_Get_TiposConstruccion";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cnn.Open();

                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        TipoConstruccion tipoConstruccion = new TipoConstruccion();
                        tipoConstruccion.Id = dr.GetInt32(dr.GetOrdinal("TipoConstruccion_Id"));
                        tipoConstruccion.Descripcion = dr.GetString(dr.GetOrdinal("TipoConstruccion_Descripcion"));

                        listaTiposConstruccion.Add(tipoConstruccion);
                    }
                }

                Utils.WriteLogMessage(HttpContext.Current.User.Identity.Name, "Acceso a Base de Datos. Se obtiene lista de Tipos de Construcción.");
                return listaTiposConstruccion;
            }
            catch (Exception ex)
            {
                Utils.WriteLogMessage(HttpContext.Current.User.Identity.Name, ex.Message);
                return null;
            }
        }
    }

    public class Desarrollo
    {
        public string Latitud { get; set; }

        public string Longitud { get; set; }

        public int Id { get; set; }

        public string Nombre { get; set; }

        public string FechaInicioVentas { get; set; }

        public string UnidadesTotales { get; set; }

        public string UnidadesVendidas { get; set; }

        public string UnidadesDisponibles
        {
            get
            {
                int _unidadesTotales = int.Parse(UnidadesTotales);
                int _unidadesVendidas = int.Parse(UnidadesVendidas);

                int _unidadesDisponibles = _unidadesTotales - _unidadesVendidas;
                return _unidadesDisponibles.ToString();
            }
        }

        public decimal SuperficieConstruccion { get; set; }

        public string FechaActualizacionVentas { get; set; }

        public int IdSegmento { get; set; }

        public string TipoSegmento { get; set; }

        public int IdTipoConstruccion { get; set; }

        public string TipoConstruccion { get; set; }

        public int Absorcion
        {
            get
            {
                int _mesInicio = DateTime.Parse(FechaInicioVentas).Month;
                int _anioInicio = int.Parse(DateTime.Parse(FechaInicioVentas).Year.ToString().Substring(2));

                int _mesActual = DateTime.Now.Month;
                int _anioActual = int.Parse(DateTime.Now.Year.ToString().Substring(2));

                int _inicio = (_anioInicio * 12) + _mesInicio;
                int _actual = (_anioActual * 12) + _mesActual;

                int res = _actual - _inicio;
                return res;
            }
        }

        public decimal PrecioInicial { get; set; }

        public decimal PrecioActualizado { get; set; }

        public decimal PrecioMetroCuadrado
        {
            get
            {
                decimal _precioMetroCuadrado = PrecioActualizado / SuperficieConstruccion;
                return _precioMetroCuadrado;
            }
        }

        public static List<Desarrollo> ObtenerDesarrollos()
        {
            try
            {
                List<Desarrollo> listaDesarrollos = new List<Desarrollo>();

                JabuConnectionString jdbConnString = new JabuConnectionString();

                using (SqlConnection cnn = new SqlConnection(jdbConnString.JabuCnnString))
                {
                    SqlCommand cmd = cnn.CreateCommand();
                    cmd.CommandText = "usp_Busqueda_Get_Desarrollos";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cnn.Open();

                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        Desarrollo desarrollo = new Desarrollo();
                        desarrollo.Latitud = dr.GetString(dr.GetOrdinal("Desarrollo_Latitud"));
                        desarrollo.Longitud = dr.GetString(dr.GetOrdinal("Desarrollo_Longitud"));
                        desarrollo.Nombre = dr.GetString(dr.GetOrdinal("Desarrollo_Nombre"));
                        desarrollo.Id = dr.GetInt32(dr.GetOrdinal("Desarrollo_Id"));
                        desarrollo.FechaInicioVentas = dr.GetDateTime(dr.GetOrdinal("Desarrollo_InicioVentas")).Date.ToShortDateString();
                        desarrollo.UnidadesTotales = dr.GetString(dr.GetOrdinal("Desarrollo_Totales"));
                        desarrollo.UnidadesVendidas = dr.GetString(dr.GetOrdinal("Desarrollo_Vendidas"));
                        desarrollo.SuperficieConstruccion = dr.GetDecimal(dr.GetOrdinal("Producto_SuperficieConstruccion"));
                        desarrollo.FechaActualizacionVentas = dr.GetDateTime(dr.GetOrdinal("Desarrollo_ActualizacionVentas")).Date.ToShortDateString();
                        desarrollo.IdSegmento = dr.GetInt32(dr.GetOrdinal("Clasificacion_Id"));
                        desarrollo.TipoSegmento = dr.GetString(dr.GetOrdinal("Clasificacion_Descripcion"));
                        desarrollo.IdTipoConstruccion = dr.GetInt32(dr.GetOrdinal("TipoConstruccion_Id"));
                        desarrollo.TipoConstruccion = dr.GetString(dr.GetOrdinal("TipoConstruccion_Descripcion"));
                        desarrollo.PrecioInicial = dr.GetDecimal(dr.GetOrdinal("Producto_PrecioInicial"));
                        desarrollo.PrecioActualizado = dr.GetDecimal(dr.GetOrdinal("Producto_PrecioActualizado"));

                        listaDesarrollos.Add(desarrollo);
                    }
                }

                Utils.WriteLogMessage(HttpContext.Current.User.Identity.Name, "Acceso a Base de Datos. Se obtiene lista de Desarrollos.");
                return listaDesarrollos;
            }
            catch (Exception ex)
            {
                Utils.WriteLogMessage(HttpContext.Current.User.Identity.Name, ex.Message);
                return null;
            }
        }

        public List<Desarrollo> ObtenerDesarrollos(int id_estado, string ids_demarcaciones, string ids_segmentos, string ids_tipos, string ids_superficies)
        {
            try
            {
                List<Desarrollo> listaDesarrollos = new List<Desarrollo>();

                JabuConnectionString jdbConnString = new JabuConnectionString();

                using (SqlConnection cnn = new SqlConnection(jdbConnString.JabuCnnString))
                {
                    SqlCommand cmd = cnn.CreateCommand();
                    cmd.CommandText = "usp_Busqueda_Get_Desarrollos";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id_Estado", id_estado));
                    cmd.Parameters.Add(new SqlParameter("@Ids_Demarcaciones", ids_demarcaciones));
                    cmd.Parameters.Add(new SqlParameter("@Ids_Segmentos", ids_segmentos));
                    cmd.Parameters.Add(new SqlParameter("@Ids_Tipos", ids_tipos));

                    cnn.Open();

                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        Desarrollo desarrollo = new Desarrollo();
                        desarrollo.Latitud = dr.GetString(dr.GetOrdinal("Desarrollo_Latitud"));
                        desarrollo.Longitud = dr.GetString(dr.GetOrdinal("Desarrollo_Longitud"));
                        desarrollo.Nombre = dr.GetString(dr.GetOrdinal("Desarrollo_Nombre"));
                        desarrollo.Id = dr.GetInt32(dr.GetOrdinal("Desarrollo_Id"));
                        desarrollo.FechaInicioVentas = dr.GetDateTime(dr.GetOrdinal("Desarrollo_InicioVentas")).Date.ToShortDateString();
                        desarrollo.UnidadesTotales = dr.GetString(dr.GetOrdinal("Desarrollo_Totales"));
                        desarrollo.UnidadesVendidas = dr.GetString(dr.GetOrdinal("Desarrollo_Vendidas"));
                        desarrollo.SuperficieConstruccion = dr.GetDecimal(dr.GetOrdinal("Producto_SuperficieConstruccion"));
                        desarrollo.FechaActualizacionVentas = dr.GetDateTime(dr.GetOrdinal("Desarrollo_ActualizacionVentas")).Date.ToShortDateString();
                        desarrollo.IdSegmento = dr.GetInt32(dr.GetOrdinal("Clasificacion_Id"));
                        desarrollo.TipoSegmento = dr.GetString(dr.GetOrdinal("Clasificacion_Descripcion"));
                        desarrollo.IdTipoConstruccion = dr.GetInt32(dr.GetOrdinal("TipoConstruccion_Id"));
                        desarrollo.TipoConstruccion = dr.GetString(dr.GetOrdinal("TipoConstruccion_Descripcion"));
                        desarrollo.PrecioInicial = dr.GetDecimal(dr.GetOrdinal("Producto_PrecioInicial"));
                        desarrollo.PrecioActualizado = dr.GetDecimal(dr.GetOrdinal("Producto_PrecioActualizado"));

                        listaDesarrollos.Add(desarrollo);
                    }
                }

                IQueryable<Desarrollo> query = null;
                char[] charSeparator = { ',' };
                foreach (var id_superficie in ids_superficies.Split(charSeparator, StringSplitOptions.RemoveEmptyEntries))
                {
                    IEnumerable<Desarrollo> tempResults = null;

                    switch (int.Parse(id_superficie))
                    {
                        case 1:
                            tempResults = listaDesarrollos.Where(x => x.SuperficieConstruccion > 0 && x.SuperficieConstruccion < 101);

                            if (query != null)
                                query = query.Union(tempResults);
                            else
                                query = tempResults.AsQueryable();

                            break;
                        case 2:
                            tempResults = listaDesarrollos.Where(x => x.SuperficieConstruccion >= 101 && x.SuperficieConstruccion < 151);

                            if (query != null)
                                query = query.Union(tempResults);
                            else
                                query = tempResults.AsQueryable();

                            break;
                        case 3:
                            tempResults = listaDesarrollos.Where(x => x.SuperficieConstruccion >= 151 && x.SuperficieConstruccion < 201);

                            if (query != null)
                                query = query.Union(tempResults);
                            else
                                query = tempResults.AsQueryable();

                            break;
                        case 4:
                            tempResults = listaDesarrollos.Where(x => x.SuperficieConstruccion >= 201 && x.SuperficieConstruccion < 301);

                            if (query != null)
                                query = query.Union(tempResults);
                            else
                                query = tempResults.AsQueryable();

                            break;
                        case 5:
                            tempResults = listaDesarrollos.Where(x => x.SuperficieConstruccion >= 301 && x.SuperficieConstruccion < 501);

                            if (query != null)
                                query = query.Union(tempResults);
                            else
                                query = tempResults.AsQueryable();

                            break;
                        case 6:
                            tempResults = listaDesarrollos.Where(x => x.SuperficieConstruccion >= 501);

                            if (query != null)
                                query = query.Union(tempResults);
                            else
                                query = tempResults.AsQueryable();

                            break;
                    }
                }

                Utils.WriteLogMessage(HttpContext.Current.User.Identity.Name, "Acceso a Base de Datos. Se obtiene lista de Desarrollos en base a filtros seleccionados.");
                if (query != null)
                    return query.ToList();
                else
                    return listaDesarrollos;
            }
            catch (Exception ex)
            {
                Utils.WriteLogMessage(HttpContext.Current.User.Identity.Name, ex.Message);
                return null;
            }
        }

        public static IEnumerable<Desarrollo> ObtenerDesarrollosPorPagina<T>(int[] ids, int pagina, int rowsPorPagina, Expression<Func<Desarrollo, T>> ordenacion, string direccion, out int paginas, out int rowsTotales)
        {
            try
            {
                if (pagina < 1)
                    pagina = 1;

                IQueryable<Desarrollo> query = Desarrollo.ObtenerDesarrollos().AsQueryable();

                if (ids.Count() > 0)
                    query = query.Where(i => ids.Contains(i.Id));

                if (direccion == "ASC")
                    query = query.OrderBy(ordenacion);
                else
                    query = query.OrderByDescending(ordenacion);

                rowsTotales = query.ToList().Count;
                paginas = 1;
                if (rowsTotales > 0)
                {
                    paginas = rowsTotales / rowsPorPagina;

                    if ((rowsTotales % rowsPorPagina) > 0)
                        paginas++;
                }

                Utils.WriteLogMessage(HttpContext.Current.User.Identity.Name, "Se obtiene lista de Desarrollos por pagina y numero de renglones.");
                return query.Skip((pagina - 1) * rowsPorPagina)
                            .Take(rowsPorPagina)
                            .ToList();
            }
            catch (Exception ex)
            {
                paginas = 0;
                rowsTotales = 0;

                Utils.WriteLogMessage(HttpContext.Current.User.Identity.Name, ex.Message);
                return null;
            }
        }
    }

    public class DesarrolloDatosGenerales
    {
        public string Titulo { get; set; }

        public string Valor { get; set; }

        public int Ordenacion { get; set; }
    }

    public class DesarrolloServicios
    {
        public string Titulo { get; set; }

        public string Valor { get; set; }

        public int Ordenacion { get; set; }
    }

    public class DesarrolloAmenidades
    {
        public string Titulo { get; set; }

        public string Valor { get; set; }

        public int Ordenacion { get; set; }
    }

    public class DesarrolloPrototipos
    {
        public string Nombre { get; set; }

        public string Tipo { get; set; }

        public decimal Superficie { get; set; }

        public decimal Precio { get; set; }

        public int Recamaras { get; set; }

        public int Banos { get; set; }

        public int Cajones { get; set; }
    }

    public class BusquedaViewModels
    {
        public List<Estado> Estados { get; set; }
        public List<Demarcacion> Demarcaciones { get; set; }
        public List<Segmento> Segmentos { get; set; }
        public List<TipoConstruccion> TiposConstruccion { get; set; }
        public List<Desarrollo> Desarrollos { get; set; }
    }

    public class ListadoViewModels
    {
        public int RowsPorPagina { get; set; }

        public int RowsTotales { get; set; }

        public int PaginaActual { get; set; }

        public int PaginasTotales { get; set; }

        public IEnumerable<Desarrollo> Desarrollos { get; set; }

        public string FiltroIds { get; set; }
    }

    public class DetallesViewModels
    {
        public List<DesarrolloDatosGenerales> ListDatosGenerales { get; set; }

        public List<DesarrolloServicios> ListServicios { get; set; }

        public List<DesarrolloAmenidades> ListAmenidades { get; set; }

        public List<DesarrolloPrototipos> ListPrototipos { get; set; }

        public void CargaListasDetalle(int id_desarrollo, out List<DesarrolloDatosGenerales> ListDatosGenerales,
                                                          out List<DesarrolloServicios> ListServicios,
                                                          out List<DesarrolloAmenidades> ListAmenidades,
                                                          out List<DesarrolloPrototipos> ListPrototipos)
        {
            try
            {
                ListDatosGenerales = new List<DesarrolloDatosGenerales>();
                ListServicios = new List<DesarrolloServicios>();
                ListAmenidades = new List<DesarrolloAmenidades>();
                ListPrototipos = new List<DesarrolloPrototipos>();

                JabuConnectionString jdbConnString = new JabuConnectionString();

                using (SqlConnection cnn = new SqlConnection(jdbConnString.JabuCnnString))
                {
                    cnn.Open();
                    using (SqlCommand cmdDesarrolloDetalles = cnn.CreateCommand())
                    {
                        cmdDesarrolloDetalles.CommandText = "usp_Detalles_Get_DesarrolloDetalles";
                        cmdDesarrolloDetalles.CommandType = CommandType.StoredProcedure;
                        cmdDesarrolloDetalles.Parameters.Add(new SqlParameter("@Id_Desarrollo", id_desarrollo));

                        SqlDataReader dr = cmdDesarrolloDetalles.ExecuteReader();
                        while (dr.Read())
                        {
                            DesarrolloDatosGenerales _DesarrolloDatosGenerales;
                            DesarrolloServicios _DesarrolloServicios;
                            DesarrolloAmenidades _DesarrolloAmenidades;

                            switch (dr.GetString(dr.GetOrdinal("Titulo")))
                            {
                                #region Datos Generales

                                case "Desarrollo_Nombre":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Nombre";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Desarrollo_Direccion":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Dirección";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Desarrollador_Telefono":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Contacto";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Desarrollo_Desarrollador":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Desarrollador";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Desarrollo_Promotor":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Promotor";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Clasificacion_Descripcion":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Segmento";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "TipoConstruccion_Descripcion":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Tipo";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Desarrollo_Prototipos":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Número de Prototipos";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Desarrollo_Torres":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Número de Torres";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Desarrollo_Niveles":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Número de Niveles";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Desarrollo_Totales":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Unidades Totales";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Desarrollo_Vendidas":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Unidades Vendidas";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Desarrollo_Disponibles":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Unidades Disponibles";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Producto_PrecioInicial":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Precio Inicial";
                                    _DesarrolloDatosGenerales.Valor = string.Format("{0:$0,0}", Convert.ToDecimal(dr.GetString(dr.GetOrdinal("Valor"))));
                                    //_DesarrolloDatosGenerales.Valor = string.Format("{0:C}", Convert.ToDecimal(dr.GetString(dr.GetOrdinal("Valor"))));

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Producto_PrecioActualizado":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Precio Actualizado";
                                    _DesarrolloDatosGenerales.Valor = string.Format("{0:$0,0}", Convert.ToDecimal(dr.GetString(dr.GetOrdinal("Valor"))));
                                    //_DesarrolloDatosGenerales.Valor = string.Format("{0:C}", Convert.ToDecimal(dr.GetString(dr.GetOrdinal("Valor"))));

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Producto_PrecioMetroCuadrado":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Precio m²";
                                    _DesarrolloDatosGenerales.Valor = string.Format("{0:$0,0}", Convert.ToDecimal(dr.GetString(dr.GetOrdinal("Valor"))));
                                    //_DesarrolloDatosGenerales.Valor = string.Format("{0:C}", Convert.ToDecimal(dr.GetString(dr.GetOrdinal("Valor"))));

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Producto_SuperficieConstruccion":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Superficie";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Desarrollo_Absorcion":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Absorción";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;

                                #endregion

                                #region Servicios

                                case "Desarrollo_ElevadorCarga":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Elevador de Carga";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_ElevadorCondominos":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Elevador de Condóminos";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_Interfon":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Interfón";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_CircuitoCerrado":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Cicuito Cerrado";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_CasetaVigilancia":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Caseta de Vigilancia";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_Cisterna":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Cisterna";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_PlantaLuz":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Planta de Luz";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_PlantaTratamiento":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Planta de Tratamiento de Aguas Residuales";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_EstacionamientoVisitas":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Estacionamiento de Visitas";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_CuartoBasura":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Cuarto para Basura";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_ManejoDesechos":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Manejo de Desechos";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_CaptacionAgua":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Captación de Aguas";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_Lobby":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Lobby";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_BussinesCenter":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Bussines Center";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_ValetParking":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Valet Parking";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_RampaDiscapacitados":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Rampa para Discapacitados";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;

                                #endregion

                                #region Amenidades

                                case "Desarrollo_AreasVerdes":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Áreas Verdes";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_SalonUsos":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Salón de Usos Múltiples";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_JuegosInfantiles":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Juegos Infantiles";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_Gym":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Gimnasio";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_Alberca":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Alberca";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_CarrilNado":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Carril de Nado";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_SalonJuegos":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Salón de Juegos";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_Spa":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Spa";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_Jacuzzi":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Jacuzzi";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_CanchaTenis":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Cancha de Tenis";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_Paddle":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Paddle";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_Squash":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Áreas Verdes";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_PistaCaminata":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Áreas Verdes";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;

                                #endregion
                            }
                        }
                    }
                    cnn.Close();

                    Utils.WriteLogMessage(HttpContext.Current.User.Identity.Name, "Acceso a Base de Datos. Se obtiene el detalle del Desarrollo seleccionado.");

                    cnn.Open();
                    using (SqlCommand cmdDesarrolloPrototipos = cnn.CreateCommand())
                    {
                        cmdDesarrolloPrototipos.CommandText = "usp_Detalles_Get_DesarrolloPrototipos";
                        cmdDesarrolloPrototipos.CommandType = CommandType.StoredProcedure;
                        cmdDesarrolloPrototipos.Parameters.Add(new SqlParameter("@Id_Desarrollo", id_desarrollo));

                        SqlDataReader drPrototipos = cmdDesarrolloPrototipos.ExecuteReader();
                        while (drPrototipos.Read())
                        {
                            DesarrolloPrototipos _DesarrolloPrototipos = new DesarrolloPrototipos();
                            _DesarrolloPrototipos.Nombre = drPrototipos.GetString(drPrototipos.GetOrdinal("NombrePrototipo"));
                            _DesarrolloPrototipos.Tipo = drPrototipos.GetString(drPrototipos.GetOrdinal("TipoConstruccion"));
                            _DesarrolloPrototipos.Superficie = drPrototipos.GetDecimal(drPrototipos.GetOrdinal("SuperficieConstruccion"));
                            _DesarrolloPrototipos.Precio = drPrototipos.GetDecimal(drPrototipos.GetOrdinal("PrecioPrototipo"));
                            _DesarrolloPrototipos.Recamaras = drPrototipos.GetInt32(drPrototipos.GetOrdinal("Recamaras"));
                            _DesarrolloPrototipos.Banos = drPrototipos.GetInt32(drPrototipos.GetOrdinal("Banos"));
                            _DesarrolloPrototipos.Cajones = drPrototipos.GetInt32(drPrototipos.GetOrdinal("Cajones"));

                            ListPrototipos.Add(_DesarrolloPrototipos);
                        }
                    }
                    cnn.Close();
                    Utils.WriteLogMessage(HttpContext.Current.User.Identity.Name, "Acceso a Base de Datos. Se obtienen los Prototipos del Desarrollo seleccionado.");
                }

                ListServicios = ListServicios.Where(x => x.Valor != "0").ToList();
                ListAmenidades = ListAmenidades.Where(x => x.Valor != "0").ToList();
            }
            catch (Exception ex)
            {
                ListDatosGenerales = null;
                ListServicios = null;
                ListAmenidades = null;
                ListPrototipos = null;

                Utils.WriteLogMessage(HttpContext.Current.User.Identity.Name, ex.Message);
            }
        }
    }

    public class ComparativoViewModels
    {
        public IEnumerable<Tuple<DesarrolloDatosGenerales,
                                 DesarrolloDatosGenerales,
                                 DesarrolloDatosGenerales,
                                 DesarrolloDatosGenerales>> ListDatosGenerales { get; set; }

        public IEnumerable<Tuple<DesarrolloDatosGenerales,
                                 DesarrolloDatosGenerales,
                                 DesarrolloDatosGenerales>> ListDatosGenerales2 { get; set; }

        public IEnumerable<Tuple<DesarrolloServicios,
                                 DesarrolloServicios,
                                 DesarrolloServicios,
                                 DesarrolloServicios>> ListServicios { get; set; }

        public IEnumerable<Tuple<DesarrolloServicios,
                                 DesarrolloServicios,
                                 DesarrolloServicios>> ListServicios2 { get; set; }

        public IEnumerable<Tuple<DesarrolloAmenidades,
                                 DesarrolloAmenidades,
                                 DesarrolloAmenidades,
                                 DesarrolloAmenidades>> ListAmenidades { get; set; }

        public IEnumerable<Tuple<DesarrolloAmenidades,
                                 DesarrolloAmenidades,
                                 DesarrolloAmenidades>> ListAmenidades2 { get; set; }

        public void CargaListasComparativo(int id_desarrollo, out List<DesarrolloDatosGenerales> ListDatosGenerales,
                                                              out List<DesarrolloServicios> ListServicios,
                                                              out List<DesarrolloAmenidades> ListAmenidades)
        {
            try
            {
                ListDatosGenerales = new List<DesarrolloDatosGenerales>();
                ListServicios = new List<DesarrolloServicios>();
                ListAmenidades = new List<DesarrolloAmenidades>();

                JabuConnectionString jdbConnString = new JabuConnectionString();

                using (SqlConnection cnn = new SqlConnection(jdbConnString.JabuCnnString))
                {
                    cnn.Open();

                    using (SqlCommand cmdDesarrolloDetalles = cnn.CreateCommand())
                    {
                        cmdDesarrolloDetalles.CommandText = "usp_Detalles_Get_DesarrolloDetalles";
                        cmdDesarrolloDetalles.CommandType = CommandType.StoredProcedure;
                        cmdDesarrolloDetalles.Parameters.Add(new SqlParameter("@Id_Desarrollo", id_desarrollo));

                        SqlDataReader dr = cmdDesarrolloDetalles.ExecuteReader();
                        while (dr.Read())
                        {
                            DesarrolloDatosGenerales _DesarrolloDatosGenerales;
                            DesarrolloServicios _DesarrolloServicios;
                            DesarrolloAmenidades _DesarrolloAmenidades;

                            switch (dr.GetString(dr.GetOrdinal("Titulo")))
                            {
                                #region Datos Generales

                                case "Desarrollo_Nombre":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Nombre";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloDatosGenerales.Ordenacion = 1;

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Clasificacion_Descripcion":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Segmento";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloDatosGenerales.Ordenacion = 2;

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "TipoConstruccion_Descripcion":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Tipo";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloDatosGenerales.Ordenacion = 3;

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Desarrollo_Prototipos":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Número de Prototipos";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloDatosGenerales.Ordenacion = 4;

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Desarrollo_Torres":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Número de Torres";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloDatosGenerales.Ordenacion = 5;

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Desarrollo_Niveles":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Número de Niveles";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloDatosGenerales.Ordenacion = 6;

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Desarrollo_Totales":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Unidades Totales";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloDatosGenerales.Ordenacion = 7;

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Desarrollo_Vendidas":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Unidades Vendidas";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloDatosGenerales.Ordenacion = 8;

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Desarrollo_Disponibles":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Unidades Disponibles";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloDatosGenerales.Ordenacion = 9;

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Producto_SuperficieConstruccion":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Superficie";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloDatosGenerales.Ordenacion = 10;

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Producto_PrecioMetroCuadrado":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Precio m²";
                                    _DesarrolloDatosGenerales.Valor = string.Format("{0:C}", Convert.ToDecimal(dr.GetString(dr.GetOrdinal("Valor"))));
                                    _DesarrolloDatosGenerales.Ordenacion = 11;

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Producto_PrecioInicial":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Precio Inicial";
                                    _DesarrolloDatosGenerales.Valor = string.Format("{0:C}", Convert.ToDecimal(dr.GetString(dr.GetOrdinal("Valor"))));
                                    _DesarrolloDatosGenerales.Ordenacion = 12;

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Producto_PrecioActualizado":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Precio Actualizado";
                                    _DesarrolloDatosGenerales.Valor = string.Format("{0:C}", Convert.ToDecimal(dr.GetString(dr.GetOrdinal("Valor"))));
                                    _DesarrolloDatosGenerales.Ordenacion = 13;

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Desarrollo_Desarrollador":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Desarrollador";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloDatosGenerales.Ordenacion = 14;

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;

                                #endregion

                                #region Servicios

                                case "Desarrollo_ElevadorCarga":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Elevador de Carga";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 1;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_ElevadorCondominos":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Elevador de Condóminos";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 2;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_Interfon":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Interfón";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 3;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_CircuitoCerrado":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Cicuito Cerrado";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 4;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_CasetaVigilancia":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Caseta de Vigilancia";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 5;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_Cisterna":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Cisterna";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 6;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_PlantaLuz":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Planta de Luz";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 7;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_PlantaTratamiento":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Planta de Tratamiento de Aguas Residuales";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 8;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_EstacionamientoVisitas":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Estacionamiento de Visitas";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 9;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_CuartoBasura":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Cuarto para Basura";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 10;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_ManejoDesechos":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Manejo de Desechos";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 11;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_CaptacionAgua":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Captación de Aguas";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 12;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_Lobby":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Lobby";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 13;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_BussinesCenter":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Bussines Center";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 14;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_ValetParking":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Valet Parking";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 15;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_RampaDiscapacitados":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Rampa para Discapacitados";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 16;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;

                                #endregion

                                #region Amenidades

                                case "Desarrollo_AreasVerdes":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Áreas Verdes";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloAmenidades.Ordenacion = 1;

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_SalonUsos":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Salón de Usos Múltiples";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloAmenidades.Ordenacion = 2;

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_JuegosInfantiles":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Juegos Infantiles";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloAmenidades.Ordenacion = 3;

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_Gym":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Gimnasio";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloAmenidades.Ordenacion = 4;

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_Alberca":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Alberca";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloAmenidades.Ordenacion = 5;

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_CarrilNado":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Carril de Nado";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloAmenidades.Ordenacion = 6;

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_SalonJuegos":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Salón de Juegos";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloAmenidades.Ordenacion = 7;

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_Spa":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Spa";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloAmenidades.Ordenacion = 8;

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_Jacuzzi":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Jacuzzi";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloAmenidades.Ordenacion = 9;

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_CanchaTenis":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Cancha de Tenis";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloAmenidades.Ordenacion = 10;

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_Paddle":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Paddle";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloAmenidades.Ordenacion = 11;

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_Squash":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Áreas Verdes";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloAmenidades.Ordenacion = 12;

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_PistaCaminata":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Áreas Verdes";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloAmenidades.Ordenacion = 13;

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;

                                #endregion
                            }
                        }
                    }
                }

                Utils.WriteLogMessage(HttpContext.Current.User.Identity.Name, "Acceso a Base de Datos. Se obtiene el detalle del Desarrollo seleccionado.");

                ListDatosGenerales = ListDatosGenerales.OrderBy(x => x.Ordenacion).ToList();
                ListServicios = ListServicios.OrderBy(x => x.Ordenacion).ToList();
                ListAmenidades = ListAmenidades.OrderBy(x => x.Ordenacion).ToList();
            }
            catch (Exception ex)
            {
                ListDatosGenerales = null;
                ListServicios = null;
                ListAmenidades = null;

                Utils.WriteLogMessage(HttpContext.Current.User.Identity.Name, ex.Message);
            }
        }

        public void CargaListasComparativo(string ids_desarrollo, out List<DesarrolloDatosGenerales> ListDatosGenerales,
                                                                  out List<DesarrolloServicios> ListServicios,
                                                                  out List<DesarrolloAmenidades> ListAmenidades)
        {
            try
            {
                ListDatosGenerales = new List<DesarrolloDatosGenerales>();
                ListServicios = new List<DesarrolloServicios>();
                ListAmenidades = new List<DesarrolloAmenidades>();

                JabuConnectionString jdbConnString = new JabuConnectionString();

                using (SqlConnection cnn = new SqlConnection(jdbConnString.JabuCnnString))
                {
                    cnn.Open();

                    using (SqlCommand cmdDesarrolloDetalles = cnn.CreateCommand())
                    {
                        cmdDesarrolloDetalles.CommandText = "usp_Detalles_Get_DesarrolloPromedio";
                        cmdDesarrolloDetalles.CommandType = CommandType.StoredProcedure;
                        cmdDesarrolloDetalles.Parameters.Add(new SqlParameter("@Ids_Desarrollos", ids_desarrollo));

                        SqlDataReader dr = cmdDesarrolloDetalles.ExecuteReader();
                        while (dr.Read())
                        {
                            DesarrolloDatosGenerales _DesarrolloDatosGenerales;
                            DesarrolloServicios _DesarrolloServicios;
                            DesarrolloAmenidades _DesarrolloAmenidades;

                            switch (dr.GetString(dr.GetOrdinal("Titulo")))
                            {
                                #region Datos Generales

                                case "Desarrollo_Nombre":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Nombre";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloDatosGenerales.Ordenacion = 1;

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Clasificacion_Descripcion":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Segmento";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloDatosGenerales.Ordenacion = 2;

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "TipoConstruccion_Descripcion":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Tipo";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloDatosGenerales.Ordenacion = 3;

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Desarrollo_Prototipos":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Número de Prototipos";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloDatosGenerales.Ordenacion = 4;

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Desarrollo_Torres":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Número de Torres";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloDatosGenerales.Ordenacion = 5;

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Desarrollo_Niveles":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Número de Niveles";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloDatosGenerales.Ordenacion = 6;

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Desarrollo_Totales":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Unidades Totales";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloDatosGenerales.Ordenacion = 7;

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Desarrollo_Vendidas":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Unidades Vendidas";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloDatosGenerales.Ordenacion = 8;

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Desarrollo_Disponibles":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Unidades Disponibles";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloDatosGenerales.Ordenacion = 9;

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Producto_SuperficieConstruccion":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Superficie";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloDatosGenerales.Ordenacion = 10;

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Producto_PrecioMetroCuadrado":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Precio m²";
                                    _DesarrolloDatosGenerales.Valor = string.Format("{0:C}", Convert.ToDecimal(dr.GetString(dr.GetOrdinal("Valor"))));
                                    _DesarrolloDatosGenerales.Ordenacion = 11;

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Producto_PrecioInicial":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Precio Inicial";
                                    _DesarrolloDatosGenerales.Valor = string.Format("{0:C}", Convert.ToDecimal(dr.GetString(dr.GetOrdinal("Valor"))));
                                    _DesarrolloDatosGenerales.Ordenacion = 12;

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Producto_PrecioActualizado":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Precio Actualizado";
                                    _DesarrolloDatosGenerales.Valor = string.Format("{0:C}", Convert.ToDecimal(dr.GetString(dr.GetOrdinal("Valor"))));
                                    _DesarrolloDatosGenerales.Ordenacion = 13;

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;
                                case "Desarrollo_Desarrollador":
                                    _DesarrolloDatosGenerales = new DesarrolloDatosGenerales();
                                    _DesarrolloDatosGenerales.Titulo = "Desarrollador";
                                    _DesarrolloDatosGenerales.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloDatosGenerales.Ordenacion = 14;

                                    ListDatosGenerales.Add(_DesarrolloDatosGenerales);
                                    break;

                                #endregion

                                #region Servicios

                                case "Desarrollo_ElevadorCarga":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Elevador de Carga";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 1;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_ElevadorCondominos":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Elevador de Condóminos";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 2;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_Interfon":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Interfón";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 3;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_CircuitoCerrado":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Cicuito Cerrado";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 4;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_CasetaVigilancia":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Caseta de Vigilancia";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 5;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_Cisterna":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Cisterna";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 6;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_PlantaLuz":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Planta de Luz";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 7;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_PlantaTratamiento":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Planta de Tratamiento de Aguas Residuales";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 8;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_EstacionamientoVisitas":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Estacionamiento de Visitas";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 9;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_CuartoBasura":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Cuarto para Basura";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 10;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_ManejoDesechos":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Manejo de Desechos";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 11;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_CaptacionAgua":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Captación de Aguas";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 12;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_Lobby":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Lobby";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 13;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_BussinesCenter":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Bussines Center";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 14;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_ValetParking":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Valet Parking";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 15;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;
                                case "Desarrollo_RampaDiscapacitados":
                                    _DesarrolloServicios = new DesarrolloServicios();
                                    _DesarrolloServicios.Titulo = "Rampa para Discapacitados";
                                    _DesarrolloServicios.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloServicios.Ordenacion = 16;

                                    ListServicios.Add(_DesarrolloServicios);
                                    break;

                                #endregion

                                #region Amenidades

                                case "Desarrollo_AreasVerdes":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Áreas Verdes";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloAmenidades.Ordenacion = 1;

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_SalonUsos":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Salón de Usos Múltiples";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloAmenidades.Ordenacion = 2;

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_JuegosInfantiles":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Juegos Infantiles";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloAmenidades.Ordenacion = 3;

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_Gym":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Gimnasio";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloAmenidades.Ordenacion = 4;

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_Alberca":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Alberca";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloAmenidades.Ordenacion = 5;

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_CarrilNado":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Carril de Nado";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloAmenidades.Ordenacion = 6;

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_SalonJuegos":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Salón de Juegos";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloAmenidades.Ordenacion = 7;

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_Spa":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Spa";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloAmenidades.Ordenacion = 8;

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_Jacuzzi":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Jacuzzi";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloAmenidades.Ordenacion = 9;

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_CanchaTenis":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Cancha de Tenis";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloAmenidades.Ordenacion = 10;

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_Paddle":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Paddle";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloAmenidades.Ordenacion = 11;

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_Squash":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Áreas Verdes";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloAmenidades.Ordenacion = 12;

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;
                                case "Desarrollo_PistaCaminata":
                                    _DesarrolloAmenidades = new DesarrolloAmenidades();
                                    _DesarrolloAmenidades.Titulo = "Áreas Verdes";
                                    _DesarrolloAmenidades.Valor = dr.GetString(dr.GetOrdinal("Valor"));
                                    _DesarrolloAmenidades.Ordenacion = 13;

                                    ListAmenidades.Add(_DesarrolloAmenidades);
                                    break;

                                #endregion
                            }
                        }
                    }
                }

                Utils.WriteLogMessage(HttpContext.Current.User.Identity.Name, "Acceso a Base de Datos. Se obtiene el detalle promedio de los Desarrollos correspondientes.");

                ListDatosGenerales = ListDatosGenerales.OrderBy(x => x.Ordenacion).ToList();
                ListServicios = ListServicios.OrderBy(x => x.Ordenacion).ToList();
                ListAmenidades = ListAmenidades.OrderBy(x => x.Ordenacion).ToList();
            }
            catch (Exception ex)
            {
                ListDatosGenerales = null;
                ListServicios = null;
                ListAmenidades = null;

                Utils.WriteLogMessage(HttpContext.Current.User.Identity.Name, ex.Message);
            }
        }
    }

    public static class Utils
    {
        public static IEnumerable<Tuple<T, U, V, Z>> CombineWith<T, U, V, Z>(this IEnumerable<T> first, IEnumerable<U> second, IEnumerable<V> third, IEnumerable<Z> fourth)
        {
            using (var firstEnumerator = first.GetEnumerator())
            using (var secondEnumerator = second.GetEnumerator())
            using (var thirdEnumerator = third.GetEnumerator())
            using (var fourthEnumerator = fourth.GetEnumerator())
            {
                bool hasFirst = true;
                bool hasSecond = true;
                bool hasThird = true;
                bool hasFourth = true;

                while (
                    // Only call MoveNext if it didn't fail last time.
                    (hasFirst && (hasFirst = firstEnumerator.MoveNext()))
                    | // WARNING: Do NOT change to ||.
                    (hasSecond && (hasSecond = secondEnumerator.MoveNext()))
                    |
                    (hasThird && (hasThird = thirdEnumerator.MoveNext()))
                    |
                    (hasFourth && (hasFourth = fourthEnumerator.MoveNext()))
                    )
                {
                    yield return Tuple.Create(
                            hasFirst ? firstEnumerator.Current : default(T),
                            hasSecond ? secondEnumerator.Current : default(U),
                            hasThird ? thirdEnumerator.Current : default(V),
                            hasFourth ? fourthEnumerator.Current : default(Z)
                        );
                }
            }
        }

        public static IEnumerable<Tuple<T, U, V>> CombineWith2<T, U, V>(this IEnumerable<T> first, IEnumerable<U> second, IEnumerable<V> third)
        {
            using (var firstEnumerator = first.GetEnumerator())
            using (var secondEnumerator = second.GetEnumerator())
            using (var thirdEnumerator = third.GetEnumerator())
            {
                bool hasFirst = true;
                bool hasSecond = true;
                bool hasThird = true;

                while (
                    // Only call MoveNext if it didn't fail last time.
                    (hasFirst && (hasFirst = firstEnumerator.MoveNext()))
                    | // WARNING: Do NOT change to ||.
                    (hasSecond && (hasSecond = secondEnumerator.MoveNext()))
                    |
                    (hasThird && (hasThird = thirdEnumerator.MoveNext()))
                    )
                {
                    yield return Tuple.Create(
                            hasFirst ? firstEnumerator.Current : default(T),
                            hasSecond ? secondEnumerator.Current : default(U),
                            hasThird ? thirdEnumerator.Current : default(V)
                    );
                }
            }
        }

        public static void WriteLogMessage(string user, string message)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Log");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path = Path.Combine(path, "JabuEventLog.txt");

            if (!File.Exists(path))
                File.Create(path).Dispose();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("==============================================================================");
            sb.AppendLine("Fecha: [" + DateTime.Now.ToString() + "]");
            sb.AppendLine("Usuario: " + user);
            sb.AppendLine("Mensaje: " + message);
            sb.AppendLine("==============================================================================");

            using (StreamWriter outfile = new StreamWriter(path, true))
            {
                outfile.Write(sb.ToString());
            }
        }
    }
}